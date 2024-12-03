using System.Collections.Generic;
using UnityEngine;
using Code.Enums;

namespace Code.GameEntities
{
    public class Dealer : MonoBehaviour
    {
        [SerializeField] private Deck deck; 
        [SerializeField] private List<PlayerHand> playerHands;
        [SerializeField] private PokerTable pokerTable;

        public void Initialize()
        {
            deck = new Deck();
            deck.Shuffle();
        }
        
        public void DealCardsToPlayers()
        {
            if (playerHands.Count != 5)
            {
                Debug.LogError("The dealer needs 5 players!");
                return;
            }
            
            deck.Shuffle();
            
            for (int i = 0; i < playerHands.Count; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    playerHands[i].AddCard(deck.DrawCard());
                }
                //Debug.Log(playerHands[i].ToString());
            }
        }
        
        public void DealFlop()
        {
            List<Card> cardsList = pokerTable.cardSet.Cards;
            if (cardsList.Count > 0)
            {
                Debug.LogError("Flop has already been dealt.");
                return;
            }

            for (int i = 0; i < 3; i++)
            {
                pokerTable.AddCard(deck.DrawCard());
            }
        }

        public void DealTurn()
        {
            List<Card> cardsList = pokerTable.cardSet.Cards;
            if (cardsList.Count < 3)
            {
                Debug.LogError("Cannot deal the Turn before the Flop.");
                return;
            }

            pokerTable.AddCard(deck.DrawCard());
        }

        public void DealRiver()
        {
            List<Card> cardsList = pokerTable.cardSet.Cards;
            if (cardsList.Count < 4)
            {
                Debug.LogError("Cannot deal the River before the Turn.");
                return;
            }

            pokerTable.AddCard(deck.DrawCard());
        }
    }
}