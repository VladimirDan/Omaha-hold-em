using TMPro;
using UnityEngine;

namespace Code.View
{
    public class ChipsView : MonoBehaviour
    {
        [SerializeField] private TMP_Text chipsText;
        [SerializeField] private GameObject chipsPanel;

        public void UpdateChipsDisplay(int chipCount)
        {
            if (chipCount > 0)
            {
                EnableChipsPanel();
            }
            else
            {
                DisableChipsPanel();
            }

            chipsText.text = chipCount.ToString();
        }

        public void EnableChipsPanel()
        {
            chipsPanel.SetActive(true);
        }
        
        public void DisableChipsPanel()
        {
            chipsPanel.SetActive(false);
        }
    }
}