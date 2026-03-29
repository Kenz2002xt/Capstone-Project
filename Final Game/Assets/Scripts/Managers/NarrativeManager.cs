using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hunger.UI;
using Hunger.Data;
using Hunger.Systems;
using Hunger.Managers;
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

        private int negativeStreak = 0;
        private bool morningFinished = false;
        public bool isMorning = false;
        private int morningInteractions = 0;

        public void StartDayFlow(int day)
        {
            StartCoroutine(RunDay(day));
        }

        IEnumerator RunDay(int day)
        {
            GameEvent e = GetRandomEvent();

            FindFirstObjectByType<UIBackgroundController>().SetDay();
            yield return StartCoroutine(narrativeUI.ShowTextRoutine(
                "December " + day + "...\n\n" + e.description));

            ApplyEvent(e);

            // --- MORNING ---
            morningFinished = false;
            EnableMorning();

            yield return new WaitUntil(() => morningFinished);

            // --- REQUEST ---
            requestSystem.GenerateRequest();
        }

        void EnableMorning()
        {
            isMorning = true;
            morningInteractions = 0;

            UIManager ui = FindFirstObjectByType<UIManager>();
            ui.isMorningPhase = true;
            ui.ResetMorningVisits();
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
            FindFirstObjectByType<UIManager>().isMorningPhase = false;
            isMorning = false;
            morningFinished = true;

            FindFirstObjectByType<UIManager>().ShowDialogue("I should see what Don wants.");
        }

        GameEvent GetRandomEvent()
        {
            List<GameEvent> pool = new List<GameEvent>();

            if (negativeStreak >= 2)
            {
                foreach (var e in events)
                {
                    if (e.isPositive)
                        pool.Add(e);
                }
            }
            else
            {
                pool = events;
            }

            GameEvent chosen = pool[Random.Range(0, pool.Count)];

            if (chosen.isPositive) negativeStreak = 0;
            else negativeStreak++;

            return chosen;
        }

        void ApplyEvent(GameEvent e)
        {
            roomItemManager.itemsToActivate += e.itemModifier;

            if (e.affectedStat == "Home")
                statSystem.homeStat += e.statChange;

            if (e.affectedStat == "Family")
                statSystem.familyStat += e.statChange;

            if (e.affectedStat == "Self")
                statSystem.selfStat += e.statChange;
        }
    }
}