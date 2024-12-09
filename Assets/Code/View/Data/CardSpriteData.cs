using Code.GameEntities;
using UnityEngine;
using Code.Enums;

namespace Code.View
{
    [CreateAssetMenu(fileName = "CardSprites", menuName = "CardSprites/Deck", order = 1)]
    public class CardSpriteData : ScriptableObject
    {
        [Header("Card Sprites")] [SerializeField]
        private Sprite aceOfClubs;

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

        [Header("Card Back")] [SerializeField] private Sprite cardBack;

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

        public Card GetCardFromSprite(Sprite sprite)
        {
            if (sprite == aceOfClubs) return new Card(CardSuit.Clubs, CardRank.Ace);
            if (sprite == twoOfClubs) return new Card(CardSuit.Clubs, CardRank.Two);
            if (sprite == threeOfClubs) return new Card(CardSuit.Clubs, CardRank.Three);
            if (sprite == fourOfClubs) return new Card(CardSuit.Clubs, CardRank.Four);
            if (sprite == fiveOfClubs) return new Card(CardSuit.Clubs, CardRank.Five);
            if (sprite == sixOfClubs) return new Card(CardSuit.Clubs, CardRank.Six);
            if (sprite == sevenOfClubs) return new Card(CardSuit.Clubs, CardRank.Seven);
            if (sprite == eightOfClubs) return new Card(CardSuit.Clubs, CardRank.Eight);
            if (sprite == nineOfClubs) return new Card(CardSuit.Clubs, CardRank.Nine);
            if (sprite == tenOfClubs) return new Card(CardSuit.Clubs, CardRank.Ten);
            if (sprite == jackOfClubs) return new Card(CardSuit.Clubs, CardRank.Jack);
            if (sprite == queenOfClubs) return new Card(CardSuit.Clubs, CardRank.Queen);
            if (sprite == kingOfClubs) return new Card(CardSuit.Clubs, CardRank.King);

            if (sprite == aceOfDiamonds) return new Card(CardSuit.Diamonds, CardRank.Ace);
            if (sprite == twoOfDiamonds) return new Card(CardSuit.Diamonds, CardRank.Two);
            if (sprite == threeOfDiamonds) return new Card(CardSuit.Diamonds, CardRank.Three);
            if (sprite == fourOfDiamonds) return new Card(CardSuit.Diamonds, CardRank.Four);
            if (sprite == fiveOfDiamonds) return new Card(CardSuit.Diamonds, CardRank.Five);
            if (sprite == sixOfDiamonds) return new Card(CardSuit.Diamonds, CardRank.Six);
            if (sprite == sevenOfDiamonds) return new Card(CardSuit.Diamonds, CardRank.Seven);
            if (sprite == eightOfDiamonds) return new Card(CardSuit.Diamonds, CardRank.Eight);
            if (sprite == nineOfDiamonds) return new Card(CardSuit.Diamonds, CardRank.Nine);
            if (sprite == tenOfDiamonds) return new Card(CardSuit.Diamonds, CardRank.Ten);
            if (sprite == jackOfDiamonds) return new Card(CardSuit.Diamonds, CardRank.Jack);
            if (sprite == queenOfDiamonds) return new Card(CardSuit.Diamonds, CardRank.Queen);
            if (sprite == kingOfDiamonds) return new Card(CardSuit.Diamonds, CardRank.King);

            if (sprite == aceOfHearts) return new Card(CardSuit.Hearts, CardRank.Ace);
            if (sprite == twoOfHearts) return new Card(CardSuit.Hearts, CardRank.Two);
            if (sprite == threeOfHearts) return new Card(CardSuit.Hearts, CardRank.Three);
            if (sprite == fourOfHearts) return new Card(CardSuit.Hearts, CardRank.Four);
            if (sprite == fiveOfHearts) return new Card(CardSuit.Hearts, CardRank.Five);
            if (sprite == sixOfHearts) return new Card(CardSuit.Hearts, CardRank.Six);
            if (sprite == sevenOfHearts) return new Card(CardSuit.Hearts, CardRank.Seven);
            if (sprite == eightOfHearts) return new Card(CardSuit.Hearts, CardRank.Eight);
            if (sprite == nineOfHearts) return new Card(CardSuit.Hearts, CardRank.Nine);
            if (sprite == tenOfHearts) return new Card(CardSuit.Hearts, CardRank.Ten);
            if (sprite == jackOfHearts) return new Card(CardSuit.Hearts, CardRank.Jack);
            if (sprite == queenOfHearts) return new Card(CardSuit.Hearts, CardRank.Queen);
            if (sprite == kingOfHearts) return new Card(CardSuit.Hearts, CardRank.King);

            if (sprite == aceOfSpades) return new Card(CardSuit.Spades, CardRank.Ace);
            if (sprite == twoOfSpades) return new Card(CardSuit.Spades, CardRank.Two);
            if (sprite == threeOfSpades) return new Card(CardSuit.Spades, CardRank.Three);
            if (sprite == fourOfSpades) return new Card(CardSuit.Spades, CardRank.Four);
            if (sprite == fiveOfSpades) return new Card(CardSuit.Spades, CardRank.Five);
            if (sprite == sixOfSpades) return new Card(CardSuit.Spades, CardRank.Six);
            if (sprite == sevenOfSpades) return new Card(CardSuit.Spades, CardRank.Seven);
            if (sprite == eightOfSpades) return new Card(CardSuit.Spades, CardRank.Eight);
            if (sprite == nineOfSpades) return new Card(CardSuit.Spades, CardRank.Nine);
            if (sprite == tenOfSpades) return new Card(CardSuit.Spades, CardRank.Ten);
            if (sprite == jackOfSpades) return new Card(CardSuit.Spades, CardRank.Jack);
            if (sprite == queenOfSpades) return new Card(CardSuit.Spades, CardRank.Queen);
            if (sprite == kingOfSpades) return new Card(CardSuit.Spades, CardRank.King);

            return null;
        }

        public Sprite GetCardBack()
        {
            return cardBack;
        }
    }
}