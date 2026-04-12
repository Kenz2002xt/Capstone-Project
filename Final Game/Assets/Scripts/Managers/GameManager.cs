using Hunger.Gameplay;
using Hunger.Systems;
using Hunger.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Hunger.Managers
{
    public class GameManager : MonoBehaviour
    {
        public int currentDay = 1;   // Tracks the current day (starts at Day 1)
        public int maxDays = 10;      // Maximum number of days required to win

        public StatSystem statSystem;       // Reference to the stat system (checks if player is dead)
        public RequestSystem requestSystem; // Reference to the request system (generates Don's request)

        public CameraSwitcher cameraSwitcher; // Reference to camera system
        public UIManager uiManager; // Reference to UI
        public ExplorationSystem explorationSystem; // Reference to exploration
        public NarrativeManager narrativeManager;

        void Start()
        {
            Debug.Log("Game Manager Initialized");

            // Begins the first day of the game
            StartDay();
        }

        public void StartDay()
        {
            // --- RESET CAMERA ---
            cameraSwitcher.GoToStart();

            // --- RESET UI ---
            uiManager.journal.SetActive(true);          // bring journal back
            uiManager.optionsPanel.SetActive(false);    // close journal panel
            uiManager.HideSacrificeOptions();           // hide sacrifice UI

            // --- RESET CURSOR ---
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            // --- RESET EXPLORATION ---
            explorationSystem.ResetDay();
            uiManager.kitchenRoomManager.ResetAllRoomItemsForNewDay();
            uiManager.sisterRoomManager.ResetAllRoomItemsForNewDay();
            uiManager.parentRoomManager.ResetAllRoomItemsForNewDay();
            uiManager.bathroomManager.ResetAllRoomItemsForNewDay();

            // --- UPDATE UI TEXT ---
            uiManager.UpdateDay(currentDay);
            //requestSystem.GenerateRequest();
            narrativeManager.StartDayFlow(currentDay);

            Debug.Log("Day " + currentDay);
        }

        public void EndDay()
        {
            // Check if any survival stat has reached zero
            if (statSystem.IsDead())
            {
                // If player is dead then show Game Lost on the UI
                SceneManager.LoadScene("GameOverScene");
                Debug.Log("You did not survive winter");
                return;
            }

            // Move to the next day
            currentDay++;

            // If player completed more days than required
            if (currentDay > maxDays)
            {
                // Show Game Won message on UI
                SceneManager.LoadScene("WinScene");
                Debug.Log("You survived until spring");
                return;
            }

            // If player is still alive and days remain, start the next day
            StartDay();
        }
    }
}
