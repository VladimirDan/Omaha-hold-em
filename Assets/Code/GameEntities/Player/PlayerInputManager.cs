using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.GameEntities.Player
{
    public class PlayerInputManager : MonoBehaviour
    {
        [SerializeField] private PlayerModel currentPlayer;
        [SerializeField] private TMP_InputField raiseAmountInputField;
        [SerializeField] private Button foldButton;
        [SerializeField] private Button callButton;
        [SerializeField] private Button raiseButton;
        [SerializeField] private Button checkButton;
        [SerializeField] private Button allInButton;

        [SerializeField] private PlayerActionsManager playerActionsManager;

        public void Initialize()
        {
            foldButton.onClick.AddListener(OnFoldButtonClicked);
            callButton.onClick.AddListener(OnCallButtonClicked);
            raiseButton.onClick.AddListener(OnRaiseButtonClicked);
            checkButton.onClick.AddListener(OnCheckButtonClicked);
            allInButton.onClick.AddListener(OnAllInButtonClicked);
        }

        private void OnFoldButtonClicked()
        {
            if (currentPlayer != null)
            {
                playerActionsManager.Fold(currentPlayer);
            }
        }

        private void OnCallButtonClicked()
        {
            if (currentPlayer != null)
            {
                playerActionsManager.Call(currentPlayer);
            }
        }

        private void OnRaiseButtonClicked()
        {
            if (currentPlayer != null && int.TryParse(raiseAmountInputField.text, out int raiseAmount))
            {
                playerActionsManager.Raise(currentPlayer, raiseAmount);
            }
            else
            {
                Debug.LogError("Invalid raise amount entered.");
            }
        }

        private void OnCheckButtonClicked()
        {
            if (currentPlayer != null)
            {
                playerActionsManager.Check(currentPlayer);
            }
        }

        private void OnAllInButtonClicked()
        {
            if (currentPlayer != null)
            {
                playerActionsManager.AllIn(currentPlayer);
            }
        }
    }
}
