using System;
using System.Collections;
using Code.GameEntities;
using UnityEngine;
using System.Collections.Generic;
using Code.GameEntities.AIPlayer;
using Code.GameEntities.Player;
using Code.Infrastructure.CoroutineRunner;

namespace Code
{
    public class Bootstrap : MonoBehaviour
    {
        [SerializeField] private Dealer dealer;
        [SerializeField] private List<PlayerModel> players;
        [SerializeField] private List<AIPlayer> AIPlayers;
        [SerializeField] private PokerTable pokerTable;
        [SerializeField] private PlayerInputManager playerInputManager;
        [SerializeField] private GameManager gameManager;
        //[SerializeField] private CoroutineRunner coroutineRunner;

        public void Start()
        {
            for (int i = 0; i < players.Count; i++)
            {
                players[i].Initialize();
            }
            
            for (int i = 0; i < AIPlayers.Count; i++)
            {
                AIPlayers[i].Initialize();
            }
            
            pokerTable.Initialize(players);
            dealer.Initialize(players);
            playerInputManager.Initialize();
            dealer.AssignRoles();
            dealer.DistributeStartingChips();

            StartCoroutine(StartGameCycle());
        }

        public IEnumerator StartGameCycle()
        {
            yield return StartCoroutine(gameManager.RunBettingRoundForBlinds());
            
            dealer.DealCardsToPlayers();
            yield return StartCoroutine(gameManager.RunFirstBettingRound());
            dealer.DealFlop();
            //dealer.DealTurn();
            //dealer.DealRiver();
        }
    }
}