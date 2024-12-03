using System.Collections.Generic;
using UnityEngine;
using Code.Enums;
using Code.View;

namespace Code.GameEntities
{
    public class PlayerHand : MonoBehaviour
    {
        [SerializeField] public CardSet cardSet;
        private int playerHandCardsCount = 4;

        [SerializeField] public CardsView cardsView;
        
        public void Initialize()
        {
            cardSet = new CardSet(playerHandCardsCount);
        }

        public void AddCard(Card card)
        {
            cardSet.AddCard(card);
            cardsView.UpdateCardsView(cardSet.Cards);
        }
        
        public List<(Card, Card)> GetHandCombinations()
        {
            var combinations = new List<(Card, Card)>();

            if (cardSet.Cards.Count < 4)
            {
                Debug.LogError("Incomplete hand! A full Omaha hand requires 4 cards.");
                return combinations;
            }

            for (int i = 0; i < cardSet.Cards.Count; i++)
            {
                for (int j = i + 1; j < cardSet.Cards.Count; j++)
                {
                    combinations.Add((cardSet.Cards[i], cardSet.Cards[j]));
                }
            }

            return combinations;
        }

        public override string ToString()
        {
            return cardSet.Cards.Count == 0 ? "The hand is empty." : string.Join(", ", cardSet.Cards);
        }
    }
}