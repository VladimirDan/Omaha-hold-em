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
        [SerializeField] public CardSet cardSet;
        private int communityCardsCount = 5;
        public int pot = 0;
        [SerializeField] public ChipsManager currentBet;
        public List<PlayerModel> playersInGame;
        
        [SerializeField] public CardsView cardsView;

        public Dictionary<PlayerModel, ChipsManager> playerBets = new Dictionary<PlayerModel, ChipsManager>();

        public void Initialize(List<PlayerModel> players)
        {
            cardSet = new CardSet(communityCardsCount);
            cardsView.UpdateCardsView(cardSet.Cards, false);
            playersInGame = new List<PlayerModel>();
            foreach (var player in players)
            {
                playersInGame.Add(player);
            }
            
            InitializePlayerBets();
            currentBet.Initialize();
        }
        
        public void InitializePlayerBets()
        {
            playerBets.Clear();
            
            if (playersInGame.Count == 0) return;

            for (int i = 0; i < playersInGame.Count; i++)
            {
                var player = playersInGame[i];
                playerBets[player] = playersInGame[i].betChipsManager;
                playerBets[player].Initialize();
            }
        }
        
        public void AddCard(Card card)
        {
            cardSet.AddCard(card);
            cardsView.UpdateCardsView(cardSet.Cards, false);
        }
        
        public void SetPlayerBet(PlayerModel player, int betAmount)
        {
            if (playerBets.ContainsKey(player))
            {
                playerBets[player].TotalChips = betAmount;
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
            if (playerBets.ContainsKey(player))
            {
                return playerBets[player].TotalChips;
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