using UnityEngine;
using Hunger.Systems;

namespace Hunger.Gameplay
{
    public class BarnTrigger : MonoBehaviour
    {
        public CameraSwitcher cameraSwitcher;
        public SacrificeSystem sacrificeSystem;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                // Snap to barn camera
                cameraSwitcher.GoToBarn();

                // Open sacrifice UI
                sacrificeSystem.OpenSacrificeMenu();
            }
        }
    }
}
