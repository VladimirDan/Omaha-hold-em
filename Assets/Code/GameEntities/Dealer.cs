using System.Collections.Generic;
using Code.GameEntities.Player;
using UnityEngine;
using Code.Enums;
using System.Linq;

namespace Code.GameEntities
{
    public class Dealer : MonoBehaviour
    {
        [SerializeField] private Deck deck;
        [SerializeField] public List<PlayerModel> players;
        [SerializeField] private PokerTable pokerTable;
        private int startingChips = 100;

        private PlayerModel smallBlindPlayer;
        private PlayerModel bigBlindPlayer;

        public int smallBlindBet = 1;
        public int bigBlindBet = 2;

        public void Initialize(List<PlayerModel> players)
        {
            this.players = players;

            deck = new Deck();
            deck.Shuffle();
        }

        public void DealCardsToPlayers()
        {
            if (players.Count != 5)
            {
                Debug.LogError("The dealer needs 5 players!");
                return;
            }

            deck.Shuffle();

            for (int i = 0; i < players.Count; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    players[i].hand.AddCard(deck.DrawCard());
                }
                //Debug.Log(playerHands[i].ToString());
            }
        }

        public void AssignRoles()
        {
            if (players.Count < 2)
            {
                Debug.LogError("Not enough players to assign Small Blind and Big Blind.");
                return;
            }

            bool smallBlindAssigned = false;
            for (int i = 0; i < players.Count; i++)
            {
                var currentPlayer = players[i];

                if (currentPlayer.playerRoleManager.role == PlayerRole.SmallBlind)
                {
                    currentPlayer.playerRoleManager.SetRole(PlayerRole.Regular);
                    smallBlindPlayer = currentPlayer;

                    int smallBlindIndex = i;
                    int nextIndex = (smallBlindIndex + 1) % players.Count;
                    players[nextIndex].playerRoleManager.SetRole(PlayerRole.SmallBlind);
                    smallBlindAssigned = true;
                    smallBlindPlayer = players[nextIndex];

                    nextIndex = (nextIndex + 1) % players.Count;
                    players[nextIndex].playerRoleManager.SetRole(PlayerRole.BigBlind);
                    break;
                }
            }

            if (!smallBlindAssigned)
            {
                players[0].playerRoleManager.SetRole(PlayerRole.SmallBlind);
                smallBlindPlayer = players[0];
                
                players[1].playerRoleManager.SetRole(PlayerRole.BigBlind);
                bigBlindPlayer = players[1];
            }
        }
        
        public void DistributeStartingChips()
        {
            foreach (var player in players)
            {
                player.stackChipsManager.AddChips(startingChips); 
            }

            Debug.Log($"Starting chips distributed: {startingChips} to each player.");
        }
        
        public void DealFlop()
        {
            List<Card> cardsList = pokerTable.cardSet.Cards;
            if (cardsList.Count > 0)
            {
                Debug.LogError("Flop has already been dealt.");
                return;
            }

            for (int i = 0; i < 3; i++)
            {
                pokerTable.AddCard(deck.DrawCard());
            }
        }

        public void DealTurn()
        {
            List<Card> cardsList = pokerTable.cardSet.Cards;
            if (cardsList.Count < 3)
            {
                Debug.LogError("Cannot deal the Turn before the Flop.");
                return;
            }

            pokerTable.AddCard(deck.DrawCard());
        }

        public void DealRiver()
        {
            List<Card> cardsList = pokerTable.cardSet.Cards;
            if (cardsList.Count < 4)
            {
                Debug.LogError("Cannot deal the River before the Turn.");
                return;
            }

            pokerTable.AddCard(deck.DrawCard());
        }
    }
}