using UnityEngine;

namespace Hunger.Systems
{
    public class StatSystem : MonoBehaviour
    {
        // Tracks the survival values for each category
        // Each stat starts at 3 (player can survive 3 sacrifices per category)
        public int homeStat = 3;
        public int selfStat = 3;
        public int familyStat = 3;

        // Reference to the main scene light
        public Light directionalLight;

        private float baseIntensity = 1f;
        private Color baseColor;

        void Start()
        {
            // Store original light color at start
            baseColor = directionalLight.color;
        }

        public void ReduceStat(string categoryTag)
        {
            // Checks which category was sacrificed based on the item's tag
            if (categoryTag == "Home")
            {
                // Reduce Home stat by 1
                homeStat--;
                ApplyColdTint();
            }
            else if (categoryTag == "Self")
            {
                // Reduce Self stat by 1
                selfStat--;
                ApplyDarkness();
            }
            else if (categoryTag == "Family")
            {
                // Reduce Family stat by 1
                familyStat--;
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