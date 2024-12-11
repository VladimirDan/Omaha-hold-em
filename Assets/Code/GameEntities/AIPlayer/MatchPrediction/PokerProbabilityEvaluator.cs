using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Code.GameEntities;
using Code.Enums;
using Code.GameEntities.Player;
using Code.GameRules;
using Code.GameEntities.AIPlayer;

namespace Code.GameAI
{
    public class PokerProbabilityEvaluator
    {
        private OmahaHandEvaluator omahaHandEvaluator = new OmahaHandEvaluator();
        private CombinationComparer combinationComparer = new CombinationComparer(new CombinationEvaluator());
        private Dictionary<PlayerModel, List<Card>> playersCombinations = new Dictionary<PlayerModel, List<Card>>();

        public MatchOutcomeProbabilities SimulateMatchOutcomes(
            PlayerModel player,
            List<PlayerModel> players,
            List<Card> communityCards,
            int opponentsCount,
            int simulationsCount)
        {
            int winCount = 0;
            int loseCount = 0;
            int tieCount = 0;

            List<Card> playerHand = player.hand.cardSet.Cards;
            if (playerHand.Count == 0)
            {
                return new MatchOutcomeProbabilities(winCount, loseCount, tieCount);
            }

            for (int i = 0; i < simulationsCount; i++)
            {
                Deck deck = new Deck();
                deck.ExcludeCardsFromDeck(playerHand.Concat(communityCards).ToList());
                deck.Shuffle();

                var simulatedCommunityCards = GetSimulatedCommunityCards(deck, communityCards);
                var simulatedHands = DealRandomHands(deck, opponentsCount, 4);

                playersCombinations.Clear();

                playersCombinations[player] =
                    omahaHandEvaluator.FindBestOmahaCombinationCards(simulatedCommunityCards,
                        player.hand.cardSet.Cards);

                var opponents = players.Where(x => x != player).ToList();
                for (int j = 0; j < opponents.Count; j++)
                {
                    playersCombinations[opponents[j]] =
                        omahaHandEvaluator.FindBestOmahaCombinationCards(simulatedCommunityCards, simulatedHands[j]);
                }

                var winners = DetermineWinners(playersCombinations);

                if (winners.Contains(player))
                {
                    if (winners.Count > 1)
                    {
                        tieCount++;
                    }
                    else
                    {
                        winCount++;
                    }
                }
                else
                {
                    loseCount++;
                }
            }

            float winChance = (float)winCount / simulationsCount;
            float loseChance = (float)loseCount / simulationsCount;
            float tieChance = (float)tieCount / simulationsCount;

            return new MatchOutcomeProbabilities(winChance, loseChance, tieChance);
        }

        private List<Card> GetSimulatedCommunityCards(Deck deck, List<Card> currentCommunityCards)
        {
            var simulatedCards = new List<Card>(currentCommunityCards);
            ;
            int cardsToAdd = 5 - currentCommunityCards.Count;

            for (int i = 0; i < cardsToAdd; i++)
            {
                simulatedCards.Add(deck.DrawCard());
            }
            
            if(simulatedCards.Count != 5)
                Debug.LogWarning("fff");
            return simulatedCards;
        }

        private List<List<Card>> DealRandomHands(Deck deck, int opponentCount, int cardsPerOpponent)
        {
            List<List<Card>> opponentSimulatedHands = new List<List<Card>>();

            for (int i = 0; i < opponentCount; i++)
            {
                var hand = new List<Card>();
                for (int j = 0; j < cardsPerOpponent; j++)
                {
                    hand.Add(deck.DrawCard());
                }
                
                opponentSimulatedHands.Add(hand);
            }
            if(opponentSimulatedHands.Count != 4)
                Debug.LogWarning("fff");
            return opponentSimulatedHands;
        }

        private List<PlayerModel> DetermineWinners(Dictionary<PlayerModel, List<Card>> playersCombinations)
        {
            //Debug.Log(string.Join(", ", playersCombinations.Values.ToList()));
            var allCombinations = new List<List<Card>>(playersCombinations.Values);

            var strongestCombination = combinationComparer.FindStrongestCombinationCards(allCombinations);

            var winners = playersCombinations
                .Where(pair => pair.Value.SequenceEqual(strongestCombination))
                .Select(pair => pair.Key)
                .ToList();

            return winners;
        }
    }
}