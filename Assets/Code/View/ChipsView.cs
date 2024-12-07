using TMPro;
using UnityEngine;

namespace Code.View
{
    public class ChipsView : MonoBehaviour
    {
        [SerializeField] private TMP_Text chipsText;

        public void UpdateChipsDisplay(int chipCount)
        {
            if (chipsText == null)
            {
                Debug.LogError("Chips text field is not assigned.");
                return;
            }

            chipsText.text = chipCount.ToString();
        }
    }
}