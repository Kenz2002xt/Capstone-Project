using System.Collections.Generic;
using UnityEngine;

namespace Hunger.Gameplay
{
    public class RoomItemManager : MonoBehaviour
    {
        public GameObject[] roomItems;
        public int itemsToActivate = 2;

        private ExplorationSystem explorationSystem;

        private void Start()
        {
            explorationSystem = FindFirstObjectByType<ExplorationSystem>();
        }

        public void GenerateRoomItems()
        {
            // Turn everything off first
            foreach (GameObject item in roomItems)
            {
                item.SetActive(false);
            }

            // Build valid item pool (NOT consumed)
            List<GameObject> validItems = new List<GameObject>();

            foreach (GameObject obj in roomItems)
            {
                InteractableItem interactable = obj.GetComponent<InteractableItem>();

                if (!explorationSystem.consumedItems.Contains(interactable.item))
                {
                    validItems.Add(obj);
                }
            }

            // Prevent errors if pool is small
            int spawnCount = Mathf.Min(itemsToActivate, validItems.Count);

            List<int> usedIndexes = new List<int>();

            for (int i = 0; i < spawnCount; i++)
            {
                int rand;

                do
                {
                    rand = Random.Range(0, validItems.Count);
                } while (usedIndexes.Contains(rand));

                usedIndexes.Add(rand);

                GameObject selected = validItems[rand];
                selected.SetActive(true);

                // Reset click state each day
                selected.GetComponent<InteractableItem>().ResetItem();
            }
        }
    }
}