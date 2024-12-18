using System.Collections.Generic;
using Code.GameEntities.Player;
using Code.Infrastructure.CoroutineRunner;
using UnityEngine;
using Code.Enums;
using System.Linq;
using System.Collections;
using Code.GameRules;
using System;

namespace Code.GameEntities
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] public PokerTable pokerTable;
        public int smallBlind = 1;
        public int bigBlind = 2;
        public int maxCountOfBetCircles = 4;
        public int maxCountOfBetCirclesToRaise = 3;
        public CombinationComparer combinationComparer = new CombinationComparer(new CombinationEvaluator());
        
        

        public IEnumerator RunShowdownRound(int firstPlayerToBetIndex)
        {
            for (int i = 0; i < pokerTable.playersInGame.Count; i++)
            {
                int currentIndex = (firstPlayerToBetIndex + i) % pokerTable.playersInGame.Count;
                yield return StartCoroutine(WaitForPlayerCombinationReveal(pokerTable.playersInGame[currentIndex]));
            }
        }
        
        public IEnumerator RunBettingRound(int firstPlayerToBetIndex)
        {
            int countOfBetCircles = 0;

            do
            {
                BetType betType = BetType.RegularBet;
                if (countOfBetCircles == maxCountOfBetCirclesToRaise) betType = BetType.NonRaiseBet;

                int countOfActivePlayersBeforeAction;
                for (int i = 0; i < pokerTable.playersInGame.Count; i++)
                {
                    countOfActivePlayersBeforeAction = pokerTable.playersInGame.Count;
                    if(countOfActivePlayersBeforeAction == 1) yield break;
                    
                    int currentIndex = (firstPlayerToBetIndex + i) % pokerTable.playersInGame.Count;
                    //Debug.Log(((Object)pokerTable.playersInGame[currentIndex]).name);
                    yield return StartCoroutine(WaitForPlayerAction(pokerTable.playersInGame[currentIndex],
                        betType));
                    if (countOfActivePlayersBeforeAction > pokerTable.playersInGame.Count)
                    {
                        i--;
                        firstPlayerToBetIndex %= pokerTable.playersInGame.Count;
                    }
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
            // int bigBlindPlayerIndexInAllPlayers =
            //     pokerTable.players.FindIndex(player => player.playerRoleManager.Role == PlayerRole.BigBlind);
            //
            // if (bigBlindPlayerIndexInAllPlayers == -1)
            //     throw new InvalidOperationException("BigBlind player not found in the table.");
            //
            // return (bigBlindPlayerIndexInAllPlayers + 1) % pokerTable.playersInGame.Count;
            int firstPlayerIndex = -1;

            int bigBlindPlayerIndex =
                pokerTable.playersInGame.FindIndex(player => player.playerRoleManager.Role == PlayerRole.BigBlind);

            if (bigBlindPlayerIndex != -1)
            {
                firstPlayerIndex = (bigBlindPlayerIndex + 1) % pokerTable.playersInGame.Count;
            }
            else
            {
                int bigBlindPlayerIndexInAllPlayers =
                    pokerTable.players.FindIndex(player => player.playerRoleManager.Role == PlayerRole.BigBlind);
                for (int i = bigBlindPlayerIndexInAllPlayers + 1; i < (pokerTable.players.Count + bigBlindPlayerIndexInAllPlayers); i++)
                {
                    int index = i % pokerTable.players.Count;
                    if (pokerTable.playersInGame.Contains(pokerTable.players[index]))
                    {
                        firstPlayerIndex = (pokerTable.playersInGame.IndexOf(pokerTable.players[index]) + 1) % pokerTable.playersInGame.Count;
                    }
                }
            }

            return firstPlayerIndex;
        }

        public int FindFirstPlayerIndexToAct()
        {
            int firstPlayerIndex = -1;

            int smallBlindPlayerIndex =
                pokerTable.playersInGame.FindIndex(player => player.playerRoleManager.Role == PlayerRole.SmallBlind);

            int bigBlindPlayerIndex =
                pokerTable.playersInGame.FindIndex(player => player.playerRoleManager.Role == PlayerRole.BigBlind);

            if (smallBlindPlayerIndex != -1)
            {
                firstPlayerIndex = smallBlindPlayerIndex;
            }
            else if (bigBlindPlayerIndex != -1)
            {
                firstPlayerIndex = bigBlindPlayerIndex;
            }
            else
            {
                int bigBlindPlayerIndexInAllPlayers =
                    pokerTable.players.FindIndex(player => player.playerRoleManager.Role == PlayerRole.BigBlind);
                for (int i = bigBlindPlayerIndexInAllPlayers + 1; i < (pokerTable.players.Count + bigBlindPlayerIndexInAllPlayers); i++)
                {
                    int index = i % pokerTable.players.Count;
                    if (pokerTable.playersInGame.Contains(pokerTable.players[index]))
                    {
                        return pokerTable.playersInGame.IndexOf(pokerTable.players[index]);
                    }
                }
            }

            return firstPlayerIndex;
        }

        public IEnumerator RunBettingRoundForBlinds()
        {
            var firstPlayer =
                pokerTable.playersInGame.FirstOrDefault(
                    player => player.playerRoleManager.Role == PlayerRole.SmallBlind);

            if (firstPlayer == null)
            {
                Debug.LogError("Нет игрока с ролью SmallBlind!");
                yield break;
            }

            Debug.Log($"{firstPlayer.name}, ваш ход! Минимальная ставка: {smallBlind} чипов.");
            pokerTable.currentBet.TotalChips = smallBlind;
            yield return StartCoroutine(WaitForPlayerAction(firstPlayer, BetType.SmallBlindBet));

            var secondPlayer =
                pokerTable.playersInGame.FirstOrDefault(player => player.playerRoleManager.Role == PlayerRole.BigBlind);

            if (secondPlayer == null)
            {
                Debug.LogError("Нет игрока с ролью BigBlind!");
                yield break;
            }

            Debug.Log($"{secondPlayer.name}, ваш ход! Минимальная ставка: {bigBlind} фишки.");
            pokerTable.currentBet.TotalChips = Mathf.Max(pokerTable.currentBet.TotalChips, bigBlind);
            yield return StartCoroutine(WaitForPlayerAction(secondPlayer, BetType.BigBlindBet));
        }

        public IEnumerator WaitForPlayerAction(PlayerModel currentPlayer, BetType betType)
        {
            Debug.Log($"{currentPlayer.name}'s turn. Waiting for action...");
            currentPlayer.playerActionsManager.hasMadeAction = false;
            currentPlayer.hand.cardsView.HighlightAllCardsWithOnTurnColor();
            currentPlayer.playerActionsManager.StartPlayerBetTurn(betType);

            while (!currentPlayer.playerActionsManager.hasMadeAction)
            {
                yield return null;
            }

            currentPlayer.hand.cardsView.ResetAllCardColors();
            currentPlayer.playerActionsManager.EndPlayerBetTurn();
            Debug.Log($"{currentPlayer.name} has completed the action.");
        }
        
        public IEnumerator WaitForPlayerCombinationReveal(PlayerModel currentPlayer)
        {
            Debug.Log($"{currentPlayer.name}'s turn. Waiting for CombinationReveal...");
            currentPlayer.playerActionsManager.hasMadeAction = false;
            currentPlayer.playerActionsManager.StartPlayerCombinationRevealTurn();
            
            while (!currentPlayer.playerActionsManager.hasMadeAction)
            {
                yield return null;
            }
            
            currentPlayer.playerActionsManager.EndPlayerCombinationRevealTurn();
            Debug.Log($"{currentPlayer.name} has completed the action.");
        }

        public bool CheckOnlyOnePlayerLeft()
        {
            return pokerTable.playersInGame.Count == 1;
        }

        public void HandleLastPlayerWin()
        {
            AwardPotToLastPlayer();
            pokerTable.Reset();
        }
        
        public void AwardPotToLastPlayer()
        {
            if (pokerTable.playersInGame.Count == 1)
            {
                PlayerModel winner = pokerTable.playersInGame[0];
                winner.stackChipsManager.AddChips(pokerTable.pot);
                pokerTable.pot = 0;

                foreach (var player in pokerTable.players)
                {
                    player.betChipsManager.Reset();
                }
            }
        }
        
        public void RemovePlayersWithoutChips()
        {
            pokerTable.players.Where(player => player.stackChipsManager.TotalChips <= 0)
                .ToList().ForEach(player => player.Reset());
            
            pokerTable.players.RemoveAll(player => player.stackChipsManager.TotalChips <= 0);
        }
    }
}