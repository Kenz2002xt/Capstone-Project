using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hunger.Data;
using Hunger.Managers;
using Hunger.Systems;
using Hunger.UI;

namespace Hunger.Gameplay
{
    public class SacrificeSystem : MonoBehaviour
    {
        public StatSystem statSystem;
        public ExplorationSystem explorationSystem;
        public UIManager uiManager;

        // Called when player reaches the barn
        public void OpenSacrificeMenu()
        {
            List<ItemData> items = explorationSystem.discoveredItems;

            if (items.Count == 0)
            {
                uiManager.ShowDialogue("You have nothing to offer...");
                return;
            }

            uiManager.ShowSacrificeOptions(items, this);
        }

        // Called by UI button when sacrificing an item
        public void SacrificeItem(ItemData item)
        {
            RequestSystem requestSystem = FindFirstObjectByType<RequestSystem>();

            bool matchesRequest = false;

            // Check if any tag matches Don's request
            foreach (string tag in item.requestTags)
            {
                if (tag == requestSystem.currentRequest)
                {
                    matchesRequest = true;
                    break;
                }
            }

            // Calculate value
            int finalValue = item.value + item.sacrificeWeight;

            string matchResultText = "";

            if (!matchesRequest)
            {
                finalValue += 8; // penalty
                matchResultText = "That didn’t seem to help...";
            }
            else
            {
                matchResultText = "Don seems satisfied.";
            }

            // Apply stat change
            statSystem.ReduceStat(item.statTag, finalValue);

            Debug.Log(item.itemName + " sacrificed! Value applied: " + finalValue);

            // Remove from today's discovered list
            explorationSystem.discoveredItems.Remove(item);

            // Permanently consume item
            explorationSystem.consumedItems.Add(item);

            // Close sacrifice UI
            uiManager.HideSacrificeOptions();

            // Get consequence text
            string consequenceText = "";

            if (item.consequenceLines != null && item.consequenceLines.Length > 0)
            {
                consequenceText = item.consequenceLines[
                    Random.Range(0, item.consequenceLines.Length)
                ];
            }
            else
            {
                consequenceText = "You feel something has changed...";
            }

            // APPEND MATCH RESULT AT THE END
            consequenceText += "\n\n" + matchResultText;

            // run coroutine instead of ending day immediately
            StartCoroutine(HandleEndOfDay(consequenceText));
        }

        // controls proper end-of-day flow
        IEnumerator HandleEndOfDay(string consequenceText)
        {
            NarrativeUI narrativeUI = FindFirstObjectByType<NarrativeUI>();

            // Show consequence text and WAIT for it to finish
            yield return StartCoroutine(
               narrativeUI.ShowConsequenceRoutine(consequenceText, true)
            );

            // Now safely move to next day
            FindFirstObjectByType<GameManager>().EndDay();
        }
    }
}