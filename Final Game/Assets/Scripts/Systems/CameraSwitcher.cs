using UnityEngine;

namespace Hunger.Systems
{
    public class CameraSwitcher : MonoBehaviour
    {
        // Camera locations placed in the scene
        public Transform camStart;
        public Transform camSister;
        public Transform camParents;
        public Transform camKitchen;
        public Transform camWindow;
        public Transform camLeave;
        public Transform camBarn;
        public Transform camBathroom;

        // Reference to the player's movement controller
        public MonoBehaviour playerController;

        // Reference to the player's look/mouse script
        public MonoBehaviour playerLook;

        void MoveCamera(Transform target)
        {
            transform.position = target.position;

            // Match the player's rotation to the camera marker
            transform.eulerAngles = new Vector3(0, target.eulerAngles.y, 0);
        }

        // Start of the day hallway camera
        public void GoToStart()
        {
            playerController.enabled = false;
            playerLook.enabled = false;
            MoveCamera(camStart);
        }

        public void GoToSisterRoom()
        {
            playerController.enabled = false;
            playerLook.enabled = false;
            MoveCamera(camSister);
        }

        public void GoToParentsRoom()
        {
            playerController.enabled = false;
            playerLook.enabled = false;
            MoveCamera(camParents);
        }

        public void GoToKitchen()
        {
            playerController.enabled = false;
            playerLook.enabled = false;
            MoveCamera(camKitchen);
        }

        public void GoToBathroom()
        {
            playerController.enabled = false;
            playerLook.enabled = false;
            MoveCamera(camBathroom);
        }

        public void LookOutWindow()
        {
            playerController.enabled = false;
            playerLook.enabled = false;
            MoveCamera(camWindow);
        }

        // Enables player walking outside
        public void GoToLeaveDoor()
        {
            playerController.enabled = true;
            playerLook.enabled = true;
            MoveCamera(camLeave);
        }

        // Barn sacrifice camera
        public void GoToBarn()
        {
            playerController.enabled = false;
            playerLook.enabled = false;
            MoveCamera(camBarn);
        }
    }
}