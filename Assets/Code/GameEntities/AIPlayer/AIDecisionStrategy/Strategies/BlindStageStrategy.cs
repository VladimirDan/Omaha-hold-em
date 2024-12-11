using Code.GameEntities.Player;
using UnityEngine;
using Code.Enums;

namespace Code.GameEntities.AIPlayer.AIDecisionStrategy.Strategies
{
    public class BlindStageStrategy : IPlayerDecisionStrategy
    {
        public int smallBlindBet = 1;
        public int bigBlindBet = 2;
        
        public void ExecuteStrategy(PokerTable pokerTable, PlayerModel player, float chanceToWin)
        {
            if (player.playerActionsManager.CanCheck(player))
            {
                player.playerActionsManager.Check(player);
            }
            else if (player.playerActionsManager.CanCall(player) && pokerTable.currentBet.TotalChips <= bigBlindBet)
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