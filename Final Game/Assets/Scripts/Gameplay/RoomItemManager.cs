using UnityEngine;

namespace Hunger.Gameplay
{
    public class RoomItemManager : MonoBehaviour
    {
        public GameObject[] roomItems;
        public int itemsToActivate = 2;

        private ExplorationSystem explorationSystem;
        private int itemsDiscoveredThisVisit = 0;

        private void Start()
        {
            explorationSystem = FindFirstObjectByType<ExplorationSystem>();
        }

        public void GenerateRoomItems()
        {
            itemsDiscoveredThisVisit = 0;

            foreach (GameObject item in roomItems)
            {
                InteractableItem interactable = item.GetComponent<InteractableItem>();

                if (interactable == null)
                    continue;

                if (explorationSystem.consumedItems.Contains(interactable.item))
                {
                    item.SetActive(false); // sacrificed forever
                }
                else
                {
                    item.SetActive(true);
                    interactable.SetAvailableForToday(true); // all visible, black, clickable
                    interactable.SetRoomManager(this); // let item report back when clicked
                }
            }
        }

        public void ResetAllRoomItemsForNewDay()
        {
            foreach (GameObject item in roomItems)
            {
                if (item == null)
                    continue;

                InteractableItem interactable = item.GetComponent<InteractableItem>();

                if (interactable == null)
                    continue;

                if (explorationSystem.consumedItems.Contains(interactable.item))
                {
                    item.SetActive(false); // still gone forever
                }
                else
                {
                    item.SetActive(true);
                    interactable.ResetItem();
                }
            }
        }

        public bool CanDiscoverMoreItems()
        {
            return itemsDiscoveredThisVisit < itemsToActivate;
        }

        public void RegisterItemDiscovery()
        {
            itemsDiscoveredThisVisit++;
        }
    }
}