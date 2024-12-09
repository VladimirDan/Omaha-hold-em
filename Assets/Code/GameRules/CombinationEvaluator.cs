using System.Collections.Generic;
using Code.GameEntities;
using System.Linq;
using Code.Enums;
using System;

namespace Code.GameRules
{
    public class CombinationEvaluator
    {
        public CombinationRank EvaluateBestCardCombinationRank(List<Card> cards)
        {
            if (cards.Count != 5)
                throw new ArgumentException("Exactly 5 cards are required to evaluate a hand.");

            if (IsRoyalFlush(cards, out var royalFlush)) return CombinationRank.RoyalFlush;
            if (IsStraightFlush(cards, out var straightFlush)) return CombinationRank.StraightFlush;
            if (IsFourOfAKind(cards, out var fourOfAKind)) return CombinationRank.FourOfAKind;
            if (IsFullHouse(cards, out var fullHouse)) return CombinationRank.FullHouse;
            if (IsFlush(cards, out var flush)) return CombinationRank.Flush;
            if (IsStraight(cards, out var straight)) return CombinationRank.Straight;
            if (IsThreeOfAKind(cards, out var threeOfAKind)) return CombinationRank.ThreeOfAKind;
            if (IsTwoPair(cards, out var twoPair)) return CombinationRank.TwoPair;
            if (IsOnePair(cards, out var onePair)) return CombinationRank.OnePair;

            return CombinationRank.HighCard;
        }

        private bool IsRoyalFlush(List<Card> cards, out List<Card> combination)
        {
            combination = null;
            if (IsStraightFlush(cards, out var straightFlush) && straightFlush.Max(c => (int)c.rank) == 14)
            {
                combination = straightFlush;
                return true;
            }

            return false;
        }

        private bool IsStraightFlush(List<Card> cards, out List<Card> combination)
        {
            combination = null;
            if (IsFlush(cards, out var flush) && IsStraight(flush, out var straight))
            {
                combination = straight;
                return true;
            }

            return false;
        }

        private bool IsFourOfAKind(List<Card> cards, out List<Card> combination)
        {
            combination = null;
            var grouped = cards.GroupBy(c => c.rank).FirstOrDefault(g => g.Count() == 4);
            if (grouped != null)
            {
                combination = grouped.ToList();
                combination.Add(cards.First(c => c.rank != grouped.Key));
                return true;
            }

            return false;
        }

        private bool IsFullHouse(List<Card> cards, out List<Card> combination)
        {
            combination = null;
            var threeOfAKind = cards.GroupBy(c => c.rank).FirstOrDefault(g => g.Count() == 3);
            var pair = cards.GroupBy(c => c.rank).FirstOrDefault(g => g.Count() == 2);

            if (threeOfAKind != null && pair != null)
            {
                combination = threeOfAKind.ToList().Concat(pair).ToList();
                return true;
            }

            return false;
        }

        private bool IsFlush(List<Card> cards, out List<Card> combination)
        {
            combination = null;
            if (cards.All(c => c.suit == cards[0].suit))
            {
                combination = cards.OrderByDescending(c => (int)c.rank).ToList();
                return true;
            }

            return false;
        }

        private bool IsStraight(List<Card> cards, out List<Card> combination)
        {
            combination = null;
            var ordered = cards.Select(c => (int)c.rank).Distinct().OrderBy(x => x).ToList();

            if (ordered.Count != 5) return false;

            if (ordered[4] - ordered[0] == 4)
            {
                combination = cards.Where(c => ordered.Contains((int)c.rank)).ToList();
                return true;
            }

            if (ordered.SequenceEqual(new List<int> { (int)CardRank.Two, (int)CardRank.Three, (int)CardRank.Four, (int)CardRank.Five, (int)CardRank.Ace }))
            {
                combination = cards.Where(c =>
                    c.rank == CardRank.Ace ||
                    c.rank == CardRank.Two ||
                    c.rank == CardRank.Three ||
                    c.rank == CardRank.Four ||
                    c.rank == CardRank.Five
                ).ToList();
                return true;
            }

            return false;
        }

        private bool IsThreeOfAKind(List<Card> cards, out List<Card> combination)
        {
            combination = null;
            var grouped = cards.GroupBy(c => c.rank).FirstOrDefault(g => g.Count() == 3);
            if (grouped != null)
            {
                combination = grouped.ToList().Concat(cards.Where(c => c.rank != grouped.Key)).ToList();
                return true;
            }

            return false;
        }

        private bool IsTwoPair(List<Card> cards, out List<Card> combination)
        {
            combination = null;
            var pairs = cards.GroupBy(c => c.rank).Where(g => g.Count() == 2).ToList();
            if (pairs.Count == 2)
            {
                combination = pairs.SelectMany(g => g).Concat(cards.Where(c => !pairs.Any(p => p.Key == c.rank)))
                    .ToList();
                return true;
            }

            return false;
        }

        private bool IsOnePair(List<Card> cards, out List<Card> combination)
        {
            combination = null;
            var pair = cards.GroupBy(c => c.rank).FirstOrDefault(g => g.Count() == 2);
            if (pair != null)
            {
                combination = pair.ToList().Concat(cards.Where(c => c.rank != pair.Key)).ToList();
                return true;
            }

            return false;
        }
    }
}