using UnityEngine;

namespace Hunger.UI
{
    public class PauseMenu : MonoBehaviour
    {
        public GameObject pausePanel;

        private bool isPaused = false;

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                TogglePause();
            }
        }

        public void TogglePause()
        {
            isPaused = !isPaused;

            pausePanel.SetActive(isPaused);

            // Optional: freeze game time
            Time.timeScale = isPaused ? 0f : 1f;

            // Show cursor when paused
            Cursor.visible = isPaused;
            Cursor.lockState = isPaused ? CursorLockMode.None : CursorLockMode.Locked;
        }

        // Hook this to your X button
        public void ClosePauseMenu()
        {
            isPaused = false;

            pausePanel.SetActive(false);
            Time.timeScale = 1f;

            //Cursor.visible = false;
            //Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
