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
            SceneManager.LoadScene("Credits");
        }

        public void OpenMainMenu()
        {
            SceneManager.LoadScene("MainMenuScene");
        }

        public void OpenWin1()
        {
            SceneManager.LoadScene("WinScene1");
        }

        public void OpenWin2()
        {
            SceneManager.LoadScene("WinScene2");
        }

        public void OpenWin3()
        {
            SceneManager.LoadScene("WinScene3");
        }

        public void OpenGameOver()
        {
            SceneManager.LoadScene("GameOverScene");
        }
    }
}