using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Code.GameEntities.Player;
using Code.GameEntities;
using Code.Menu;

namespace Code.InputManager_
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] private PlayerModel currentPlayer;
        [SerializeField] private CardSelectionController cardSelectionController;
        [SerializeField] private TMP_InputField raiseAmountInputField;
        [SerializeField] private Button foldButton;
        [SerializeField] private Button callButton;
        [SerializeField] private Button raiseButton;
        [SerializeField] private Button checkButton;
        [SerializeField] private Button allInButton;
        [SerializeField] private Button submitCombinationButton;
        [SerializeField] private Button restartPokerRound;
        
        [SerializeField] private Button menuButton;
        [SerializeField] private Button continueButton;
        [SerializeField] private Button restartButton;
        [SerializeField] private Button exitButton;
        
        [SerializeField] private Button[] selectCardButtons;

        [SerializeField] private PlayerActionsManager playerActionsManager;
        [SerializeField] private GameLoopManager gameLoopManager;
        [SerializeField] private GameMenuController gameMenuController;

        public void Initialize()
        {
            foldButton.onClick.AddListener(OnFoldButtonClicked);
            callButton.onClick.AddListener(OnCallButtonClicked);
            raiseButton.onClick.AddListener(OnRaiseButtonClicked);
            checkButton.onClick.AddListener(OnCheckButtonClicked);
            allInButton.onClick.AddListener(OnAllInButtonClicked);
            submitCombinationButton.onClick.AddListener(OnSubmitCombinationButtonClicked);
            restartPokerRound.onClick.AddListener(OnRestartPokerRoundButtonClicked);
            
            menuButton.onClick.AddListener(OnMenuButtonPressed);
            continueButton.onClick.AddListener(OnContinueButtonPressed);
            restartButton.onClick.AddListener(OnRestartButtonPressed);
            exitButton.onClick.AddListener(OnExitButtonPressed);

            foreach (Button button in selectCardButtons)
            {
                button.onClick.AddListener(() => OnSelectCardButtonClicked(button.gameObject));
            }
        }

        private void OnFoldButtonClicked()
        {
            if (currentPlayer != null && playerActionsManager.CanFold(currentPlayer))
            {
                playerActionsManager.Fold(currentPlayer);
            }
        }

        private void OnCallButtonClicked()
        {
            if (currentPlayer != null && playerActionsManager.CanCall(currentPlayer))
            {
                playerActionsManager.Call(currentPlayer);
            }
        }

        private void OnRaiseButtonClicked()
        {
            if (currentPlayer != null && int.TryParse(raiseAmountInputField.text, out int raiseAmount) &&
                playerActionsManager.CanRaise(currentPlayer, raiseAmount))
            {
                playerActionsManager.Raise(currentPlayer, raiseAmount);
            }
            else
            {
                Debug.LogError("Cant raise");
            }
        }

        private void OnCheckButtonClicked()
        {
            if (currentPlayer != null && playerActionsManager.CanCheck(currentPlayer))
            {
                playerActionsManager.Check(currentPlayer);
            }
        }

        private void OnAllInButtonClicked()
        {
            if (currentPlayer != null && playerActionsManager.CanAllIn(currentPlayer))
            {
                playerActionsManager.AllIn(currentPlayer);
            }
        }
        
        private void OnSelectCardButtonClicked(GameObject buttonObject)
        {
            if (cardSelectionController != null && playerActionsManager.CanSelectCards())
            {
                cardSelectionController.HandleCardSelection(buttonObject.GetComponent<Image>());
            }
        }
        
        private void OnSubmitCombinationButtonClicked()
        {
            if (cardSelectionController != null && playerActionsManager.CanSubmitCombination())
            {
                playerActionsManager.SubmitCombination(currentPlayer);
            }
        }
        
        private void OnRestartPokerRoundButtonClicked()
        {
            if (gameLoopManager != null && gameLoopManager.isPotDistributed == true)
            {
                gameLoopManager.isPokerRoundOver = true;
            }
        }
        
        
       
        public void OnMenuButtonPressed()
        {
            gameMenuController.OpenMenu();
        }
        
        public void OnContinueButtonPressed()
        {
            gameMenuController.CloseMenu();
        }

        public void OnRestartButtonPressed()
        {
            gameMenuController.RestartGame();
        }

        public void OnExitButtonPressed()
        {
            gameMenuController.ExitGame();
        }
    }
}
