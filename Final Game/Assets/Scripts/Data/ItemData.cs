using UnityEngine;

namespace Hunger.Data
{
    [System.Serializable]
    public class ItemData
    {
        public string itemName;

        // MULTIPLE possible request matches
        public string[] requestTags;

        // What happens after sacrificing this item
        public string[] consequenceLines;

        // Category used for sacrifice balancing
        public string statTag; // Home, Self, Family

        public int value;
        public int sacrificeWeight = 0;

        // Multiple dialogue lines for replayability
        public string[] descriptions;

        public ItemData(
            string name,
            string[] requests,
            string stat,
            int val,
            string[] desc,
            string[] consequences
        )
        {
            itemName = name;
            requestTags = requests;
            statTag = stat;
            value = val;
            descriptions = desc;
            consequenceLines = consequences;
        }
    }
}