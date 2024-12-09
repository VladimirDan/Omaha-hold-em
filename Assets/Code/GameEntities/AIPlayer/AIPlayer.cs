using System.Collections;
using System.Collections.Generic;
using Code.GameEntities.Player;
using UnityEngine;
using Code.Enums;
using Code.GameLogic;
using System.Collections.Generic;
using System.Linq;
using Code.GameEntities;

namespace Code.GameEntities.AIPlayer
{
    public class AIPlayer : MonoBehaviour
    {
        [SerializeField] private PlayerModel player;
        private OmahaHandEvaluator omahaHandEvaluator = new OmahaHandEvaluator();
        private int waitTime = 0;

        public void Initialize()
        {
            if (player != null && player.playerActionsManager != null)
            {
                player.playerActionsManager.OnPlayerBetTurn += OnPlayerBetTurn;
                player.playerActionsManager.OnCombinationRevealTurn += OnCombinationRevealTurn;
            }
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
            switch (betType)
            {
                case BetType.SmallBlindBet:
                    StartCoroutine(ActOnSBBlind());
                    break;

                case BetType.BigBlindBet:
                    StartCoroutine(ActOnSBBlind());
                    break;

                case BetType.RegularBet:
                    StartCoroutine(ActOnSBBlind());
                    break;

                case BetType.NonRaiseBet:
                    StartCoroutine(ActOnSBBlind());
                    break;

                default:
                    Debug.LogError("Unknown game stage.");
                    break;
            }
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
                Debug.Log($"{player.name} делает call.");
                player.playerActionsManager.Call(player);
            }
            else
            {
                player.playerActionsManager.Check(player);
            }

            //Debug.Log($"{player.name} завершил свой ход.");
        }

        public IEnumerator AlwaysFold()
        {
            yield return new WaitForSeconds(waitTime);
            
            player.playerActionsManager.Fold(player);
        }
    }
}