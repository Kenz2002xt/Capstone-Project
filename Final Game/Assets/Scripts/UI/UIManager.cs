using TMPro;
using UnityEngine;
using Hunger.Systems;
using UnityEditor.Rendering;
using System.Collections;
using System.Collections.Generic;
using Hunger.Gameplay;

namespace Hunger.UI
{
    public class UIManager : MonoBehaviour
    {
        // References to UI text elements in the Canvas
        public TextMeshProUGUI requestText;
        public TextMeshProUGUI dialogueText;
        public TextMeshProUGUI dayText;
        public TextMeshProUGUI endText;

        // Journal UI
        public GameObject optionsPanel;
        public GameObject journal;

        // Exploration System
        public ExplorationSystem explorationSystem;

        // Camera system
        public CameraSwitcher cameraSwitcher;

        void Start()
        {
            // Hide the options panel when the game starts
            optionsPanel.SetActive(false);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        // JOURNAL BUTTON
        public void OpenJournal()
        {
            optionsPanel.SetActive(true);
        }

        // CLOSE BUTTON
        public void CloseJournal()
        {
            optionsPanel.SetActive(false);
        }

        // ROOM BUTTONS

        public void GoToSisterRoom()
        {
            if (!explorationSystem.CanExploreRoom())
            {
                ShowDialogue("You are too tired to search another room today.");
                return;
            }

            explorationSystem.RoomExplored();
            cameraSwitcher.GoToSisterRoom();
            optionsPanel.SetActive(false);
        }

        public void GoToParentsRoom()
        {
            if (!explorationSystem.CanExploreRoom())
            {
                ShowDialogue("You are too tired to search another room today.");
                return;
            }

            explorationSystem.RoomExplored();
            cameraSwitcher.GoToParentsRoom();
            optionsPanel.SetActive(false);
        }

        public void GoToKitchen()
        {
            if (!explorationSystem.CanExploreRoom())
            {
                ShowDialogue("You are too tired to search another room today.");
                return;
            }

            explorationSystem.RoomExplored();
            cameraSwitcher.GoToKitchen();
            optionsPanel.SetActive(false);
        }

        public void GoToBathroom()
        {
            if (!explorationSystem.CanExploreRoom())
            {
                ShowDialogue("You are too tired to search another room today.");
                return;
            }

            explorationSystem.RoomExplored();
            cameraSwitcher.GoToBathroom();
            optionsPanel.SetActive(false);
        }

        public void LookOutWindow()
        {
            cameraSwitcher.LookOutWindow();
            optionsPanel.SetActive(false);
        }

        public void GoToLeaveDoor()
        {
            cameraSwitcher.GoToLeaveDoor();
            optionsPanel.SetActive(false);
            journal.SetActive(false);
            Cursor.visible = false;
        }

        // --- UI TEXT FUNCTIONS ---

        // Don's request
        public void UpdateRequest(string request)
        {
            requestText.text = "Don Wants: " + request;
        }

        public void ShowDialogue(string text)
        {
            dialogueText.text = text;

            StopAllCoroutines(); // prevents overlapping timers
            StartCoroutine(ClearDialogueAfterTime());
        }

        IEnumerator ClearDialogueAfterTime()
        {
            yield return new WaitForSeconds(3f);

            dialogueText.text = "";
        }

        public void UpdateDay(int currentDay)
        {
            dayText.text = "Day: " + currentDay + "/3";
        }

        public void ShowEnd(string result)
        {
            endText.text = result;
        }
    }
}