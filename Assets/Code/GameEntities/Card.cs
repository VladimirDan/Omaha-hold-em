using Code.Enums;
using UnityEngine;

namespace Code.GameEntities
{
    [System.Serializable]
    public class Card
    {
        public CardSuit suit;
        public CardRank rank;

        public Card(CardSuit suit, CardRank rank)
        {
            this.suit = suit;
            this.rank = rank;
        }
        
        public Card(Card other)
        {
            this.suit = other.suit;
            this.rank = other.rank;
        }
        
        public override string ToString()
        {
            return $"{rank} of {suit}";
        }
        
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj.GetType() != typeof(Card)) return false;
            Card otherCard = (Card)obj;
            return this.suit == otherCard.suit && this.rank == otherCard.rank;
        }
        
        public override int GetHashCode()
        {
            return (suit, rank).GetHashCode();
        }
    }
}