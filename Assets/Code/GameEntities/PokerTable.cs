using System.Collections.Generic;
using UnityEngine;
using Code.Enums;
using Code.View;

namespace Code.GameEntities
{
    public class PokerTable : MonoBehaviour
    {
        [SerializeField] public CardSet cardSet;
        private int communityCardsCount = 5;
        
        [SerializeField] public CardsView cardsView;

        public void Initialize()
        {
            cardSet = new CardSet(communityCardsCount);
        }
        
        public void AddCard(Card card)
        {
            cardSet.AddCard(card);
            cardsView.UpdateCardsView(cardSet.Cards);
        }
    }
}