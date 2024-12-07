using UnityEngine;
using UnityEngine.UI;
using Code.GameEntities;
using Code.Enums;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine.Serialization;

namespace Code.View
{
    public class CardsView : MonoBehaviour
    {
        [SerializeField] private List<Image> cardImages = new List<Image>(5);
        [SerializeField] private CardSpriteData cardSpriteData;

        private Color onFoldColor = new Color(1f, 1f, 1f, 0.5f); 
        private Color onTurnColorColor = new Color(1f, 1f, 0.5f, 1f);
        private Color defaultColor = Color.white; 

        public void UpdateCardsView([CanBeNull] List<Card> cards, bool isSecret)
        {
            for (int i = 0; i < cards.Count; i++)
            {
                Sprite cardSprite;
                if (isSecret)
                {
                    cardSprite = cardSpriteData.GetCardBack();
                }
                else
                {
                    cardSprite = cardSpriteData.GetSprite(cards[i].Suit, cards[i].Rank);
                }

                if (cardImages[i] != null && cardSprite != null)
                {
                    cardImages[i].sprite = cardSprite;
                    cardImages[i].enabled = true;
                }
            }

            for (int i = cards.Count; i < cardImages.Count; i++)
            {
                if (cardImages[i] != null)
                {
                    cardImages[i].enabled = false;
                }
            }
        }
        
        public void HighlightAllCardsWithOnTurnColor()
        {
            for (int i = 0; i < cardImages.Count; i++)
            {
                if (cardImages[i] != null)
                {
                    SetCardColor(i, onTurnColorColor);
                }
            }
        }
        
        public void HighlightAllCardsWithOnFoldTransparency()
        {
            for (int i = 0; i < cardImages.Count; i++)
            {
                if (cardImages[i] != null)
                {
                    SetCardTransparency(i, onFoldColor.a);
                }
            }
        }
        
        public void SetCardColor(int cardIndex, Color color)
        {
            if (cardImages[cardIndex] != null)
            {
                Color currentColor = cardImages[cardIndex].color;
                cardImages[cardIndex].color = new Color(color.r, color.g, color.b, currentColor.a);
            }
        }

        public void SetCardTransparency(int cardIndex, float alpha)
        {
            if (cardImages[cardIndex] != null)
            {
                Color currentColor = cardImages[cardIndex].color;
                cardImages[cardIndex].color = new Color(currentColor.r, currentColor.g, currentColor.b, alpha);
            }
        }

        public void SetCardColorAndTransparency(int cardIndex, Color color, float alpha)
        {
            if (cardImages[cardIndex] != null)
            {
                cardImages[cardIndex].color = new Color(color.r, color.g, color.b, alpha);
            }
        }
        
        public void ResetAllCardColors()
        {
            for (int i = 0; i < cardImages.Count; i++)
            {
                if (cardImages[i] != null)
                {
                    ResetCardColor(i);
                }
            }
        }

        public void ResetAllCardTransparencies()
        {
            for (int i = 0; i < cardImages.Count; i++)
            {
                if (cardImages[i] != null)
                {
                    ResetCardTransparency(i);
                }
            }
        }

        public void ResetAllCardColorAndTransparency(Color color)
        {
            for (int i = 0; i < cardImages.Count; i++)
            {
                if (cardImages[i] != null)
                {
                    ResetCardColorAndTransparency(i, color);
                }
            }
        }
        
        public void ResetCardColor(int cardIndex)
        {
            if (cardImages[cardIndex] != null)
            {
                Color currentColor = cardImages[cardIndex].color;
                cardImages[cardIndex].color = new Color(defaultColor.r, defaultColor.g, defaultColor.b, currentColor.a);
            }
        }

        public void ResetCardTransparency(int cardIndex)
        {
            if (cardImages[cardIndex] != null)
            {
                Color currentColor = cardImages[cardIndex].color;
                cardImages[cardIndex].color = new Color(currentColor.r, currentColor.g, currentColor.b, defaultColor.a);
            }
        }

        public void ResetCardColorAndTransparency(int cardIndex, Color color)
        {
            if (cardImages[cardIndex] != null)
            {
                cardImages[cardIndex].color = new Color(color.r, color.g, color.b, color.a);
            }
        }
    }
}
