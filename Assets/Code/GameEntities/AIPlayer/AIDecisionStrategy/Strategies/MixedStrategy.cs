using Code.GameEntities.Player;
using UnityEngine;
using Code.Enums;

namespace Code.GameEntities.AIPlayer.AIDecisionStrategy.Strategies
{
    public class MixedStrategy : IPlayerDecisionStrategy
    {
        public void ExecuteStrategy(PokerTable pokerTable, PlayerModel player, float chanceToWin)
        {
            float raisePercent = 5;
            int raiseAmount = Mathf.RoundToInt((raisePercent / 100f) * player.stackChipsManager.TotalChips + pokerTable.currentBet.TotalChips);
            int maxCallAmount = Mathf.RoundToInt(chanceToWin * (player.betChipsManager.TotalChips + player.stackChipsManager.TotalChips));

            if (Random.value < 0.1f && 
                chanceToWin > 0.4f &&
                player.playerActionsManager.CanAllIn(player) && 
                pokerTable.isRiverMade &&
                !player.playerActionsManager.IsEveryOpponentInAllIn(player))
            {
                player.playerActionsManager.AllIn(player);
            }
            else if (Random.value < (chanceToWin/2) && 
                     pokerTable.isFlopMade &&
                     player.playerActionsManager.CanRaise(player, raiseAmount))
            {
                player.playerActionsManager.Raise(player, raiseAmount);
            }
            else if (player.playerActionsManager.CanCall(player) && pokerTable.currentBet.TotalChips < maxCallAmount)
            {
                player.playerActionsManager.Call(player);
            }
            else if(player.playerActionsManager.CanCheck(player))
            {
                player.playerActionsManager.Check(player);
            }
            else if (pokerTable.currentBet.TotalChips != player.betChipsManager.TotalChips && pokerTable.currentBet.TotalChips < maxCallAmount)
            {
                player.playerActionsManager.AllIn(player);
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