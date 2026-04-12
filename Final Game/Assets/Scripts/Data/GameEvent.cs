using UnityEngine;

namespace Hunger.Data
{
    [System.Serializable]
    public class GameEvent
    {
        [Header("Main Event Text")]
        [TextArea(3, 6)]
        public string description;

        [Header("Optional Extra Feedback")]
        [TextArea(2, 4)]
        public string resultHint;

        [Header("Day Rules")]
        public bool dayOneOnly = false;
        public bool isPositive = false;

        [Header("Direct Category Effect")]
        public string affectedStat;   // "Home", "Family", "Self", or empty
        public int statChange;        // Example: -10 or +10

        [Header("Needed Item Check (Optional)")]
        public string neededItemName; // Example: "Medication"

        [TextArea(2, 4)]
        public string hasItemText;

        [TextArea(2, 4)]
        public string missingItemText;

        [Header("Optional Bonus Sacrifice Item")]
        public ItemData bonusItem;
    }
}