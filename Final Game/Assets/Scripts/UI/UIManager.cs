using TMPro;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Hunger.Gameplay;
using Hunger.Data;
using Hunger.Managers;
using Hunger.Systems;

namespace Hunger.UI
{
    public class UIManager : MonoBehaviour
    {
        [Header("Day 1 Instruction UI")]
        public GameObject dayOneInstructionPanel;
        public TextMeshProUGUI dayOneInstructionText;

        private bool journalOpenedToday = false;
        private bool dayOneRoomHintShown = false;
        private bool dayOneSearchHintShown = false;
        private bool dayOneBarnHintShown = false;

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

        // Morning dialogue data
        public List<MorningDialogueData> parentMorningDialogues;
        public List<MorningDialogueData> sisterMorningDialogues;
        public List<MorningDialogueData> selfMorningDialogues;

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

        // Audi
        public AudioSource morningAudio;
        public AudioSource nightAudio;
        public AudioSource breathingAudio;

        // Lighting
        public Light sunlight;

        void Start()
        {
            optionsPanel.SetActive(false);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            if (sunlight != null)
                sunlight.enabled = true; // START ON

            if (dayOneInstructionPanel != null)
                dayOneInstructionPanel.SetActive(false);

        }

        // JOURNAL BUTTON
        public void OpenJournal()
        {
            optionsPanel.SetActive(true);

            if (IsDayOne())
            {
                journalOpenedToday = true;
            }
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
            morningAudio?.Play();

            if (IsDayOne())
            {
                journalOpenedToday = false;
                dayOneRoomHintShown = false;
                dayOneSearchHintShown = false;
                dayOneBarnHintShown = false;
            }
        }

        bool IsDayOne()
        {
            GameManager gm = FindFirstObjectByType<GameManager>();
            return gm != null && gm.currentDay == 1;
        }

        public void ShowDayOneInstruction(string message)
        {
            if (!IsDayOne())
                return;

            if (dayOneInstructionPanel != null)
                dayOneInstructionPanel.SetActive(true);

            if (dayOneInstructionText != null)
                dayOneInstructionText.text = message;
        }

        public IEnumerator ShowDayOneJournalReminder()
        {
            yield return new WaitForSeconds(6f);

            if (IsDayOne() && isMorningPhase && !journalOpenedToday)
            {
                ShowDayOneInstruction("Click your journal.");
            }
        }

        public void HideDayOneInstruction()
        {
            if (dayOneInstructionPanel != null)
                dayOneInstructionPanel.SetActive(false);
        }

        MorningDialogueData GetDialogueForDay(List<MorningDialogueData> dialogueList)
        {
            int day = FindFirstObjectByType<GameManager>().currentDay - 1;
            day = Mathf.Clamp(day, 0, dialogueList.Count - 1);
            return dialogueList[day];
        }

        IEnumerator PlayMorningDialogue(
            MorningDialogueData data,
            System.Action firstGood,
            System.Action firstBad,
            System.Action secondGood,
            System.Action secondBad)
        {
            NarrativeUI ui = FindFirstObjectByType<NarrativeUI>();

            yield return StartCoroutine(ui.ShowChoiceRoutine(
                data.firstLine,
                data.firstChoiceA,
                data.firstChoiceB,
                firstGood,
                firstBad
            ));

            yield return StartCoroutine(ui.ShowChoiceRoutine(
                data.secondLine,
                data.secondChoiceA,
                data.secondChoiceB,
                secondGood,
                secondBad
            ));

            yield return StartCoroutine(ui.ShowTextRoutine(
                data.finalLine
            ));
        }

        public void GoToSisterRoom()
        {
            Debug.Log("Morning Phase: " + isMorningPhase);

            if (narrativeManager.requestPending)
            {
                ShowDialogue("I should see what Don wants first.");
                return;
            }

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

            if (IsDayOne() && !dayOneSearchHintShown)
            {
                ShowDayOneInstruction("Click a few items to inspect them for sacrifice.");
                dayOneSearchHintShown = true;
            }
        }

        IEnumerator SisterMorningRoutine()
        {
            StatSystem stats = FindFirstObjectByType<StatSystem>();
            NarrativeManager nm = FindFirstObjectByType<NarrativeManager>();

            FindFirstObjectByType<UIBackgroundController>().SetSister();

            MorningDialogueData data = GetDialogueForDay(sisterMorningDialogues);

            yield return StartCoroutine(PlayMorningDialogue(
                data,
                () => stats.familyStat += 0,
                () => stats.familyStat -= 0,
                () => stats.familyStat += 0,
                () => stats.familyStat -= 0
            ));

            nm.RegisterMorningInteraction();
        }

        public void GoToParentsRoom()
        {
            if (narrativeManager.requestPending)
            {
                ShowDialogue("I should see what Don wants first.");
                return;
            }

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

            if (IsDayOne() && !dayOneSearchHintShown)
            {
                ShowDayOneInstruction("Click a few items to inspect them for sacrifice.");
                dayOneSearchHintShown = true;
            }
        }

        IEnumerator ParentsMorningRoutine()
        {
            StatSystem stats = FindFirstObjectByType<StatSystem>();
            NarrativeManager nm = FindFirstObjectByType<NarrativeManager>();

            FindFirstObjectByType<UIBackgroundController>().SetParents();

            MorningDialogueData data = GetDialogueForDay(parentMorningDialogues);

            yield return StartCoroutine(PlayMorningDialogue(
                data,
                () => stats.homeStat += 0,
                () => stats.homeStat -= 0,
                () => stats.homeStat += 0,
                () => stats.homeStat -= 0
            ));

            nm.RegisterMorningInteraction();
        }

        public void GoToKitchen()
        {
            if (narrativeManager.requestPending)
            {
                ShowDialogue("I should see what Don wants first.");
                return;
            }

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

            if (IsDayOne() && !dayOneSearchHintShown)
            {
                ShowDayOneInstruction("Click a few items to inspect them for sacrifice.");
                dayOneSearchHintShown = true;
            }
        }

        public void GoToBathroom()
        {
            Debug.Log("Morning Phase: " + isMorningPhase);

            if (narrativeManager.requestPending)
            {
                ShowDialogue("I should see what Don wants first.");
                return;
            }

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

            if (IsDayOne() && !dayOneSearchHintShown)
            {
                ShowDayOneInstruction("Click a few items to inspect them for sacrifice.");
                dayOneSearchHintShown = true;
            }
        }

        IEnumerator SelfMorningRoutine()
        {
            StatSystem stats = FindFirstObjectByType<StatSystem>();
            NarrativeManager nm = FindFirstObjectByType<NarrativeManager>();

            FindFirstObjectByType<UIBackgroundController>().SetBathroom();

            MorningDialogueData data = GetDialogueForDay(selfMorningDialogues);

            breathingAudio?.Play();

            yield return StartCoroutine(PlayMorningDialogue(
                data,
                () => stats.selfStat += 0,
                () => stats.selfStat -= 0,
                () => stats.selfStat += 0,
                () => stats.selfStat -= 0
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

            morningAudio?.Stop();
            cameraSwitcher.LookOutWindow();
            optionsPanel.SetActive(false);

            // Only show Don's request the first time during request phase
            if (narrativeManager.requestPending)
            {
                StartCoroutine(ShowDonRequest());
            }
        }

        IEnumerator ShowDonRequest()
        {
            NarrativeUI ui = FindFirstObjectByType<NarrativeUI>();
            RequestSystem request = FindFirstObjectByType<RequestSystem>();

            yield return StartCoroutine(ui.ShowTextRoutine(
                "Don wants: " + request.currentRequest
            ));

            nightAudio?.Play();

            narrativeManager.requestPending = false;

            // TURN OFF 
            if (sunlight != null)
                sunlight.enabled = false;

            if (IsDayOne())
            {
                ShowDayOneInstruction("Pick two rooms to search today.");
            }
        }

        public void GoToLeaveDoor()
        {
            if (narrativeManager.requestPending)
            {
                ShowDialogue("I should see what Don wants first.");
                return;
            }

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

        public void UpdateRequest(string request)
        {
            requestText.text = "Don Wants: " + request;
        }

        public void ShowDialogue(string text)
        {
            dialogueText.text = text;
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
            Cursor.lockState = CursorLockMode.None;

            foreach (Transform child in sacrificeButtonContainer)
            {
                Destroy(child.gameObject);
            }

            foreach (ItemData item in items)
            {
                if (item == null || string.IsNullOrWhiteSpace(item.itemName))
                    continue;

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
            nightAudio?.Stop();
        }

        public void ShowEnd(string result)
        {
            endText.text = result;
        }
    }
}