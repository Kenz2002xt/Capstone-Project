using UnityEngine;
using Hunger.Data;

namespace Hunger.Gameplay
{
    public class InteractableItem : MonoBehaviour
    {
        public ItemData item;

        private ExplorationSystem explorationSystem;
        private bool discovered = false;

        private void Start()
        {
            explorationSystem = FindFirstObjectByType<ExplorationSystem>();
        }

        private void OnMouseDown()
        {
            if (!discovered)
            {
                explorationSystem.DiscoverItem(item);
                discovered = true;
            }
        }
    }
}