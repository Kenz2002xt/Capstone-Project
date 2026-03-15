using UnityEngine;

namespace Hunger.Data
{
    [System.Serializable]
    public class ItemData
    {
        public string itemName;

        // What Don is asking for (Warmth, Comfort, etc)
        public string requestTag;

        // Category used for sacrifice balancing
        public string statTag; // Home, Self, Family

        public int value;

        // Multiple dialogue lines for replayability
        public string[] descriptions;

        public ItemData(string name, string request, string stat, int val, string[] desc)
        {
            itemName = name;
            requestTag = request;
            statTag = stat;
            value = val;
            descriptions = desc;
        }
    }
}