using System.Collections.Generic;
using Hunger.Managers;
using Hunger.Systems;
using Hunger.UI;
using Hunger.Data;
using UnityEngine;

namespace Hunger.Gameplay
{
    public class SacrificeSystem : MonoBehaviour
    {
        public StatSystem statSystem;
        public RequestSystem requestSystem;

        // Stores all items the player has discovered through exploration
        public List<ItemData> unlockedItems = new List<ItemData>();

        // Stores the item currently selected for sacrifice
        private ItemData selectedItem = null;

        // Tracks whether the player is inside the barn trigger area
        private bool playerInRange = false;

        // Called when the player discovers an item through exploration
        public void AddItem(ItemData item)
        {
            unlockedItems.Add(item);
            Debug.Log("Discovered: " + item.itemName);
        }

        // Called when the player selects an item to sacrifice
        public void SelectItem(ItemData item)
        {
            selectedItem = item;
            Debug.Log("Selected for sacrifice: " + item.itemName);
        }

        private void Update()
        {
            // If the player is not inside the barn trigger, do nothing
            if (!playerInRange) return;

            // If player presses the "1" key while inside the barn
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                // Sacrifice the selected item
                SacrificeSelectedItem();
            }
        }

        void SacrificeSelectedItem()
        {
            // If the player has not selected anything, stop
            if (selectedItem == null)
            {
                Debug.Log("No item selected.");
                return;
            }

            // Get the item's category (Home, Self, Family)
            string category = selectedItem.statTag;

            // Reduce the corresponding survival stat based on the item's category
            statSystem.ReduceStat(category);
            Debug.Log(selectedItem.itemName + " sacrificed!");

            // Remove the item so it cannot be used again
            unlockedItems.Remove(selectedItem);

            // Clear the selected item
            selectedItem = null;

            // Tell the GameManager to end the current day
            FindFirstObjectByType<GameManager>().EndDay();
        }

        private void OnTriggerEnter(Collider other)
        {
            // If the player enters the barn trigger
            if (other.CompareTag("Player"))
            {
                // Allow sacrificing
                playerInRange = true;
                Debug.Log("Press 1 to sacrifice selected item.");
            }
        }

        private void OnTriggerExit(Collider other)
        {
            // If the player leaves the barn trigger
            if (other.CompareTag("Player"))
            {
                // Disable sacrificing
                playerInRange = false;
            }
        }
    }
}