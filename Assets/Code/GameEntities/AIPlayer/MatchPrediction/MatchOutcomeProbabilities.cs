using UnityEngine;

namespace Code.GameEntities.AIPlayer
{
    [System.Serializable]
    public struct MatchOutcomeProbabilities
    {
        public float WinChance;
        public float LoseChance;
        public float TieChance;

        public MatchOutcomeProbabilities(float winChance, float loseChance, float tieChance)
        {
            WinChance = winChance;
            LoseChance = loseChance;
            TieChance = tieChance;
        }
    }

}