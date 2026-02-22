using Hunger.Systems;
using Hunger.UI;
using UnityEngine;

namespace Hunger.Managers
{
    public class GameManager : MonoBehaviour
    {
        public int currentDay = 1;   // Tracks the current day (starts at Day 1)
        public int maxDays = 3;      // Maximum number of days required to win

        public StatSystem statSystem;       // Reference to the stat system (checks if player is dead)
        public RequestSystem requestSystem; // Reference to the request system (generates Don's request)

        void Start()
        {
            Debug.Log("Game Manager Initialized");

            // Begins the first day of the game
            StartDay();
        }

        public void StartDay()
        {
            // Updates the UI to show the current day (ex: Day 1/3)
            FindFirstObjectByType<UIManager>().UpdateDay(currentDay);
            Debug.Log("Day " + currentDay);

            // Generates a new request for day
            requestSystem.GenerateRequest();
        }

        public void EndDay()
        {
            // Check if any survival stat has reached zero
            if (statSystem.IsDead())
            {
                // If player is dead then show Game Lost on the UI
                FindFirstObjectByType<UIManager>().ShowEnd("Game Lost");
                Debug.Log("You did not survive winter");
                return;
            }

            // Move to the next day
            currentDay++;

            // If player completed more days than required
            if (currentDay > maxDays)
            {
                // Show Game Won message on UI
                FindFirstObjectByType<UIManager>().ShowEnd("Game Won");
                Debug.Log("You survived until spring");
                return;
            }

            // If player is still alive and days remain, start the next day
            StartDay();
        }
    }
}
