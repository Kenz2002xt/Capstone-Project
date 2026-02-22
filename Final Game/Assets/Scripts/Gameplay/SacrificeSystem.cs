using Hunger.Managers;
using Hunger.Systems;
using Hunger.UI;
using UnityEngine;

namespace Hunger.Gameplay
{
    public class SacrificeSystem : MonoBehaviour
    {
        public StatSystem statSystem;

        // Stores the currently carried item (player can only carry one at a time)
        private InteractableItem carriedItem = null;

        // Tracks whether the player is inside the barn trigger area
        private bool playerInRange = false;

        // Called when the player picks something up
        public void SetCarriedItem(InteractableItem item)
        {
            // Store the picked up item as the current carried item
            carriedItem = item;
            Debug.Log("Carrying: " + item.gameObject.name);
        }

        private void Update()
        {
            // If the player is not inside the barn trigger, do nothing
            if (!playerInRange) return;

            // If player presses the "1" key while inside the barn
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                // Sacrifice the carried item
                SacrificeCarriedItem();
            }
        }

        void SacrificeCarriedItem()
        {
            // If the player is not carrying anything, stop
            if (carriedItem == null)
            {
                Debug.Log("You are not carrying anything.");
                return;
            }

            // Get the item's tag (category: Home, Self, Family)
            string category = carriedItem.gameObject.tag;

            // Reduce the corresponding survival stat based on the item's category
            statSystem.ReduceStat(category);
            Debug.Log(carriedItem.gameObject.name + " sacrificed!");

            // Clear the carried item (player is no longer holding anything)
            carriedItem = null;

            // Clear the Item text in the UI
            UIManager ui = FindFirstObjectByType<UIManager>();
            ui.ClearItem();

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
                Debug.Log("Press 1 to sacrifice carried item.");
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