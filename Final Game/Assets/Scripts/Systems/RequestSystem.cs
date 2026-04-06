using Hunger.UI;
using Hunger.Managers;
using UnityEngine;
using System.Collections.Generic;

namespace Hunger.Systems
{
    public class RequestSystem : MonoBehaviour
    {
        public string currentRequest;

        // TRACK USED REQUESTS
        private List<string> usedRequests = new List<string>();

        // Early game
        private string[] earlyRequests =
        {
            "Warmth",
            "Comfort",
            "Protection",
            "Noise",
            "Distraction"
        };

        // Mid game
        private string[] midRequests =
        {
            "Memory",
            "Escape",
            "Information",
            "Silence"
        };

        // Late game (creepy shift)
        private string[] lateRequests =
        {
            "Hunger",
            "Sight",
            "Blood"
        };

        public void GenerateRequest()
        {
            GameManager gameManager = FindFirstObjectByType<GameManager>();
            int currentDay = gameManager.currentDay;

            List<string> pool = new List<string>();

            // Build pool based on day
            if (currentDay <= 3)
            {
                pool.AddRange(earlyRequests);
            }
            else if (currentDay <= 6)
            {
                pool.AddRange(earlyRequests);
                pool.AddRange(midRequests);
            }
            else
            {
                pool.AddRange(earlyRequests);
                pool.AddRange(midRequests);
                pool.AddRange(lateRequests);
            }

            // REMOVE USED REQUESTS
            foreach (string used in usedRequests)
            {
                pool.Remove(used);
            }

            // SAFETY
            if (pool.Count == 0)
            {
                Debug.LogWarning("No more unique requests available!");
                return;
            }

            // PICK RANDOM
            int randomIndex = Random.Range(0, pool.Count);
            currentRequest = pool[randomIndex];

            // STORE IT
            usedRequests.Add(currentRequest);

            // UPDATE UI
            FindFirstObjectByType<UIManager>().UpdateRequest(currentRequest);
            Debug.Log("Don asks for: " + currentRequest);
        }
    }
}