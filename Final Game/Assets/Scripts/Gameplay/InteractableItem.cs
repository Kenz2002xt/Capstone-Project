using UnityEngine;
using Hunger.Data;

namespace Hunger.Gameplay
{
    public class InteractableItem : MonoBehaviour
    {
        public ItemData item;

        private ExplorationSystem explorationSystem;
        private RoomItemManager roomManager;

        private bool discovered = false;
        private bool canBeDiscoveredToday = false;

        private Renderer[] childRenderers;
        private MaterialPropertyBlock propBlock;

        private void Start()
        {
            explorationSystem = FindFirstObjectByType<ExplorationSystem>();

            childRenderers = GetComponentsInChildren<Renderer>(true);
            propBlock = new MaterialPropertyBlock();
        }

        public void SetRoomManager(RoomItemManager manager)
        {
            roomManager = manager;
        }

        private void OnMouseDown()
        {
            if (!canBeDiscoveredToday)
                return;

            if (discovered)
                return;

            if (roomManager != null && !roomManager.CanDiscoverMoreItems())
                return;

            explorationSystem.DiscoverItem(item);
            discovered = true;

            if (roomManager != null)
                roomManager.RegisterItemDiscovery();

            SetNormalColor();
        }

        public void SetAvailableForToday(bool available)
        {
            canBeDiscoveredToday = available;

            if (available)
            {
                if (discovered)
                    SetNormalColor();
                else
                    SetBlackColor();
            }
            else
            {
                SetNormalColor();
            }
        }

        public void ResetItem()
        {
            discovered = false;

            if (canBeDiscoveredToday)
                SetBlackColor();
            else
                SetNormalColor();
        }

        private void SetBlackColor()
        {
            if (childRenderers == null)
                return;

            foreach (Renderer rend in childRenderers)
            {
                if (rend == null)
                    continue;

                rend.GetPropertyBlock(propBlock);
                propBlock.SetColor("_BaseColor", Color.black);
                rend.SetPropertyBlock(propBlock);
            }
        }

        private void SetNormalColor()
        {
            if (childRenderers == null)
                return;

            foreach (Renderer rend in childRenderers)
            {
                if (rend == null)
                    continue;

                rend.GetPropertyBlock(propBlock);
                propBlock.SetColor("_BaseColor", Color.white);
                rend.SetPropertyBlock(propBlock);
            }
        }
    }
}