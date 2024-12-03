using UnityEngine;
using UnityEngine.UI;
using Code.GameEntities;
using Code.Enums;
using System.Collections.Generic;
using UnityEngine.Serialization;

namespace Code.View
{
    public class CardsView : MonoBehaviour
    {
        [SerializeField] private List<Image> cardImages = new List<Image>(5);
        [SerializeField] private CardSpriteData cardSpriteData;

        public void UpdateCardsView(List<Card> cards)
        {
            for (int i = 0; i < cards.Count; i++)
            {
                Sprite cardSprite = cardSpriteData.GetSprite(cards[i].Suit, cards[i].Rank);

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
    }
}