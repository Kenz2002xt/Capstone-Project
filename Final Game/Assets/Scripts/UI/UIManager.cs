using TMPro;
using UnityEngine;
using Hunger.Systems;
using UnityEditor.Rendering;
using System.Collections;
using System.Collections.Generic;
using Hunger.Gameplay;
using Hunger.Data;
using Hunger.Managers;

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
        public RoomItemManager kitchenRoomManager;
        public RoomItemManager sisterRoomManager;
        public RoomItemManager parentRoomManager;
        public RoomItemManager bathroomManager;
        public NarrativeManager narrativeManager;

        // MORNING FLAGS
        private bool visitedParents = false;
        private bool visitedSister = false;
        private bool visitedSelf = false;
        public bool isMorningPhase = false;

        // Sacrifice System
        public GameObject sacrificePanel;
        public Transform sacrificeButtonContainer;
        public GameObject sacrificeButtonPrefab;

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

        public void ResetMorningVisits()
        {
            visitedParents = false;
            visitedSister = false;
            visitedSelf = false;
        }

        public void GoToSisterRoom()
        {
            Debug.Log("Morning Phase: " + isMorningPhase);
            if (isMorningPhase)
            {
                if (visitedSister)
                {
                    ShowDialogue("She’s already gone quiet.");
                    return;
                }

                visitedSister = true;

                StartCoroutine(SisterMorningRoutine());
                optionsPanel.SetActive(false);
                return;
            }

            // NORMAL GAMEPLAY
            if (!explorationSystem.CanExploreRoom())
            {
                ShowDialogue("You are too tired to search another room today.");
                return;
            }

            explorationSystem.RoomExplored();
            cameraSwitcher.GoToSisterRoom();
            sisterRoomManager.GenerateRoomItems();
            optionsPanel.SetActive(false);
        }

        IEnumerator SisterMorningRoutine()
        {
            NarrativeUI ui = FindFirstObjectByType<NarrativeUI>();
            StatSystem stats = FindFirstObjectByType<StatSystem>();
            NarrativeManager nm = FindFirstObjectByType<NarrativeManager>();

            // FIRST EXCHANGE
            FindFirstObjectByType<UIBackgroundController>().SetSister();
            yield return StartCoroutine(ui.ShowChoiceRoutine(
                "She’s sitting on the bed, hugging her knees.\n\n\"It was loud again last night…\"",
                "I heard it too.",
                "It was just the wind.",
                () => stats.familyStat += 5,
                () => stats.familyStat -= 5
            ));

            // SECOND EXCHANGE
            yield return StartCoroutine(ui.ShowChoiceRoutine(
                "\"Do you think it’s gonna stop?\"",
                "Yeah… it will.",
                "I don’t know.",
                () => stats.familyStat += 5,
                () => stats.familyStat -= 5
            ));

            // FINAL LINE
            yield return StartCoroutine(ui.ShowTextRoutine(
                "She nods, but she doesn’t really look convinced."
            ));

            nm.RegisterMorningInteraction();
        }

        public void GoToParentsRoom()
        {
            if (isMorningPhase)
            {
                if (visitedParents)
                {
                    ShowDialogue("They’ve already said what they needed to.");
                    return;
                }

                visitedParents = true;

                StartCoroutine(ParentsMorningRoutine());
                optionsPanel.SetActive(false);
                return;
            }

            // NORMAL GAMEPLAY
            if (!explorationSystem.CanExploreRoom())
            {
                ShowDialogue("You are too tired to search another room today.");
                return;
            }

            cameraSwitcher.GoToParentsRoom();
            explorationSystem.RoomExplored();
            parentRoomManager.GenerateRoomItems();
            optionsPanel.SetActive(false);
        }
        IEnumerator ParentsMorningRoutine()
        {
            NarrativeUI ui = FindFirstObjectByType<NarrativeUI>();
            StatSystem stats = FindFirstObjectByType<StatSystem>();
            NarrativeManager nm = FindFirstObjectByType<NarrativeManager>();

            // FIRST EXCHANGE
            FindFirstObjectByType<UIBackgroundController>().SetParents();
            yield return StartCoroutine(ui.ShowChoiceRoutine(
                "Mom looks tired. \"Did you sleep okay?\"",
                "Yeah… I think so.",
                "No. I kept waking up.",
                () => stats.homeStat += 5,
                () => stats.homeStat -= 5
            ));

            // SECOND EXCHANGE
            yield return StartCoroutine(ui.ShowChoiceRoutine(
                "\"Try to stay warm.\"",
                "I will.",
                "We’re running out of wood.",
                () => stats.homeStat += 5,
                () => stats.homeStat -= 5
            ));

            // FINAL LINE (no choice)
            yield return StartCoroutine(ui.ShowTextRoutine(
                "They don’t say anything after that."
            ));

            nm.RegisterMorningInteraction();
        }

        public void GoToKitchen()
        {
            if (isMorningPhase)
            {
                ShowDialogue("You don’t feel like eating right now.");
                return;
            }

            if (!explorationSystem.CanExploreRoom())
            {
                ShowDialogue("You are too tired to search another room today.");
                return;
            }

            explorationSystem.RoomExplored();
            cameraSwitcher.GoToKitchen();
            kitchenRoomManager.GenerateRoomItems();
            optionsPanel.SetActive(false);
        }

        public void GoToBathroom()
        {
            Debug.Log("Morning Phase: " + isMorningPhase);
            if (isMorningPhase)
            {
                if (visitedSelf)
                {
                    ShowDialogue("You don’t want to look again.");
                    return;
                }

                visitedSelf = true;

                StartCoroutine(SelfMorningRoutine());
                optionsPanel.SetActive(false);
                return;
            }

            // NORMAL GAMEPLAY
            if (!explorationSystem.CanExploreRoom())
            {
                ShowDialogue("You are too tired to search another room today.");
                return;
            }

            explorationSystem.RoomExplored();
            cameraSwitcher.GoToBathroom();
            bathroomManager.GenerateRoomItems();
            optionsPanel.SetActive(false);
        }

        IEnumerator SelfMorningRoutine()
        {
            NarrativeUI ui = FindFirstObjectByType<NarrativeUI>();
            StatSystem stats = FindFirstObjectByType<StatSystem>();
            NarrativeManager nm = FindFirstObjectByType<NarrativeManager>();

            // FIRST EXCHANGE
            FindFirstObjectByType<UIBackgroundController>().SetBathroom();
            yield return StartCoroutine(ui.ShowChoiceRoutine(
                "You stare at yourself in the mirror.\n\nYou look tired.",
                "I’m fine.",
                "I look awful.",
                () => stats.selfStat += 5,
                () => stats.selfStat -= 5
            ));

            // SECOND EXCHANGE
            yield return StartCoroutine(ui.ShowChoiceRoutine(
                "Your hands feel cold.\n\nYou don’t remember when that started.",
                "It’s nothing.",
                "Something’s wrong.",
                () => stats.selfStat += 5,
                () => stats.selfStat -= 5
            ));

            // FINAL LINE
            yield return StartCoroutine(ui.ShowTextRoutine(
                "You look away from the mirror."
            ));

            nm.RegisterMorningInteraction();
        }

        public void LookOutWindow()
        {
            // BLOCK during morning
            if (isMorningPhase)
            {
                ShowDialogue("I should check on everyone first.");
                return;
            }

            cameraSwitcher.LookOutWindow();
            optionsPanel.SetActive(false);

            StartCoroutine(ShowDonRequest());
        }

        IEnumerator ShowDonRequest()
        {
            NarrativeUI ui = FindFirstObjectByType<NarrativeUI>();
            RequestSystem request = FindFirstObjectByType<RequestSystem>();

            yield return StartCoroutine(ui.ShowTextRoutine(
                "Don wants: " + request.currentRequest
            ));

            // This is where your one fade will go later
            
        }

        public void GoToLeaveDoor()
        {
            // BLOCK during morning
            if (isMorningPhase)
            {
                ShowDialogue("I can't go outside yet.");
                return;
            }
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

            //StopAllCoroutines(); 
            StartCoroutine(ClearDialogueAfterTime());
        }

        IEnumerator ClearDialogueAfterTime()
        {
            yield return new WaitForSeconds(3f);

            dialogueText.text = "";
        }

        // --- SACRIFICE FUNCTIONS ---
        public void ShowSacrificeOptions(List<ItemData> items, SacrificeSystem system)
        {
            sacrificePanel.SetActive(true);
            Cursor.visible = true;

            // Clear old buttons
            foreach (Transform child in sacrificeButtonContainer)
            {
                Destroy(child.gameObject);
            }

            // Create new buttons
            foreach (ItemData item in items)
            {
                GameObject btn = Instantiate(sacrificeButtonPrefab, sacrificeButtonContainer);

                btn.GetComponentInChildren<TextMeshProUGUI>().text = item.itemName;

                btn.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
                {
                    system.SacrificeItem(item);
                });
            }
        }

        public void HideSacrificeOptions()
        {
            sacrificePanel.SetActive(false);
        }

        public void UpdateDay(int currentDay)
        {
            dayText.text = "Day: " + currentDay + "/10";
        }

        public void ShowEnd(string result)
        {
            endText.text = result;
        }
    }
}