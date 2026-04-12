using System.Collections.Generic;
using UnityEngine;
using Hunger.Data;
using Hunger.UI;
using Hunger.Managers;

namespace Hunger.Gameplay
{
    public class ExplorationSystem : MonoBehaviour
    {
        public UIManager uiManager;

        // Items discovered THIS DAY
        public List<ItemData> discoveredItems = new List<ItemData>();

        // Items permanently used this run
        public List<ItemData> consumedItems = new List<ItemData>();

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
            if (!discoveredItems.Contains(item) && !consumedItems.Contains(item))
            {
                discoveredItems.Add(item);

                string line = item.descriptions[
                    Random.Range(0, item.descriptions.Length)
                ];

                uiManager.ShowDialogue(line);

                Debug.Log("Discovered: " + item.itemName);

                GameManager gm = FindFirstObjectByType<GameManager>();
                UIManager ui = FindFirstObjectByType<UIManager>();

                if (gm != null && ui != null && gm.currentDay == 1 && discoveredItems.Count >= 4)
                {
                    ui.ShowDayOneInstruction("Head to the barn through the porch.");
                }
            }
        }
    }
}