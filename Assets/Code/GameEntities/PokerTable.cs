using System.Collections.Generic;
using Code.GameEntities.Player;
using UnityEngine;
using System.Linq;
using Code.View;
using UnityEngine.Serialization;

namespace Code.GameEntities
{
    public class PokerTable : MonoBehaviour
    {
        [SerializeField] public CardSet communityCards;
        private int communityCardsMaxCount = 5;
        public int pot = 0;
        [SerializeField] public ChipsManager currentBet;
        public List<PlayerModel> playersInGame;
        public List<PlayerModel> players;

        public bool isFlopMade = false;
        public bool isTurnMade = false;
        public bool isRiverMade = false;
        
        [SerializeField] public CardsView cardsView;

        public Dictionary<PlayerModel, ChipsManager> playersBets = new Dictionary<PlayerModel, ChipsManager>();
        public Dictionary<PlayerModel, List<Card>> playersCombinations = new Dictionary<PlayerModel, List<Card>>();

        public void Initialize(List<PlayerModel> players)
        {
            communityCards = new CardSet(communityCardsMaxCount);
            cardsView.UpdateCardsView(communityCards.Cards, false);
            this.players = players;
            playersInGame = new List<PlayerModel>();
            foreach (var player in players)
            {
                playersInGame.Add(player);
            }
            
            InitializePlayerBets();
            currentBet.Initialize();
        }

        public void Reset()
        {
            communityCards.Reset();
            cardsView.UpdateCardsView(communityCards.Cards, false);
            cardsView.ResetCardsColorAndTransparency();
            playersInGame.Clear();
            foreach (var player in players)
            {
                player.Reset();
                playersInGame.Add(player);
            }
            currentBet.Reset();
            
            isFlopMade = false;
            isTurnMade = false;
            isRiverMade = false;
            pot = 0;
        }
        
        public void InitializePlayerBets()
        {
            playersBets.Clear();
            
            if (playersInGame.Count == 0) return;

            for (int i = 0; i < playersInGame.Count; i++)
            {
                var player = playersInGame[i];
                playersBets[player] = playersInGame[i].betChipsManager;
                playersBets[player].Initialize();
            }
        }
        
        public void ResetPlayerBets()
        {
            foreach (var playerBet in playersBets)
            {
                playerBet.Value.Reset();
            }
            currentBet.Reset();
        }
        
        public void AddCard(Card card)
        {
            communityCards.AddCard(card);
            cardsView.UpdateCardsView(communityCards.Cards, false);
        }
        
        public void IncreasePlayerBet(PlayerModel player, int betAmount)
        {
            if (playersBets.ContainsKey(player))
            {
                playersBets[player].TotalChips = betAmount;
                if (betAmount > currentBet.TotalChips)
                    currentBet.AddChips(betAmount - currentBet.TotalChips);
            }
            else
            {
                Debug.LogError($"Игрок {player.name} не добавлен в игру.");
            }
        }

        public int GetPlayerBet(PlayerModel player)
        {
            if (playersBets.ContainsKey(player))
            {
                return playersBets[player].TotalChips;
            }
            else
            {
                Debug.LogError($"Игрок {player.name} не добавлен в игру.");
                return 0;
            }
        }

        public void AddToPot(int amount)
        {
            pot += amount;
        }
        
        public void RemovePlayerFromActivePlayers(PlayerModel player)
        {
            if (playersInGame.Remove(player))
            {
                
            }
            else
            {
                Debug.LogWarning($"Player {player.name} was not found in the game.");
            }
        }
    }
}