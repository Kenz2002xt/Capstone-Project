using UnityEngine;

namespace Hunger.Systems
{
    public class StatSystem : MonoBehaviour
    {
        // Tracks the survival values for each category
        // Each stat starts at 3 (player can survive 3 sacrifices per category)
        public int homeStat = 100;
        public int selfStat = 100;
        public int familyStat = 100;

        // Reference to the main scene light
        public Light directionalLight;

        private float baseIntensity = 1f;
        private Color baseColor;

        void Start()
        {
            // Store original light color at start
            baseColor = directionalLight.color;
        }

        public void ReduceStat(string categoryTag, int amount)
        {
            if (categoryTag == "Home")
            {
                homeStat -= amount;
                ApplyColdTint();
            }
            else if (categoryTag == "Self")
            {
                selfStat -= amount;
                ApplyDarkness();
            }
            else if (categoryTag == "Family")
            {
                familyStat -= amount;
                ApplyWarmthLoss();
            }

            Debug.Log("Home: " + homeStat);
            Debug.Log("Self: " + selfStat);
            Debug.Log("Family: " + familyStat);
        }

        void ApplyColdTint()
        {
            // Shift light slightly toward blue
            directionalLight.color = Color.Lerp(directionalLight.color, Color.blue, 0.6f);
        }

        void ApplyDarkness()
        {
            // Reduce light intensity
            directionalLight.intensity -= 0.8f;
        }

        void ApplyWarmthLoss()
        {
            // Shift light toward white
            directionalLight.color = Color.Lerp(directionalLight.color, Color.white, 0.8f);
        }

        public bool IsDead()
        {
            // Check if any stat has reached zero or below

            if (homeStat <= 0) return true;
            else if (selfStat <= 0) return true;
            else if (familyStat <= 0) return true;

            // If all stats are above zero, player is still alive
            return false;
        }
    }
}