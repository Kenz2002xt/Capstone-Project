using UnityEngine;

namespace Hunger.Data
{
    [System.Serializable]
    public class GameEvent
    {
        [TextArea]
        public string description;

        public int itemModifier; // -1, 0, +1

        public string affectedStat; // "Home", "Family", "Self"
        public int statChange;

        public bool isPositive;
    }
}