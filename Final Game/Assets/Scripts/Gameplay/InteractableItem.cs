using UnityEngine;
using Hunger.Data;

namespace Hunger.Gameplay
{
    public class InteractableItem : MonoBehaviour
    {
        public ItemData item;

        private ExplorationSystem explorationSystem;

        private void Start()
        {
            explorationSystem = FindFirstObjectByType<ExplorationSystem>();
        }

        private void OnMouseDown()
        {
            explorationSystem.DiscoverItem(item);

            gameObject.SetActive(false);
        }
    }
}