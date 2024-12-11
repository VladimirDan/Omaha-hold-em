using System;
using System.Collections;
using Code.GameEntities;
using UnityEngine;
using System.Collections.Generic;
using Code.GameEntities.AIPlayer;
using Code.GameEntities.Player;
using Code.Infrastructure.CoroutineRunner;
using UnityEngine.Serialization;
using Code.GameRules;
using Code.InputManager_;

namespace Code
{
    public class Bootstrap : MonoBehaviour
    {
        [SerializeField] private Dealer dealer;
        [SerializeField] private List<PlayerModel> players;
        [SerializeField] private List<AIPlayer> AIPlayers;
        [SerializeField] private PokerTable pokerTable;
        [SerializeField] private InputManager inputManager;
        [SerializeField] private GameManager gameManager;
        [SerializeField] private GameLoopManager gameLoopManager;
        
        private CombinationComparer combinationComparer = new CombinationComparer(new CombinationEvaluator());

        public void Start()
        {
            for (int i = 0; i < players.Count; i++)
            {
                players[i].Initialize();
            }
            for (int i = 0; i < AIPlayers.Count; i++)
            {
                AIPlayers[i].Initialize(pokerTable);
            }
            pokerTable.Initialize(players);
            
            dealer.Initialize(players, combinationComparer);
            inputManager.Initialize();
            gameLoopManager.Initialize(dealer, gameManager);

            StartCoroutine(gameLoopManager.StartGameCycle());
        }
    }
}