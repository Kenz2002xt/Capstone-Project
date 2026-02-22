using Hunger.UI;
using UnityEngine;

namespace Hunger.Systems
{
    public class RequestSystem : MonoBehaviour
    {
        public string currentRequest;   // Stores the current request for the day

        // List of possible request categories Don can ask for
        private string[] requests = { "Warmth", "Comfort", "Protection" };

        public void GenerateRequest()
        {
            // Generate a random number between 0 and the number of requests
            int randomNumber = Random.Range(0, requests.Length);

            // Select a request from the array using the random index
            currentRequest = requests[randomNumber];

            // Update the UI to display the new request
            FindFirstObjectByType<UIManager>().UpdateRequest(currentRequest);
            Debug.Log("Don asks for: " + currentRequest);
        }
    }
}
