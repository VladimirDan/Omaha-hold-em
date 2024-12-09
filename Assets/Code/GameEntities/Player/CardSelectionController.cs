using System;
using Code.View;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;

namespace Code.GameEntities.Player
{
    public class CardSelectionController : MonoBehaviour
    {
        private int maxHandCards = 2;
        private int maxTableCards = 3;

        public List<Image> selectedCardImages = new List<Image>();
        public List<Card> selectedCards = new List<Card>();

        [SerializeField] public CardsView cardsView;
        [SerializeField] public PokerTable pokerTable;
        [SerializeField] public PlayerHand hand;

        public void Initialize(PokerTable pokerTable, PlayerHand hand)
        {
            this.pokerTable = pokerTable;
            this.hand = hand;
        }

        public void Reset()
        {
            selectedCardImages.Clear();
            selectedCards.Clear();
        }

        public void HandleCardSelection(Image cardImage)
        {
            bool isCardFromHand = hand.cardsView.cardImages.Contains(cardImage);
            bool isCardFromTable = pokerTable.cardsView.cardImages.Contains(cardImage);

            if (selectedCardImages.Contains(cardImage))
            {
                SetSelectedState(false, cardImage);
                RemoveCard(cardImage);
            }
            else
            {
                if (isCardFromHand)
                {
                    if (CountHandCards() < maxHandCards)
                    {
                        AddCard(cardImage);
                        SetSelectedState(true, cardImage);
                    }
                    else
                    {
                        ReplaceFirstCardFromHand(cardImage);
                    }
                }
                else if (isCardFromTable)
                {
                    if (CountTableCards() < maxTableCards)
                    {
                        AddCard(cardImage);
                        SetSelectedState(true, cardImage);
                    }
                    else
                    {
                        ReplaceFirstCardFromTable(cardImage);
                    }
                }
            }
        }

        private void ReplaceFirstCardFromHand(Image newCard)
        {
            var firstHandCard = selectedCardImages.FirstOrDefault(c => hand.cardsView.cardImages.Contains(c));
            if (firstHandCard != null)
            {
                SetSelectedState(false, firstHandCard);
                RemoveCard(firstHandCard);
            }

            if (!selectedCardImages.Contains(newCard))
            {
                AddCard(newCard);
                SetSelectedState(true, newCard);
            }
        }

        private void ReplaceFirstCardFromTable(Image newCard)
        {
            var firstTableCard = selectedCardImages.FirstOrDefault(c => pokerTable.cardsView.cardImages.Contains(c));
            if (firstTableCard != null)
            {
                SetSelectedState(false, firstTableCard);
                RemoveCard(firstTableCard);
            }

            if (!selectedCardImages.Contains(newCard))
            {
                AddCard(newCard);
                SetSelectedState(true, newCard);
            }
        }

        private int CountHandCards()
        {
            return selectedCardImages.Count(c => hand.cardsView.cardImages.Contains(c));
        }

        private int CountTableCards()
        {
            return selectedCardImages.Count(c => pokerTable.cardsView.cardImages.Contains(c));
        }

        public void SetSelectedState(bool selected, Image card)
        {
            if (selected)
            {
                cardsView?.HighlightCardWithOnSelectedColor(card);
            }
            else
            {
                cardsView?.ResetCardColor(card);
            }
        }

        private void AddCard(Image cardImage)
        {
            if (!selectedCardImages.Contains(cardImage) && selectedCardImages.Count < maxHandCards + maxTableCards)
            {
                selectedCardImages.Add(cardImage);
                cardsView?.cardImages.Add(cardImage);

                var card = GetCardFromImage(cardImage);
                if (card != null && !selectedCards.Contains(card))
                {
                    selectedCards.Add(card);
                }
            }
        }

        private void RemoveCard(Image cardImage)
        {
            if (selectedCardImages.Contains(cardImage))
            {
                selectedCardImages.Remove(cardImage);
                cardsView?.cardImages.Remove(cardImage);

                Card card = GetCardFromImage(cardImage);
                if (card != null && selectedCards.Contains(card))
                {
                    selectedCards.Remove(card);
                }
            }
        }

        private Card GetCardFromImage(Image cardImage)
        {
            return hand.cardsView.cardSpriteData.GetCardFromSprite(cardImage.sprite);
        }
    }
}
