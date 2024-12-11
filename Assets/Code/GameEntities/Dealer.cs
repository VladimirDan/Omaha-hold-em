using System;
using System.Collections.Generic;
using Code.GameEntities.Player;
using UnityEngine;
using Code.Enums;
using System.Linq;
using Code.GameRules;
using Code.GameEntities.Pot;

namespace Code.GameEntities
{
    public class Dealer : MonoBehaviour
    {
        [SerializeField] private Deck deck;
        [SerializeField] public List<PlayerModel> players;
        [SerializeField] private PokerTable pokerTable;
        private CombinationComparer combinationComparer;
        private PotManager potManager;
        private int startingChips = 100;

        private PlayerModel smallBlindPlayer;
        private PlayerModel bigBlindPlayer;

        public int smallBlindBet = 1;
        public int bigBlindBet = 2;

        public void Initialize(List<PlayerModel> players, CombinationComparer combinationComparer)
        {
            this.players = players;
            this.combinationComparer = combinationComparer;
            this.potManager = new PotManager(combinationComparer);

            deck = new Deck();
            deck.Shuffle();
        }

        public void Reset()
        {
            deck = new Deck();
            deck.Shuffle();
        }

        public void DealCardsToPlayers()
        {
            if (pokerTable.playersInGame.Count < 1)
            {
                Debug.LogError("The dealer needs 5 players!");
                return;
            }

            deck.Shuffle();

            for (int i = 0; i < pokerTable.playersInGame.Count; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    pokerTable.playersInGame[i].hand.AddCard(deck.DrawCard());
                }
                //Debug.Log(playerHands[i].ToString());
            }
        }

        public void AssignRoles()
        {
            if (pokerTable.playersInGame.Count < 2)
            {
                Debug.LogError("Not enough players to assign Small Blind and Big Blind.");
                return;
            }
            
            if (smallBlindPlayer == null)
            {
                pokerTable.playersInGame[0].playerRoleManager.SetRole(PlayerRole.SmallBlind);
                smallBlindPlayer = pokerTable.playersInGame[0];

                pokerTable.playersInGame[1].playerRoleManager.SetRole(PlayerRole.BigBlind);
                bigBlindPlayer = pokerTable.playersInGame[1];

                return;
            }
            
            smallBlindPlayer.playerRoleManager.SetRole(PlayerRole.Regular);

            var newSmallBlindPlayer = players
                .SkipWhile(e => e != smallBlindPlayer)
                .Skip(1)
                .First(e => pokerTable.playersInGame.Contains(e));

            newSmallBlindPlayer.playerRoleManager.SetRole(PlayerRole.SmallBlind);
            smallBlindPlayer = newSmallBlindPlayer;

            var newBigBlindPlayer = pokerTable.playersInGame
                .SkipWhile(e => e != newSmallBlindPlayer)
                .Skip(1)
                .First();

            newBigBlindPlayer.playerRoleManager.SetRole(PlayerRole.BigBlind);
            bigBlindPlayer = newBigBlindPlayer;
        }
        
        public void DistributeStartingChips()
        {
            foreach (var player in pokerTable.playersInGame)
            {
                player.stackChipsManager.AddChips(startingChips);
            }

            Debug.Log($"Starting chips distributed: {startingChips} to each player.");
        }

        public void DealFlop()
        {
            List<Card> cardsList = pokerTable.communityCards.Cards;
            if (cardsList.Count > 0)
            {
                Debug.LogError("Flop has already been dealt.");
                return;
            }

            for (int i = 0; i < 3; i++)
            {
                pokerTable.AddCard(deck.DrawCard());
            }

            pokerTable.isFlopMade = true;
        }

        public void DealTurn()
        {
            List<Card> cardsList = pokerTable.communityCards.Cards;
            if (cardsList.Count < 3)
            {
                Debug.LogError("Cannot deal the Turn before the Flop.");
                return;
            }

            pokerTable.AddCard(deck.DrawCard());

            pokerTable.isTurnMade = true;
        }

        public void DealRiver()
        {
            List<Card> cardsList = pokerTable.communityCards.Cards;
            if (cardsList.Count < 4)
            {
                Debug.LogError("Cannot deal the River before the Turn.");
                return;
            }

            pokerTable.AddCard(deck.DrawCard());

            pokerTable.isRiverMade = true;
        }

        public void DistributePotChips()
        {
            potManager.DistributePotChips(pokerTable.playersInGame, pokerTable.playersBets,
                pokerTable.playersCombinations,
                pokerTable.pot);
            pokerTable.ResetPlayerBets();
        }
    }
}