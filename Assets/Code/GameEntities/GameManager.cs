using System.Collections.Generic;
using Code.GameEntities.Player;
using Code.Infrastructure.CoroutineRunner;
using UnityEngine;
using Code.Enums;
using System.Linq;
using System.Collections;

namespace Code.GameEntities
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private PokerTable pokerTable;
        public int smallBlind = 1;
        public int bigBlind = 2;
        public int maxCountOfBetCircles = 3;

        public IEnumerator RunFirstBettingRound()
        {
            int firstPlayerToBetIndex = FindFirstPlayerIndexToActAfterCardDealing();
            for (int i = firstPlayerToBetIndex; i < pokerTable.playersInGame.Count; i++)
            {
                Debug.Log(((Object)pokerTable.playersInGame[i]).name);
                yield return StartCoroutine(WaitForPlayerAction(pokerTable.playersInGame[i], GameStage.RegularAction));
            }

            if (AreAllPlayersBetsEqual())
            {
                yield break;
            }

            int countOfBetCircles = 1;
            
            do
            {
                for (int i = 0; i < pokerTable.playersInGame.Count; i++)
                {
                    Debug.Log(((Object)pokerTable.playersInGame[i]).name);
                    yield return StartCoroutine(WaitForPlayerAction(pokerTable.playersInGame[i], GameStage.RegularAction));
                }

                countOfBetCircles++;
            } while (countOfBetCircles != maxCountOfBetCircles && !AreAllPlayersBetsEqual());
        }
        
        private bool AreAllPlayersBetsEqual()
        {
            foreach (var player in pokerTable.playersInGame)
            {
                int playerBet = pokerTable.GetPlayerBet(player);

                if (playerBet != pokerTable.currentBet.TotalChips && player.stackChipsManager.TotalChips != 0)
                {
                    return false;
                }
            }
            return true;
        }
        
        public int FindFirstPlayerIndexToActAfterCardDealing()
        {
            int firstPlayerIndex;
            int bigBlindPlayerIndex = pokerTable.playersInGame.FindIndex(player => player.playerRoleManager.role == PlayerRole.BigBlind);
            int smallBlindPlayerIndex = pokerTable.playersInGame.FindIndex(player => player.playerRoleManager.role == PlayerRole.SmallBlind);
            
            if (bigBlindPlayerIndex != -1)
            {
                firstPlayerIndex = (bigBlindPlayerIndex + 1) % pokerTable.playersInGame.Count;
            }
            else if(smallBlindPlayerIndex != -1)
            {
                firstPlayerIndex = (smallBlindPlayerIndex + 1) % pokerTable.playersInGame.Count;
            }
            else
            {
                firstPlayerIndex = 0;
            }

            return firstPlayerIndex;
        }

        public IEnumerator RunBettingRoundForBlinds()
        {
            var firstPlayer = pokerTable.playersInGame.FirstOrDefault(player => player.playerRoleManager.role == PlayerRole.SmallBlind);

            if (firstPlayer == null)
            {
                Debug.LogError("Нет игрока с ролью SmallBlind!");
                yield break;
            }

            Debug.Log($"{firstPlayer.name}, ваш ход! Минимальная ставка: {smallBlind} чипов.");
            pokerTable.currentBet.TotalChips = smallBlind;
            yield return StartCoroutine(WaitForPlayerAction(firstPlayer, GameStage.SmallBlindBet));
            
            var secondPlayer = pokerTable.playersInGame.FirstOrDefault(player => player.playerRoleManager.role == PlayerRole.BigBlind);

            if (secondPlayer == null)
            {
                Debug.LogError("Нет игрока с ролью BigBlind!");
                yield break;
            }

            Debug.Log($"{secondPlayer.name}, ваш ход! Минимальная ставка: {bigBlind} чипов.");
            pokerTable.currentBet.TotalChips = Mathf.Max(pokerTable.currentBet.TotalChips, bigBlind);
            yield return StartCoroutine(WaitForPlayerAction(secondPlayer, GameStage.BigBlindBet));
        }
        
        public IEnumerator WaitForPlayerAction(PlayerModel currentPlayer, GameStage gameStage)
        {
            Debug.Log($"{currentPlayer.name}'s turn. Waiting for action...");
            currentPlayer.playerActionsManager.hasMadeAction = false;
            currentPlayer.hand.cardsView.HighlightAllCardsWithOnTurnColor();
            currentPlayer.playerActionsManager.TogglePlayerTurn(gameStage);
            
            while (!currentPlayer.playerActionsManager.hasMadeAction)
            {
                yield return null;
            }
            
            currentPlayer.hand.cardsView.ResetAllCardColors();
            currentPlayer.playerActionsManager.TogglePlayerTurn();
            Debug.Log($"{currentPlayer.name} has completed the action.");
        }
    }
}