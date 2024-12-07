using System.Collections;
using System.Collections.Generic;
using Code.GameEntities.Player;
using UnityEngine;
using Code.Enums;
using Unity.VisualScripting;

namespace Code.GameEntities.AIPlayer
{
    public class AIPlayer : MonoBehaviour
    {
        [SerializeField] private PlayerModel player;

        public void Initialize()
        {
            if (player != null && player.playerActionsManager != null)
            {
                player.playerActionsManager.OnPlayerTurnChanged += OnPlayerTurn;
            }
        }

        private void OnDestroy()
        {
            if (player != null && player.playerActionsManager != null)
            {
                player.playerActionsManager.OnPlayerTurnChanged -= OnPlayerTurn;
            }
        }

        private void OnPlayerTurn(GameStage gameStage)
        {
            switch (gameStage)
            {
                case GameStage.SmallBlindBet:
                    StartCoroutine(ActOnSBBlind());
                    break;

                case GameStage.BigBlindBet:
                    StartCoroutine(ActOnSBBlind());
                    break;

                case GameStage.RegularAction:
                    StartCoroutine(ActOnSBBlind());
                    break;

                default:
                    Debug.LogError("Unknown game stage.");
                    break;
            }
        }

        public IEnumerator ActOnSBBlind()
        {
            yield return new WaitForSeconds(2);
            
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

            Debug.Log($"{player.name} завершил свой ход.");
        }
    }
}