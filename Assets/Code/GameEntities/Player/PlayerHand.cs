using System;
using System.Collections.Generic;
using UnityEngine;
using Code.Enums;
using Code.View;

namespace Code.GameEntities.Player
{
    public class PlayerHand : MonoBehaviour
    {
        [SerializeField] public CardSet cardSet;
        private int playerHandCardsCount = 5;
        [SerializeField] private bool isSecret;

        [SerializeField] public CardsView cardsView;
        
        public void Initialize()
        {
            cardSet = new CardSet(playerHandCardsCount);
            cardsView.UpdateCardsView(cardSet.Cards, false);
        }

        public void Reset()
        {
            cardSet.Reset();
            cardsView.UpdateCardsView(cardSet.Cards, false);
            cardsView.ResetCardsColorAndTransparency();
        }

        public void AddCard(Card card)
        {
            cardSet.AddCard(card);
            cardsView.UpdateCardsView(cardSet.Cards, isSecret);
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