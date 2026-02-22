using TMPro;
using UnityEngine;

namespace Hunger.UI
{
    public class UIManager : MonoBehaviour
    {
        // References to UI text elements in the Canvas
        public TextMeshProUGUI requestText;  // Displays Don's current request
        public TextMeshProUGUI itemText;     // Displays currently carried item
        public TextMeshProUGUI dayText;      // Displays current day (ex: 1/3)
        public TextMeshProUGUI endText;      // Displays win or loss message

        public void UpdateRequest(string request)
        {
            // Updates the request UI text
            requestText.text = "Don Wants: " + request;
        }

        public void UpdateItem(string itemName)
        {
            // Updates the item UI text when player picks something up
            itemText.text = "Item: " + itemName;
        }

        public void ClearItem()
        {
            // Clears the item UI text after sacrificing
            itemText.text = "Item: ";
        }

        public void UpdateDay(int currentDay)
        {
            // Updates the day display
            dayText.text = "Day: " + currentDay + "/3";
        }

        public void ShowEnd(string result)
        {
            // Displays final result message ("Game Won" or "Game Lost")
            endText.text = result;
        }
    }
}