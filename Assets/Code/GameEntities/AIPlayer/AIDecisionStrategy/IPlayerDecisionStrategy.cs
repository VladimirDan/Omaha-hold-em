using Code.GameEntities.Player;
using Code.Enums;

namespace Code.GameEntities.AIPlayer.AIDecisionStrategy
{
    public interface IPlayerDecisionStrategy
    {
        void ExecuteStrategy(PokerTable pokerTable, PlayerModel player, float chanceToWin)
        {
            
        }
    }
}