using System.Collections.Generic;
using UnityEngine;
using System;
using Code.Enums;
using System.Linq;

namespace Code.GameEntities.Player
{
    public class PlayerActionsManager : MonoBehaviour
    {
        [SerializeField] public PokerTable pokerTable;
        [SerializeField] public CardSelectionController cardSelectionController;
        public bool hasMadeAction = false;
        private bool isPlayerBetTurn = false;
        private bool isPlayerCombinationRevealTurn = false;
        private bool isRaiseRound = false;
        public event Action<BetType> OnPlayerBetTurn;
        public event Action OnCombinationRevealTurn;
        
        public void Initialize(PlayerHand hand)
        {
            cardSelectionController.Initialize(pokerTable, hand);
        }

        public void Reset()
        {
            cardSelectionController.Reset();
        }

        public void StartPlayerBetTurn(BetType betType)
        {
            isPlayerBetTurn = true;
            if (betType == BetType.NonRaiseBet)
            {
                isRaiseRound = false;
            }
            else
            {
                isRaiseRound = true;
            }
            OnPlayerBetTurn?.Invoke(betType);
        }
        
        public void EndPlayerBetTurn()
        {
            isPlayerBetTurn = false;
            isRaiseRound = false;
        }

        public bool IsPlayerBetTurn(PlayerModel currentPlayer)
        {
            if (!isPlayerBetTurn)
            {
                Debug.LogError($"{currentPlayer.name} cannot act because it's not their turn.");
                return false;
            }

            return true;
        }
        
        public bool IsRaiseRound(PlayerModel currentPlayer)
        {
            if (!isRaiseRound)
            {
                Debug.LogError($"{currentPlayer.name} cannot raise because it's not raise turn.");
                return false;
            }

            return true;
        }
        
        public void StartPlayerCombinationRevealTurn()
        {
            isPlayerCombinationRevealTurn = true;
            
            OnCombinationRevealTurn?.Invoke();
        }
        
        public void EndPlayerCombinationRevealTurn()
        {
            isPlayerCombinationRevealTurn = false;
        }
        
        public void Fold(PlayerModel currentPlayer)
        {
            Debug.Log($"{currentPlayer.name} has folded.");
            pokerTable.RemovePlayerFromActivePlayers(currentPlayer);
            currentPlayer.hand.cardsView.HighlightAllCardsWithOnFoldTransparency();
            hasMadeAction = true;
        }

        public void Call(PlayerModel currentPlayer)
        {
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
            Debug.Log($"{currentPlayer.name} has checked.");
            hasMadeAction = true;
        }

        public void AllIn(PlayerModel currentPlayer)
        {
            int currentBalance = currentPlayer.stackChipsManager.TotalChips;
            currentPlayer.stackChipsManager.RemoveChips(currentBalance);
            pokerTable.SetPlayerBet(currentPlayer, currentBalance);
            pokerTable.AddToPot(currentBalance);
            Debug.Log($"{currentPlayer.name} is all in with {currentBalance} chips.");

            hasMadeAction = true;
        }

        public bool CanFold(PlayerModel currentPlayer)
        {
            if (!IsPlayerBetTurn(currentPlayer)) return false;
            
            int activePlayersCount = pokerTable.playersInGame.Count;
            if (activePlayersCount <= 1) 
            {
                Debug.Log("Cannot fold - you would be the last active player.");
                return false;
            }
            
            return true;
        }

        public bool CanCall(PlayerModel currentPlayer)
        {
            if (!IsPlayerBetTurn(currentPlayer)) return false;
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

        public bool CanRaise(PlayerModel currentPlayer, int raiseAmount)
        {
            if (!IsPlayerBetTurn(currentPlayer) || !IsRaiseRound(currentPlayer)) return false;
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

        public bool CanCheck(PlayerModel currentPlayer)
        {
            if (!IsPlayerBetTurn(currentPlayer)) return false;
            if (pokerTable.GetPlayerBet(currentPlayer) != pokerTable.currentBet.TotalChips && currentPlayer.stackChipsManager.TotalChips != 0)
            {
                Debug.Log($"{currentPlayer.name} cannot check because they need to match the current bet.");
                return false;
            }

            return true;
        }

        public bool CanAllIn(PlayerModel currentPlayer)
        {
            if (!IsPlayerBetTurn(currentPlayer)) return false;
            if (currentPlayer.stackChipsManager.TotalChips <= 0)
            {
                Debug.Log($"{currentPlayer.name} cannot go all-in because they have no chips left.");
                return false;
            }

            return true;
        }
        
        public bool CanSelectCards()
        {
            if (!isPlayerCombinationRevealTurn) return false;

            return true;
        }

        public void SubmitCombination(PlayerModel currentPlayer)
        {
            List<Card> combinationCards = cardSelectionController.selectedCards
                .Select(item => new Card(item))
                .OrderByDescending(card => (int)card.rank) 
                .ToList();
            
            pokerTable.playersCombinations[currentPlayer] = combinationCards;
            
            currentPlayer.hand.cardsView.ResetCardsColorAndTransparency();
            pokerTable.cardsView.ResetCardsColorAndTransparency();
            
            currentPlayer.hand.cardSet = new CardSet(combinationCards, currentPlayer.hand.cardSet.maxCardsCount);
            currentPlayer.hand.cardsView.UpdateCardsView(currentPlayer.hand.cardSet.Cards, false);
            currentPlayer.hand.cardsView.HighlightCardsFromTable(pokerTable.communityCards.Cards, combinationCards);
            
            hasMadeAction = true;
        }

        public bool CanSubmitCombination()
        {
            if (!isPlayerCombinationRevealTurn) return false;
            if(cardSelectionController.selectedCards.Count != 5) return false;
            foreach (Card card in cardSelectionController.selectedCards)
            {
                if(card == null) return false;
            }

            return true;
        }
    }
}