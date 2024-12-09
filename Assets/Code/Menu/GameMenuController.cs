using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Code.Menu
{
    public class GameMenuController : MonoBehaviour
    {
        [SerializeField] private GameObject menuPanel;

        public void OpenMenu()
        {
            menuPanel.SetActive(true);
        }

        public void CloseMenu()
        {
            menuPanel.SetActive(false);
        }

        public void RestartGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void ExitGame()
        {
            Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }
}