using System.Collections.Generic;
using UnityEngine;
using Code.Enums;

namespace Code.GameEntities
{
    [System.Serializable]
    public class Deck
    {
        [SerializeField] private List<Card> cards = new List<Card>();

        public IReadOnlyList<Card> Cards => cards;

        public Deck()
        {
            CreateDeck();
        }

        public void CreateDeck()
        {
            cards.Clear();
            foreach (CardSuit suit in System.Enum.GetValues(typeof(CardSuit)))
            {
                foreach (CardRank rank in System.Enum.GetValues(typeof(CardRank)))
                {
                    cards.Add(new Card(suit, rank));
                }
            }
        }

        public void Shuffle()
        {
            for (int i = cards.Count - 1; i > 0; i--)
            {
                int j = Random.Range(0, i + 1);
                (cards[i], cards[j]) = (cards[j], cards[i]);
            }
        }

        public Card DrawCard()
        {
            if (cards.Count == 0)
            {
                Debug.LogError("The deck is empty!");
                return null;
            }

            Card card = cards[0];
            cards.RemoveAt(0);
            return card;
        }

        public int RemainingCards => cards.Count;
    }

}