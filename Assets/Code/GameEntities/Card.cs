using Code.Enums;
using UnityEngine;

namespace Code.GameEntities
{
    [System.Serializable]
    public class Card
    {
        public CardSuit Suit { get; private set; }
        public CardRank Rank { get; private set; }

        public Card(CardSuit suit, CardRank rank)
        {
            Suit = suit;
            Rank = rank;
        }

        public int Strength => (int)Rank;

        public override string ToString()
        {
            return $"{Rank} of {Suit} (Strength: {Strength})";
        }
    }
}