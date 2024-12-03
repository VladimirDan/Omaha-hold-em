using System;
using Code.GameEntities;
using UnityEngine;

namespace Code
{
    public class Bootstrap : MonoBehaviour
    {
        [SerializeField] private Dealer dealer;
        [SerializeField] private PlayerHand[] playerHands;
        [SerializeField] private PokerTable pokerTable;

        public void Start()
        {
            foreach (PlayerHand hand in playerHands)
            {
                hand.Initialize();
            }
            pokerTable.Initialize();
            
            dealer.Initialize();
            dealer.DealCardsToPlayers();
            dealer.DealFlop();
            dealer.DealTurn();
            dealer.DealRiver();
        }
    }
}