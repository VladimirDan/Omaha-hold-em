using Code.GameEntities.Player;
using UnityEngine;
using Code.Enums;

namespace Code.GameEntities.AIPlayer.AIDecisionStrategy.Strategies
{
    public class GreedyStrategy : IPlayerDecisionStrategy
    {
        public void ExecuteStrategy(PokerTable pokerTable, PlayerModel player, float chanceToWin)
        {
            if (player.playerActionsManager.CanAllIn(player) && 
                !player.playerActionsManager.IsEveryOpponentInAllIn(player) &&
                Random.value > 0.3f &&
                pokerTable.isRiverMade)
            {
                player.playerActionsManager.Call(player);
            }
            else if(player.playerActionsManager.CanCall(player))
            {
                player.playerActionsManager.CanCall(player);
            }
            else if(player.playerActionsManager.CanCheck(player))
            {
                player.playerActionsManager.Check(player);
            }
            else if(player.playerActionsManager.CanAllIn(player))
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