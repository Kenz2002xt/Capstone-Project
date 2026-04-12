using UnityEngine;
using UnityEngine.SceneManagement;

namespace Hunger.Managers
{
    public class SceneLoader : MonoBehaviour
    {
        public void PlayGame()
        {
            SceneManager.LoadScene("MainScene");
        }

        public void OpenControls()
        {
            SceneManager.LoadScene("ControlsScene");
        }

        public void OpenCredits()
        {
            SceneManager.LoadScene("CreditsScene");
        }

        public void OpenMainMenu()
        {
            SceneManager.LoadScene("MainMenuScene");
        }

        public void OpenWin()
        {
            SceneManager.LoadScene("WinScene");
        }

        public void OpenGameOver()
        {
            SceneManager.LoadScene("GameOverScene");
        }
    }
}