using System.Collections.Generic;
using Code.GameEntities.Player;
using UnityEngine;
using Code.Enums;
using System.Linq;
using Code.GameRules;
using System;

namespace Code.GameEntities.Pot
{
    public class PotManager
    {
        private CombinationComparer combinationComparer;

        public PotManager(CombinationComparer comparer)
        {
            combinationComparer = comparer;
        }

        public Dictionary<PlayerModel, (int, List<Card>)> GatherPlayerBetsAndCombinations(
            List<PlayerModel> players, Dictionary<PlayerModel, ChipsManager> playersBets,
            Dictionary<PlayerModel, List<Card>> playersCombinations)
        {
            var playerBetsAndCombinations = new Dictionary<PlayerModel, (int, List<Card>)>();

            foreach (var player in players)
            {
                if (playersBets.ContainsKey(player) && playersCombinations.ContainsKey(player))
                {
                    int playerBet = playersBets[player].TotalChips;
                    var playerCombination = playersCombinations[player];
                    playerBetsAndCombinations[player] = (playerBet, playerCombination);
                }
            }

            return playerBetsAndCombinations;
        }

        public List<ChipPot> DivideChipPots(
            Dictionary<PlayerModel, (int, List<Card>)> playerBetsAndCombinations, int totalPot)
        {
            var sortedPlayers = playerBetsAndCombinations.OrderBy(x => x.Value.Item1).ToList();
            var chipPots = new List<ChipPot>();
            int previousBet = 0;

            foreach (var player in sortedPlayers)
            {
                int currentBet = player.Value.Item1;

                if (currentBet > previousBet)
                {
                    int eligiblePlayersCount = sortedPlayers.Count(x => x.Value.Item1 >= currentBet);

                    chipPots.Add(new ChipPot
                    {
                        TotalAmount = (currentBet - previousBet) * eligiblePlayersCount,
                        EligiblePlayers = sortedPlayers.Where(x => x.Value.Item1 >= currentBet).Select(x => x.Key)
                            .ToList()
                    });

                    previousBet = currentBet;
                }
            }

            int calculatedTotalPot = chipPots.Sum(pot => pot.TotalAmount);
            if (calculatedTotalPot < totalPot)
            {
                int missingAmount = totalPot - calculatedTotalPot;
                chipPots.Last().TotalAmount += missingAmount;
            }

            return chipPots;
        }

        public void AllocateChipsFromPots(
            List<ChipPot> chipPots,
            Dictionary<PlayerModel, (int, List<Card>)> playerBetsAndCombinations)
        {
            foreach (var pot in chipPots)
            {
                if (pot.EligiblePlayers.Count > 0)
                {
                    var winningCombination = combinationComparer.FindStrongestCombinationCards(
                        pot.EligiblePlayers.Select(x => playerBetsAndCombinations[x].Item2).ToList());

                    var winners = pot.EligiblePlayers.Where(player =>
                        playerBetsAndCombinations[player].Item2.SequenceEqual(winningCombination)).ToList();

                    if (winners.Count > 0)
                    {
                        int maxPayoutPerPlayer = pot.EligiblePlayers.Min(player =>
                            playerBetsAndCombinations[player].Item1 * pot.EligiblePlayers.Count);

                        int payoutShare = Math.Min(pot.TotalAmount / winners.Count, maxPayoutPerPlayer);
                        int remainingChips = pot.TotalAmount - payoutShare * winners.Count;

                        foreach (var winner in winners)
                        {
                            winner.stackChipsManager.TotalChips += payoutShare;
                        }

                        if (remainingChips > 0)
                        {
                            winners[UnityEngine.Random.Range(0, winners.Count)].stackChipsManager.TotalChips += remainingChips;
                        }
                    }
                }
            }
        }

        public void DistributePotChips(
            List<PlayerModel> players,
            Dictionary<PlayerModel, ChipsManager> playersBets,
            Dictionary<PlayerModel, List<Card>> playersCombinations,
            int totalPot)
        {
            var playerBetsAndCombinations = GatherPlayerBetsAndCombinations(players, playersBets, playersCombinations);

            if (playerBetsAndCombinations.Count == 0)
            {
                Debug.LogWarning("Нет игроков с активными ставками.");
                return;
            }

            var chipPots = DivideChipPots(playerBetsAndCombinations, totalPot);

            AllocateChipsFromPots(chipPots, playerBetsAndCombinations);
        }
    }
}