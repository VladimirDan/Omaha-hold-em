using UnityEngine;
using Code.Enums;

namespace Code.View
{
    [CreateAssetMenu(fileName = "CardSprites", menuName = "CardSprites/Deck", order = 1)]
    public class CardSpriteData : ScriptableObject
    {
        [Header("Card Sprites")]
        [SerializeField] private Sprite aceOfClubs;
        [SerializeField] private Sprite twoOfClubs;
        [SerializeField] private Sprite threeOfClubs;
        [SerializeField] private Sprite fourOfClubs;
        [SerializeField] private Sprite fiveOfClubs;
        [SerializeField] private Sprite sixOfClubs;
        [SerializeField] private Sprite sevenOfClubs;
        [SerializeField] private Sprite eightOfClubs;
        [SerializeField] private Sprite nineOfClubs;
        [SerializeField] private Sprite tenOfClubs;
        [SerializeField] private Sprite jackOfClubs;
        [SerializeField] private Sprite queenOfClubs;
        [SerializeField] private Sprite kingOfClubs;

        [SerializeField] private Sprite aceOfDiamonds;
        [SerializeField] private Sprite twoOfDiamonds;
        [SerializeField] private Sprite threeOfDiamonds;
        [SerializeField] private Sprite fourOfDiamonds;
        [SerializeField] private Sprite fiveOfDiamonds;
        [SerializeField] private Sprite sixOfDiamonds;
        [SerializeField] private Sprite sevenOfDiamonds;
        [SerializeField] private Sprite eightOfDiamonds;
        [SerializeField] private Sprite nineOfDiamonds;
        [SerializeField] private Sprite tenOfDiamonds;
        [SerializeField] private Sprite jackOfDiamonds;
        [SerializeField] private Sprite queenOfDiamonds;
        [SerializeField] private Sprite kingOfDiamonds;

        [SerializeField] private Sprite aceOfHearts;
        [SerializeField] private Sprite twoOfHearts;
        [SerializeField] private Sprite threeOfHearts;
        [SerializeField] private Sprite fourOfHearts;
        [SerializeField] private Sprite fiveOfHearts;
        [SerializeField] private Sprite sixOfHearts;
        [SerializeField] private Sprite sevenOfHearts;
        [SerializeField] private Sprite eightOfHearts;
        [SerializeField] private Sprite nineOfHearts;
        [SerializeField] private Sprite tenOfHearts;
        [SerializeField] private Sprite jackOfHearts;
        [SerializeField] private Sprite queenOfHearts;
        [SerializeField] private Sprite kingOfHearts;

        [SerializeField] private Sprite aceOfSpades;
        [SerializeField] private Sprite twoOfSpades;
        [SerializeField] private Sprite threeOfSpades;
        [SerializeField] private Sprite fourOfSpades;
        [SerializeField] private Sprite fiveOfSpades;
        [SerializeField] private Sprite sixOfSpades;
        [SerializeField] private Sprite sevenOfSpades;
        [SerializeField] private Sprite eightOfSpades;
        [SerializeField] private Sprite nineOfSpades;
        [SerializeField] private Sprite tenOfSpades;
        [SerializeField] private Sprite jackOfSpades;
        [SerializeField] private Sprite queenOfSpades;
        [SerializeField] private Sprite kingOfSpades;

        public Sprite GetSprite(CardSuit suit, CardRank rank)
        {
            switch (suit)
            {
                case CardSuit.Clubs:
                    return rank switch
                    {
                        CardRank.Ace => aceOfClubs,
                        CardRank.Two => twoOfClubs,
                        CardRank.Three => threeOfClubs,
                        CardRank.Four => fourOfClubs,
                        CardRank.Five => fiveOfClubs,
                        CardRank.Six => sixOfClubs,
                        CardRank.Seven => sevenOfClubs,
                        CardRank.Eight => eightOfClubs,
                        CardRank.Nine => nineOfClubs,
                        CardRank.Ten => tenOfClubs,
                        CardRank.Jack => jackOfClubs,
                        CardRank.Queen => queenOfClubs,
                        CardRank.King => kingOfClubs,
                        _ => null
                    };
                case CardSuit.Diamonds:
                    return rank switch
                    {
                        CardRank.Ace => aceOfDiamonds,
                        CardRank.Two => twoOfDiamonds,
                        CardRank.Three => threeOfDiamonds,
                        CardRank.Four => fourOfDiamonds,
                        CardRank.Five => fiveOfDiamonds,
                        CardRank.Six => sixOfDiamonds,
                        CardRank.Seven => sevenOfDiamonds,
                        CardRank.Eight => eightOfDiamonds,
                        CardRank.Nine => nineOfDiamonds,
                        CardRank.Ten => tenOfDiamonds,
                        CardRank.Jack => jackOfDiamonds,
                        CardRank.Queen => queenOfDiamonds,
                        CardRank.King => kingOfDiamonds,
                        _ => null
                    };
                case CardSuit.Hearts:
                    return rank switch
                    {
                        CardRank.Ace => aceOfHearts,
                        CardRank.Two => twoOfHearts,
                        CardRank.Three => threeOfHearts,
                        CardRank.Four => fourOfHearts,
                        CardRank.Five => fiveOfHearts,
                        CardRank.Six => sixOfHearts,
                        CardRank.Seven => sevenOfHearts,
                        CardRank.Eight => eightOfHearts,
                        CardRank.Nine => nineOfHearts,
                        CardRank.Ten => tenOfHearts,
                        CardRank.Jack => jackOfHearts,
                        CardRank.Queen => queenOfHearts,
                        CardRank.King => kingOfHearts,
                        _ => null
                    };
                case CardSuit.Spades:
                    return rank switch
                    {
                        CardRank.Ace => aceOfSpades,
                        CardRank.Two => twoOfSpades,
                        CardRank.Three => threeOfSpades,
                        CardRank.Four => fourOfSpades,
                        CardRank.Five => fiveOfSpades,
                        CardRank.Six => sixOfSpades,
                        CardRank.Seven => sevenOfSpades,
                        CardRank.Eight => eightOfSpades,
                        CardRank.Nine => nineOfSpades,
                        CardRank.Ten => tenOfSpades,
                        CardRank.Jack => jackOfSpades,
                        CardRank.Queen => queenOfSpades,
                        CardRank.King => kingOfSpades,
                        _ => null
                    };
                default:
                    return null;
            }
        }
    }
}