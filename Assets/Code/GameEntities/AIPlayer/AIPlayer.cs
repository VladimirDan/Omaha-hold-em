using Code.GameEntities.AIPlayer.AIDecisionStrategy;
using System.Collections;
using System.Collections.Generic;
using Code.GameEntities.Player;
using UnityEngine;
using Code.Enums;
using Code.GameRules;
using Code.GameAI;
using Code.GameEntities.AIPlayer.AIDecisionStrategy.Strategies;
using System.Linq;
using Code.GameEntities;
using System;

namespace Code.GameEntities.AIPlayer
{
    public class AIPlayer : MonoBehaviour
    {
        [SerializeField] private PlayerModel player;
        [SerializeField] private PokerTable pokerTable;
        private OmahaHandEvaluator omahaHandEvaluator = new OmahaHandEvaluator();
        private PokerProbabilityEvaluator pokerProbabilityEvaluator = new PokerProbabilityEvaluator();
        private int waitTime = 0;
        [SerializeField] private IPlayerDecisionStrategy playerDecisionStrategy;
        private int simulationsCount = 100;
        [SerializeField] private MatchOutcomeProbabilities outcomeProbabilities;
        public string strategyType;

        public void Initialize(PokerTable pokerTable)
        {
            if (player != null && player.playerActionsManager != null)
            {
                player.playerActionsManager.OnPlayerBetTurn += OnPlayerBetTurn;
                player.playerActionsManager.OnCombinationRevealTurn += OnCombinationRevealTurn;
            }

            this.pokerTable = pokerTable;
        }

        private void OnDestroy()
        {
            if (player != null && player.playerActionsManager != null)
            {
                player.playerActionsManager.OnPlayerBetTurn -= OnPlayerBetTurn;
            }
        }

        private void OnPlayerBetTurn(BetType betType)
        {
            StartCoroutine(MakeDecision(betType));
        }

        private void OnCombinationRevealTurn()
        {
            StartCoroutine(SubmitBestCombination());
        }

        public IEnumerator SubmitBestCombination()
        {
            yield return new WaitForSeconds(waitTime);

            player.playerActionsManager.cardSelectionController.selectedCards = FindBestCombination();
            player.playerActionsManager.SubmitCombination(player);
        }

        public List<Card> FindBestCombination()
        {
            List<Card> tableCards = player.playerActionsManager.pokerTable.communityCards.Cards;
            List<Card> playerCards = player.hand.cardSet.Cards;
            return omahaHandEvaluator.FindBestOmahaCombinationCards(tableCards, playerCards);
        }

        public IEnumerator ActOnSBBlind()
        {
            yield return new WaitForSeconds(waitTime);

            var pokerTable = player.playerActionsManager.pokerTable;
            int currentBet = pokerTable.currentBet.TotalChips;

            if (pokerTable.GetPlayerBet(player) < currentBet)
            {
                player.playerActionsManager.Call(player);
            }
            else
            {
                player.playerActionsManager.Check(player);
            }
        }

        public IEnumerator AlwaysFold()
        {
            yield return new WaitForSeconds(waitTime);

            player.playerActionsManager.Fold(player);
        }

        public IEnumerator MakeDecision(BetType betType)
        {
            yield return new WaitForSeconds(waitTime);

            int opponentCount = pokerTable.playersInGame.Count - 1;

            outcomeProbabilities = pokerProbabilityEvaluator.SimulateMatchOutcomes(
                player,
                pokerTable.playersInGame,
                pokerTable.communityCards.Cards,
                opponentCount,
                simulationsCount);

            float chanceToWinOrTie = outcomeProbabilities.TieChance + outcomeProbabilities.WinChance;
            playerDecisionStrategy = SelectStrategy(chanceToWinOrTie, player, betType);
            string fullTypeName = playerDecisionStrategy.GetType().ToString();
            strategyType = fullTypeName.Substring(fullTypeName.LastIndexOf('.') + 1);
            playerDecisionStrategy.ExecuteStrategy(pokerTable, player, chanceToWinOrTie);
        }

        public IPlayerDecisionStrategy SelectStrategy(float chanceToWin, PlayerModel player, BetType betType)
        {
            bool isBetOverThirtyPercent = (float)player.betChipsManager.TotalChips /
                (player.stackChipsManager.TotalChips + player.betChipsManager.TotalChips) > 0.3f;

            if (betType == BetType.BigBlindBet || betType == BetType.SmallBlindBet)
            {
                return new BlindStageStrategy();
            }
            
            if (playerDecisionStrategy is BluffStrategy || (chanceToWin < 0.1f && UnityEngine.Random.value < 0.1f))
            {
                return new BluffStrategy();
            }

            if (chanceToWin >= 0.5f)
            {
                return new OpponentGrindingStrategy();
            }

            if (chanceToWin >= 0.1f && chanceToWin <= 0.3f)
            {
                return new PassiveStrategy();
            }

            if (chanceToWin < 0.1f)
            {
                return new ChipDefenseStrategy();
            }

            if (chanceToWin >= 0.3f && chanceToWin < 0.5f)
            {
                return new MixedStrategy();
            }

            if (chanceToWin >= 0.2f && chanceToWin < 0.3f && isBetOverThirtyPercent)
            {
                return new GreedyStrategy();
            }

            Debug.LogError("Cant chose strat");
            return null;
        }
    }
}