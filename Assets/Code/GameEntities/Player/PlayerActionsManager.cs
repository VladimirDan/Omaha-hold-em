using UnityEngine;
using System;
using Code.Enums;

namespace Code.GameEntities.Player
{
    public class PlayerActionsManager : MonoBehaviour
    {
        [SerializeField] public PokerTable pokerTable;
        public bool hasMadeAction = false;
        private bool isPlayerTurn = false;
        public event Action<GameStage> OnPlayerTurnChanged;

        public void TogglePlayerTurn(GameStage gameStage)
        {
            isPlayerTurn = !isPlayerTurn;
            if(isPlayerTurn) OnPlayerTurnChanged?.Invoke(gameStage);
        }
        
        public void TogglePlayerTurn()
        {
            isPlayerTurn = !isPlayerTurn;
        }

        private bool IsPlayerTurn(PlayerModel currentPlayer)
        {
            if (!isPlayerTurn)
            {
                Debug.LogError($"{currentPlayer.name} cannot act because it's not their turn.");
                return false;
            }

            return true;
        }

        public void Fold(PlayerModel currentPlayer)
        {
            if (!CanFold(currentPlayer)) return;

            Debug.Log($"{currentPlayer.name} has folded.");
            pokerTable.RemovePlayerFromActivePlayers(currentPlayer);
            currentPlayer.hand.cardsView.HighlightAllCardsWithOnFoldTransparency();
            hasMadeAction = true;
        }

        public void Call(PlayerModel currentPlayer)
        {
            if (!CanCall(currentPlayer)) return;

            int currentPlayerBet = pokerTable.GetPlayerBet(currentPlayer);
            int callAmount = pokerTable.currentBet.TotalChips - currentPlayerBet;

            if (callAmount > 0)
            {
                currentPlayer.stackChipsManager.RemoveChips(callAmount);
                pokerTable.SetPlayerBet(currentPlayer, pokerTable.currentBet.TotalChips);
                pokerTable.AddToPot(callAmount);
                Debug.Log(
                    $"{currentPlayer.name} has called with {callAmount} chips. Remaining chips: {currentPlayer.stackChipsManager.TotalChips}");
            }
            else
            {
                Debug.Log($"{currentPlayer.name} already matches the current bet.");
            }

            hasMadeAction = true;
        }

        public void Raise(PlayerModel currentPlayer, int raiseAmount)
        {
            if (!CanRaise(currentPlayer, raiseAmount)) return;

            int additionalAmount = raiseAmount - pokerTable.GetPlayerBet(currentPlayer);

            currentPlayer.stackChipsManager.RemoveChips(additionalAmount);
            pokerTable.currentBet.TotalChips = raiseAmount;
            pokerTable.SetPlayerBet(currentPlayer, raiseAmount);
            pokerTable.AddToPot(additionalAmount);
            Debug.Log(
                $"{currentPlayer.name} has raised to {raiseAmount}. Remaining chips: {currentPlayer.stackChipsManager.TotalChips}");

            hasMadeAction = true;
        }

        public void Check(PlayerModel currentPlayer)
        {
            if (!CanCheck(currentPlayer)) return;

            Debug.Log($"{currentPlayer.name} has checked.");
            hasMadeAction = true;
        }

        public void AllIn(PlayerModel currentPlayer)
        {
            if (!CanAllIn(currentPlayer)) return;

            int currentBalance = currentPlayer.stackChipsManager.TotalChips;
            currentPlayer.stackChipsManager.RemoveChips(currentBalance);
            pokerTable.SetPlayerBet(currentPlayer, currentBalance);
            pokerTable.AddToPot(currentBalance);
            Debug.Log($"{currentPlayer.name} is all in with {currentBalance} chips.");

            hasMadeAction = true;
        }

        private bool CanFold(PlayerModel currentPlayer)
        {
            if (!IsPlayerTurn(currentPlayer)) return false;

            return true;
        }

        private bool CanCall(PlayerModel currentPlayer)
        {
            if (!IsPlayerTurn(currentPlayer)) return false;
            int callAmount = pokerTable.currentBet.TotalChips - pokerTable.GetPlayerBet(currentPlayer);
            if (callAmount <= 0)
            {
                Debug.Log($"{currentPlayer.name} cannot call because they already match the current bet.");
                return false;
            }

            if (currentPlayer.stackChipsManager.TotalChips < callAmount)
            {
                Debug.Log($"{currentPlayer.name} doesn't have enough chips to call.");
                return false;
            }

            return true;
        }

        private bool CanRaise(PlayerModel currentPlayer, int raiseAmount)
        {
            if (!IsPlayerTurn(currentPlayer)) return false;
            if (raiseAmount <= pokerTable.currentBet.TotalChips)
            {
                Debug.Log($"{currentPlayer.name}'s raise amount must be greater than the current bet.");
                return false;
            }

            int additionalAmount = raiseAmount - pokerTable.GetPlayerBet(currentPlayer);
            if (currentPlayer.stackChipsManager.TotalChips < additionalAmount)
            {
                Debug.Log($"{currentPlayer.name} doesn't have enough chips to raise.");
                return false;
            }

            return true;
        }

        private bool CanCheck(PlayerModel currentPlayer)
        {
            if (!IsPlayerTurn(currentPlayer)) return false;
            if (pokerTable.GetPlayerBet(currentPlayer) != pokerTable.currentBet.TotalChips && currentPlayer.stackChipsManager.TotalChips != 0)
            {
                Debug.Log($"{currentPlayer.name} cannot check because they need to match the current bet.");
                return false;
            }

            return true;
        }

        private bool CanAllIn(PlayerModel currentPlayer)
        {
            if (!IsPlayerTurn(currentPlayer)) return false;
            if (currentPlayer.stackChipsManager.TotalChips <= 0)
            {
                Debug.Log($"{currentPlayer.name} cannot go all-in because they have no chips left.");
                return false;
            }

            return true;
        }
    }
}