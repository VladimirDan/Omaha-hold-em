using Code.GameEntities.Player;
using UnityEngine;
using Code.Enums;

namespace Code.GameEntities.AIPlayer.AIDecisionStrategy.Strategies
{
    public class PassiveStrategy : IPlayerDecisionStrategy
    {
        public void ExecuteStrategy(PokerTable pokerTable, PlayerModel player, float chanceToWin)
        {
            int maxCallAmount = Mathf.RoundToInt(0.2f * (player.betChipsManager.TotalChips + player.stackChipsManager.TotalChips));
            
            if (player.playerActionsManager.CanCheck(player))
            {
                player.playerActionsManager.Check(player);
            }
            else if (player.playerActionsManager.CanCall(player) && pokerTable.currentBet.TotalChips < maxCallAmount)
            {
                player.playerActionsManager.Call(player);
            }
            else if(player.playerActionsManager.CanFold(player))
            {
                player.playerActionsManager.Fold(player);
            }
            else
            {
                Debug.LogError("AI is broken");
            }
        }
    }
}