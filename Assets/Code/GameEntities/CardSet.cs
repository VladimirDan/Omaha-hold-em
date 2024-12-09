using System.Collections.Generic;
using UnityEngine;
using Code.Enums;

namespace Code.GameEntities
{
    public class CardSet
    {
        private List<Card> cards;
        public int maxCardsCount;

        public List<Card> Cards => cards;

        public CardSet(int maxCardsCount)
        {
            this.maxCardsCount = maxCardsCount;
            cards = new List<Card>(maxCardsCount);
        }
        
        public CardSet(List<Card> cards, int maxCardsCount)
        {
            this.cards = cards;
            this.maxCardsCount = maxCardsCount;
        }
        
        public void AddCard(Card card)
        {
            if (cards.Count >= maxCardsCount)
            {
                Debug.LogError("Cannot add more than maxCardsCount cards to the player's hand in Omaha!");
                return;
            }

            if (card == null)
            {
                Debug.LogError("Attempted to add a null card to the hand!");
                return;
            }

            Cards.Add(card);
        }

        public void Reset()
        {
            Cards.Clear();
        }
    }
}