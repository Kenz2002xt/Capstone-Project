using UnityEngine;

namespace Hunger.Data
{
    [System.Serializable]
    public class GameEvent
    {
        [Header("Intro Text")]
        [TextArea]
        public string description;

        [Header("Optional Follow-Up Text")]
        [TextArea]
        public string resultHint;

        [Header("Item Spawn Change")]
        public int itemModifier; // -1, 0, +1

        [Header("Direct Stat Effect")]
        public string affectedStat; // "Home", "Family", "Self", or empty
        public int statChange;

        [Header("Sacrifice Pressure")]
        public string weightedStat; // "Home", "Family", "Self", or empty
        public int weightModifier;  // example: +3 or +5 extra sacrifice pain

        // BONUS ITEM
        [Header("Bonus Sacrifice Item (Optional)")]
        public ItemData bonusItem;

        [Header("Event Tone")]
        public bool isPositive;
    }
}