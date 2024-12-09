using System;
using System.Collections;
using Code.GameEntities;
using UnityEngine;
using System.Collections.Generic;
using Code.GameEntities.AIPlayer;
using Code.GameEntities.Player;
using Code.Infrastructure.CoroutineRunner;

namespace Code.GameEntities
{
    public class GameLoopManager : MonoBehaviour
    {
        private Dealer dealer;
        private GameManager gameManager;
        public bool isPokerRoundOver = false;
        public bool isPotDistributed = false;

        public void Initialize(Dealer dealer, GameManager gameManager)
        {
            this.dealer = dealer;
            this.gameManager = gameManager;
        }

        public IEnumerator StartGameCycle()
        {
            dealer.DistributeStartingChips();
            while (!CheckOnlyOnePlayerLeft())
            {
                isPokerRoundOver = false;
                isPotDistributed = false;
                dealer.AssignRoles();

                yield return StartCoroutine(gameManager.RunBettingRoundForBlinds());
                if (gameManager.CheckOnlyOnePlayerLeft())
                {
                    gameManager.HandleLastPlayerWin();
                    continue;
                }

                dealer.DealCardsToPlayers();

                int firstPlayerToBetAfterBigBlindIndex = gameManager.FindFirstPlayerIndexToActAfterCardDealing();
                yield return StartCoroutine(gameManager.RunBettingRound(firstPlayerToBetAfterBigBlindIndex));
                if (gameManager.CheckOnlyOnePlayerLeft())
                {
                    gameManager.HandleLastPlayerWin();
                    continue;
                }

                dealer.DealFlop();
                
                yield return StartCoroutine(gameManager.RunBettingRound(gameManager.FindFirstPlayerIndexToAct()));
                if (gameManager.CheckOnlyOnePlayerLeft()) break;
                
                dealer.DealTurn();
                
                yield return StartCoroutine(gameManager.RunBettingRound(gameManager.FindFirstPlayerIndexToAct()));
                if (gameManager.CheckOnlyOnePlayerLeft())
                {
                    gameManager.HandleLastPlayerWin();
                    continue;
                }

                dealer.DealRiver();
                
                yield return StartCoroutine(gameManager.RunBettingRound(gameManager.FindFirstPlayerIndexToAct()));
                if (gameManager.CheckOnlyOnePlayerLeft())
                {
                    gameManager.HandleLastPlayerWin();
                    continue;
                }
                
                yield return StartCoroutine(gameManager.RunShowdownRound(gameManager.FindFirstPlayerIndexToAct()));
                
                dealer.DistributePotChips();
                isPotDistributed = true;

                while (!isPokerRoundOver)
                {
                    yield return null;
                }
                
                gameManager.pokerTable.Reset();
                dealer.Reset();
                gameManager.RemovePlayersWithoutChips();
            }
        }

        public bool CheckOnlyOnePlayerLeft()
        {
            return gameManager.pokerTable.players.Count == 1;
        }
    }
}