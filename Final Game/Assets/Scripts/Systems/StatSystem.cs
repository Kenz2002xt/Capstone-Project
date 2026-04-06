using UnityEngine;

namespace Hunger.Systems
{
    public class StatSystem : MonoBehaviour
    {
        public int homeStat = 100;
        public int selfStat = 100;
        public int familyStat = 100;

        public Light directionalLight;

        private Color baseColor;
        private float baseIntensity;

        [Header("Animation Controllers")]
        public CharacterAnimationController motherController;
        public CharacterAnimationController fatherController;
        public CharacterAnimationController sisterController;

        void Start()
        {
            if (directionalLight != null)
            {
                baseColor = directionalLight.color;
                baseIntensity = directionalLight.intensity;
            }

            UpdateAnimations(); // IMPORTANT: sets idle at game start
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

            ClampStats();
            UpdateAnimations();

            Debug.Log($"Home: {homeStat} | Self: {selfStat} | Family: {familyStat}");
        }

        void ClampStats()
        {
            homeStat = Mathf.Clamp(homeStat, 0, 100);
            selfStat = Mathf.Clamp(selfStat, 0, 100);
            familyStat = Mathf.Clamp(familyStat, 0, 100);
        }

        void UpdateAnimations()
        {
            float lowestStat = Mathf.Min(homeStat, selfStat, familyStat);

            float stateValue;

            if (lowestStat < 50)
                stateValue = 25f;   // LOW
            else if (lowestStat < 85)
                stateValue = 70f;   // MEDIUM
            else
                stateValue = 100f;  // HIGH

            motherController?.UpdateAnimation(stateValue);
            fatherController?.UpdateAnimation(stateValue);
            sisterController?.UpdateAnimation(stateValue);

            Debug.Log("Animation StateLevel set to: " + stateValue);
        }

        void ApplyColdTint()
        {
            if (directionalLight != null)
                directionalLight.color = Color.Lerp(directionalLight.color, Color.blue, 0.6f);
        }

        void ApplyDarkness()
        {
            if (directionalLight != null)
                directionalLight.intensity = Mathf.Max(0, directionalLight.intensity - 0.8f);
        }

        void ApplyWarmthLoss()
        {
            if (directionalLight != null)
                directionalLight.color = Color.Lerp(directionalLight.color, Color.white, 0.8f);
        }

        public bool IsDead()
        {
            return homeStat <= 0 || selfStat <= 0 || familyStat <= 0;
        }
    }
}