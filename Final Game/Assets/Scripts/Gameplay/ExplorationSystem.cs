using System.Collections.Generic;
using UnityEngine;
using Hunger.Data;
using Hunger.UI;

namespace Hunger.Gameplay
{
    public class ExplorationSystem : MonoBehaviour
    {
        public UIManager uiManager;

        // Items the player has discovered today
        public List<ItemData> discoveredItems = new List<ItemData>();

        // Limit exploration per day
        public int investigationsRemaining = 2;

        public void ResetInvestigations()
        {
            investigationsRemaining = 2;
        }

        public void DiscoverItem(ItemData item)
        {
            if (!discoveredItems.Contains(item))
            {
                discoveredItems.Add(item);

                // Pick a random dialogue line
                string line = item.descriptions[
                    Random.Range(0, item.descriptions.Length)
                ];

                // Show it on the UI
                uiManager.ShowDialogue(line);

                Debug.Log("Discovered: " + item.itemName);
            }
        }
    }
}