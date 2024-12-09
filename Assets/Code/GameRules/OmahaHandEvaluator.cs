using System.Collections.Generic;
using System.Linq;
using Code.GameEntities;
using Code.GameRules;
using Code.Enums;
using System;

namespace Code.GameLogic
{
    public class OmahaHandEvaluator
    {
        private CombinationEvaluator combinationEvaluator;
        private CombinationComparer combinationComparer;

        public OmahaHandEvaluator()
        {
            combinationEvaluator = new CombinationEvaluator();
            combinationComparer = new CombinationComparer(combinationEvaluator);
        }

        public CombinationRank FindBestOmahaCombination(List<Card> tableCards, List<Card> handCards)
        {
            if (tableCards.Count != 5 || handCards.Count != 4)
                throw new ArgumentException("Must be 5 card on table and 4 in hand");

            var allCombinations = GenerateAllValidCombinations(tableCards, handCards);

            return combinationComparer.FindStrongestCombinationRank(allCombinations);
        }
        
        public List<Card> FindBestOmahaCombinationCards(List<Card> tableCards, List<Card> handCards)
        {
            if (tableCards.Count != 5 || handCards.Count != 4)
                throw new ArgumentException("Must be 5 cards on table and 4 in hand");

            var allCombinations = GenerateAllValidCombinations(tableCards, handCards);

            var bestCombination = combinationComparer.FindStrongestCombinationCards(allCombinations);

            return bestCombination;
        }

        private List<List<Card>> GenerateAllValidCombinations(List<Card> tableCards, List<Card> handCards)
        {
            var tableCombinations = GetCombinations(tableCards, 3);
            var handCombinations = GetCombinations(handCards, 2);

            var validHands = new List<List<Card>>();

            foreach (var tableCombo in tableCombinations)
            {
                foreach (var handCombo in handCombinations)
                {
                    var combinedHand = tableCombo.Concat(handCombo).ToList();
                    if (combinedHand.Count == 5)
                        validHands.Add(combinedHand);
                }
            }

            return validHands;
        }

        private List<List<Card>> GetCombinations(List<Card> cards, int count)
        {
            var result = new List<List<Card>>();
            FindAllCombinations(cards, count, 0, new List<Card>(), result);
            return result;
        }

        private void FindAllCombinations(List<Card> cards, int count, int index, List<Card> current, List<List<Card>> result)
        {
            if (current.Count == count)
            {
                result.Add(new List<Card>(current));
                return;
            }

            for (int i = index; i < cards.Count; i++)
            {
                current.Add(cards[i]);
                FindAllCombinations(cards, count, i + 1, current, result);
                current.RemoveAt(current.Count - 1);
            }
        }
    }
}
