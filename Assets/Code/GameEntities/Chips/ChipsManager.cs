using UnityEngine;
using Code.View;

namespace Code.GameEntities.Player
{
    public class ChipsManager : MonoBehaviour
    {
        [SerializeField] private ChipsView chipsView;
        private int totalChips;

        public int TotalChips
        {
            get => totalChips;
            set
            {
                if (value < 0)
                {
                    Debug.LogError("Cannot set a negative amount of chips.");
                    return;
                }

                totalChips = value;
                UpdateChipsDisplay();
            }
        }

        public void Initialize()
        {
            UpdateChipsDisplay();
        }
        
        public void AddChips(int amount)
        {
            if (amount <= 0)
            {
                Debug.LogError("Cannot add a non-positive amount of chips.");
                return;
            }

            TotalChips += amount;
        }

        public bool RemoveChips(int amount)
        {
            if (amount <= 0)
            {
                Debug.LogError("Cannot remove a non-positive amount of chips.");
                return false;
            }

            if (amount > TotalChips)
            {
                Debug.LogError("Not enough chips to remove the specified amount.");
                return false;
            }

            TotalChips -= amount;
            return true;
        }

        public void SetChips(int amount)
        {
            if (amount < 0)
            {
                Debug.LogError("Cannot set a negative amount of chips.");
                return;
            }

            TotalChips = amount;
        }

        private void UpdateChipsDisplay()
        {
            if (chipsView != null)
            {
                chipsView.UpdateChipsDisplay(TotalChips);
            }
            else
            {
                Debug.LogWarning("ChipsView is not assigned.");
            }
        }

        public void ResetChipsManager()
        {
            TotalChips = 0;
        }
    }
}
