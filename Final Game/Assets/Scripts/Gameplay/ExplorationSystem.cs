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

        // Room exploration limits
        public int roomsExploredToday = 0;
        public int maxRoomsPerDay = 2;

        // ---------- DAY RESET ----------

        public void ResetDay()
        {
            roomsExploredToday = 0;
            discoveredItems.Clear();
        }

        // ---------- ROOM CHECK ----------

        public bool CanExploreRoom()
        {
            return roomsExploredToday < maxRoomsPerDay;
        }

        public void RoomExplored()
        {
            roomsExploredToday++;

            Debug.Log("Rooms explored today: " + roomsExploredToday);
        }

        // ---------- ITEM DISCOVERY ----------

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