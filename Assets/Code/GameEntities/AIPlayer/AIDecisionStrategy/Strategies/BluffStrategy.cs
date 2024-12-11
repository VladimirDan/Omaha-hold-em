using Code.GameEntities.Player;
using UnityEngine;
using Code.Enums;

namespace Code.GameEntities.AIPlayer.AIDecisionStrategy.Strategies
{
    public class BluffStrategy : IPlayerDecisionStrategy
    {
        public void ExecuteStrategy(PokerTable pokerTable, PlayerModel player, float chanceToWin)
        {
            float raisePercent = Random.Range(5f, 10f);
            int raiseAmount = Mathf.RoundToInt((raisePercent / 100f) * player.stackChipsManager.TotalChips + pokerTable.currentBet.TotalChips);

            if (Random.value < 0.1f && 
                player.playerActionsManager.CanAllIn(player) && 
                pokerTable.isRiverMade &&
                !player.playerActionsManager.IsEveryOpponentInAllIn(player))
            {
                player.playerActionsManager.AllIn(player);
            }
            else if (Random.value < 0.5f && player.playerActionsManager.CanRaise(player, raiseAmount))
            {
                player.playerActionsManager.Raise(player, raiseAmount);
            }
            else if (player.playerActionsManager.CanCall(player))
            {
                player.playerActionsManager.Call(player);
            }
            else if(player.playerActionsManager.CanCheck(player))
            {
                player.playerActionsManager.Check(player);
            }
            else if (player.playerActionsManager.CanAllIn(player))
            {
                player.playerActionsManager.AllIn(player);
            }
            else
            {
                Debug.LogError("AI is broken");
            }
        }
    }

}