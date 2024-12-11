using System.Collections.Generic;
using Code.GameEntities;
using System.Linq;
using Code.Enums;
using UnityEngine;

namespace Code.GameRules
{
    public class CombinationComparer
    {
        private CombinationEvaluator combinationEvaluator;

        public CombinationComparer(CombinationEvaluator combinationEvaluator)
        {
            this.combinationEvaluator = combinationEvaluator;
        }
        
        public CombinationRank FindStrongestCombinationRank(List<List<Card>> combinations)
        {
            CombinationRank bestCombinationRank = CombinationRank.HighCard;
            List<Card> bestHand = null;

            foreach (var hand in combinations)
            {
                if (hand == null || !hand.Any())
                {
                    Debug.LogWarning("Skipping invalid or empty hand.");
                    continue;
                }

                var rank = combinationEvaluator.EvaluateBestCardCombinationRank(hand);

                if (rank > bestCombinationRank)
                {
                    bestCombinationRank = rank;
                    bestHand = hand;
                }
                else if (rank == bestCombinationRank)
                {
                    if (bestHand != null && CompareSameRankCombination(hand, bestHand) > 0)
                    {
                        bestHand = hand;
                    }
                }
            }

            return bestCombinationRank;
        }

        public List<Card> FindStrongestCombinationCards(List<List<Card>> combinations)
        {
            CombinationRank bestHandRank = combinationEvaluator.EvaluateBestCardCombinationRank(combinations[0]);
            List<Card> bestCombination = combinations[0];

            foreach (var combination in combinations)
            {
                if (combination == null || combination.Count != 5)
                {
                    Debug.LogWarning("Skipping invalid or empty combination.");
                    continue;
                }

                var rank = combinationEvaluator.EvaluateBestCardCombinationRank(combination);

                if (rank > bestHandRank)
                {
                    bestHandRank = rank;
                    bestCombination = combination;
                }
                else if (rank == bestHandRank)
                {
                    if (bestCombination != null && CompareSameRankCombination(combination, bestCombination) > 0)
                    {
                        bestCombination = combination;
                    }
                }
            }

            if (bestCombination == null)
            {
                Debug.LogWarning("Strongest C");
            }

            return bestCombination;
        }

        private int CompareSameRankCombination(List<Card> combination1, List<Card> combination2)
        {
            if (combination1 == null || combination2 == null)
            {
                Debug.LogError("One or both combinations are null.");
                return 0;
            }

            if (!combination1.Any() || !combination2.Any())
            {
                Debug.LogError("One or both combinations are empty.");
                return 0;
            }

            var sortedHand1 = combination1.OrderByDescending(c => (int)c.rank).ToList();
            var sortedHand2 = combination2.OrderByDescending(c => (int)c.rank).ToList();

            for (int i = 0; i < Mathf.Min(sortedHand1.Count, sortedHand2.Count); i++)
            {
                if ((int)sortedHand1[i].rank > (int)sortedHand2[i].rank)
                    return 1;
                if ((int)sortedHand1[i].rank < (int)sortedHand2[i].rank)
                    return -1;
            }
            
            return 0;
        }
    }
}
