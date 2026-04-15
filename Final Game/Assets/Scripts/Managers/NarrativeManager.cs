using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hunger.UI;
using Hunger.Data;
using Hunger.Systems;
using Hunger.Gameplay;

namespace Hunger.Managers
{
    public class NarrativeManager : MonoBehaviour
    {
        public NarrativeUI narrativeUI;

        public List<GameEvent> events;

        public StatSystem statSystem;
        public RequestSystem requestSystem;
        public RoomItemManager roomItemManager;
        public GameManager gameManager;

        private bool morningFinished = false;
        public bool isMorning = false;
        private int morningInteractions = 0;

        public bool requestPending = false;

        private GameEvent lastEvent = null;
        private int negativeStreak = 0;
        private string lastAffectedStat = "";
        private int sameStatStreak = 0;

        private List<GameEvent> usedEvents = new List<GameEvent>();

        public void StartDayFlow(int day)
        {
            StartCoroutine(RunDay(day));
        }

        IEnumerator RunDay(int day)
        {
            GameEvent e = GetEventForDay(day);

            UIManager ui = FindFirstObjectByType<UIManager>();

            // SUNLIGHT ON AT START OF DAY
            if (ui != null && ui.sunlight != null)
                ui.sunlight.enabled = true;

            // Hide Day 1 instruction panel on later days
            if (ui != null && day != 1)
            {
                ui.HideDayOneInstruction();
            }

            // Apply event AND get any extra text from item checks
            string extraEventText = ApplyEventAndGetExtraText(e);

            string eventText = "December " + day + "...\n\n" + e.description;

            if (!string.IsNullOrEmpty(e.resultHint))
            {
                eventText += "\n\n" + e.resultHint;
            }

            if (!string.IsNullOrEmpty(extraEventText))
            {
                eventText += "\n\n" + extraEventText;
            }

            FindFirstObjectByType<UIBackgroundController>().SetDay();
            yield return StartCoroutine(narrativeUI.ShowTextRoutine(eventText));

            // --- MORNING ---
            morningFinished = false;
            EnableMorning();

            // Day 1 instruction setup
            if (day == 1 && ui != null)
            {
                ui.ShowDayOneInstruction("Check on your family in their rooms. Check on yourself in the bathroom as well.");
                StartCoroutine(ui.ShowDayOneJournalReminder());
            }

            yield return new WaitUntil(() => morningFinished);

            requestSystem.GenerateRequest();
        }

        void EnableMorning()
        {
            isMorning = true;
            requestPending = false;
            morningInteractions = 0;

            UIManager ui = FindFirstObjectByType<UIManager>();
            if (ui != null)
            {
                ui.isMorningPhase = true;
                ui.ResetMorningVisits();
            }
        }

        public void RegisterMorningInteraction()
        {
            morningInteractions++;

            if (morningInteractions >= 3)
            {
                FinishMorning();
            }
        }

        public void FinishMorning()
        {
            UIManager ui = FindFirstObjectByType<UIManager>();
           
            if (ui != null)
            {
                ui.isMorningPhase = false;
            }

            isMorning = false;
            morningFinished = true;
            requestPending = true;

            if (gameManager.currentDay == 1 && ui != null)
            {
                ui.ShowDayOneInstruction("Head to the window.");
            }
            else if (ui != null)
            {
                ui.ShowDialogue("I should see what Don wants.");
            }
        }

        GameEvent GetEventForDay(int day)
        {
            // Day 1 always uses the special intro/tutorial-style event
            if (day == 1)
            {
                foreach (GameEvent e in events)
                {
                    if (e.dayOneOnly)
                        return e;
                }
            }

            List<GameEvent> pool = new List<GameEvent>();

            foreach (GameEvent e in events)
            {
                if (!e.dayOneOnly && !usedEvents.Contains(e))
                {
                    pool.Add(e);
                }
            }

            // If too many negative events in a row, force a positive one
            if (negativeStreak >= 5)
            {
                List<GameEvent> positivePool = pool.FindAll(e => e.isPositive);

                if (positivePool.Count > 0)
                {
                    pool = positivePool;
                }
            }

            // If too many of the same affected stat in a row, force a different one
            if (sameStatStreak >= 2 && !string.IsNullOrWhiteSpace(lastAffectedStat))
            {
                List<GameEvent> differentStatPool = pool.FindAll(e => e.affectedStat != lastAffectedStat);

                if (differentStatPool.Count > 0)
                {
                    pool = differentStatPool;
                }
            }

            if (pool.Count == 0)
            {
                Debug.LogWarning("No valid unused events left after filtering. Falling back to any unused non-day-one event.");

                foreach (GameEvent e in events)
                {
                    if (!e.dayOneOnly && !usedEvents.Contains(e))
                    {
                        pool.Add(e);
                    }
                }
            }

            if (pool.Count == 0)
            {
                Debug.LogWarning("All events have been used. Falling back to any non-day-one event.");

                foreach (GameEvent e in events)
                {
                    if (!e.dayOneOnly)
                    {
                        pool.Add(e);
                    }
                }
            }

            GameEvent chosen = pool[Random.Range(0, pool.Count)];

            // Track used events so they don't repeat this run
            if (!usedEvents.Contains(chosen))
            {
                usedEvents.Add(chosen);
            }

            // Track positivity streak
            if (chosen.isPositive)
                negativeStreak = 0;
            else
                negativeStreak++;

            // Track same-stat streak
            if (!string.IsNullOrWhiteSpace(chosen.affectedStat) && chosen.affectedStat == lastAffectedStat)
                sameStatStreak++;
            else
                sameStatStreak = 1;

            lastAffectedStat = chosen.affectedStat;
            lastEvent = chosen;

            return chosen;
        }

        string ApplyEventAndGetExtraText(GameEvent e)
        {
            ExplorationSystem exploration = FindFirstObjectByType<ExplorationSystem>();
            string extraText = "";

            // --- NEEDED ITEM CHECK ---
            if (!string.IsNullOrWhiteSpace(e.neededItemName) && exploration != null)
            {
                bool hasNeededItem = true;

                foreach (ItemData consumed in exploration.consumedItems)
                {
                    if (consumed != null && consumed.itemName == e.neededItemName)
                    {
                        hasNeededItem = false;
                        break;
                    }
                }

                if (hasNeededItem)
                {
                    if (!string.IsNullOrEmpty(e.hasItemText))
                    {
                        extraText = e.hasItemText;
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(e.missingItemText))
                    {
                        extraText = e.missingItemText;
                    }

                    // Only apply penalty if item is missing
                    ApplyStatChange(e.affectedStat, e.statChange);
                }
            }
            else
            {
                // Normal event without needed item check
                ApplyStatChange(e.affectedStat, e.statChange);
            }

            // --- RANDOM ITEM LOSS ---
            if (e.randomItemsToRemove > 0)
            {
                RemoveRandomItems(e.randomItemsToRemove);
            }

            // --- BONUS ITEM ---
            if (e.bonusItem != null && !string.IsNullOrWhiteSpace(e.bonusItem.itemName) && exploration != null)
            {
                exploration.discoveredItems.Add(e.bonusItem);
                Debug.Log("Bonus item added: " + e.bonusItem.itemName);
            }

            return extraText;
        }

        void RemoveRandomItems(int count)
        {
            ExplorationSystem exploration = FindFirstObjectByType<ExplorationSystem>();

            if (exploration == null || count <= 0)
                return;

            List<ItemData> removableItems = new List<ItemData>();

            RoomItemManager[] roomManagers = FindObjectsByType<RoomItemManager>(FindObjectsSortMode.None);

            foreach (RoomItemManager room in roomManagers)
            {
                if (room.roomItems == null)
                    continue;

                foreach (GameObject obj in room.roomItems)
                {
                    if (obj == null)
                        continue;

                    InteractableItem interactable = obj.GetComponent<InteractableItem>();

                    if (interactable == null || interactable.item == null)
                        continue;

                    ItemData item = interactable.item;

                    if (!exploration.consumedItems.Contains(item) &&
                        !exploration.discoveredItems.Contains(item) &&
                        !removableItems.Contains(item))
                    {
                        removableItems.Add(item);
                    }
                }
            }

            int removeCount = Mathf.Min(count, removableItems.Count);

            for (int i = 0; i < removeCount; i++)
            {
                int rand = Random.Range(0, removableItems.Count);
                ItemData removed = removableItems[rand];

                exploration.consumedItems.Add(removed);
                removableItems.RemoveAt(rand);

                Debug.Log("Event removed item: " + removed.itemName);
            }
        }

        void ApplyStatChange(string statName, int amount)
        {
            if (string.IsNullOrWhiteSpace(statName) || amount == 0)
                return;

            if (statName == "Home")
            {
                statSystem.homeStat += amount;
            }
            else if (statName == "Family")
            {
                statSystem.familyStat += amount;
            }
            else if (statName == "Self")
            {
                statSystem.selfStat += amount;
            }

            statSystem.homeStat = Mathf.Clamp(statSystem.homeStat, 0, 100);
            statSystem.familyStat = Mathf.Clamp(statSystem.familyStat, 0, 100);
            statSystem.selfStat = Mathf.Clamp(statSystem.selfStat, 0, 100);

            Debug.Log("Event stat change applied: " + statName + " " + amount);
            Debug.Log("Home: " + statSystem.homeStat + " | Family: " + statSystem.familyStat + " | Self: " + statSystem.selfStat);
        }
    }
}