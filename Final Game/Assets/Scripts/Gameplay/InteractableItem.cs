using Hunger.UI;
using UnityEngine;

namespace Hunger.Gameplay
{
    public class InteractableItem : MonoBehaviour
    {
        // Tracks whether the player is close enough to interact with the item
        private bool playerInRange = false;

        private void Update()
        {
            // If the player is near the item AND presses the E key
            if (playerInRange && Input.GetKeyDown(KeyCode.E))
            {
                // Find the SacrificeSystem in the scene
                SacrificeSystem sacrificeSystem = FindFirstObjectByType<SacrificeSystem>();

                // Tell the SacrificeSystem that this item is being carried
                sacrificeSystem.SetCarriedItem(this);

                // Find the UIManager in the scene
                UIManager ui = FindFirstObjectByType<UIManager>();

                // Update the UI to show the name of the picked up item
                ui.UpdateItem(gameObject.name);

                // Disable this object in the scene (simulates picking it up)
                gameObject.SetActive(false);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            // When something enters this item's trigger collider
            if (other.CompareTag("Player"))
            {
                // If it is the player, allow interaction
                playerInRange = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            // When something leaves this item's trigger collider
            if (other.CompareTag("Player"))
            {
                // If it is the player, disable interaction
                playerInRange = false;
            }
        }
    }
}
