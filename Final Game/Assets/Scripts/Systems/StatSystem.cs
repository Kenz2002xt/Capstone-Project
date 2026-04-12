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

        [Header("Audio")]
        public AudioSource music;

        private float currentPitch = 0.9f;
        private float pitchStep = 0.05f;
        private float minPitch = 0.4f;

        void Start()
        {
            if (directionalLight != null)
            {
                baseColor = directionalLight.color;
                baseIntensity = directionalLight.intensity;
            }

            if (music != null)
            {
                music.pitch = currentPitch;
            }

            UpdateAnimations(); // IMPORTANT: sets idle at game start
        }

        public void ReduceStat(string categoryTag, int amount)
        {
            Debug.Log("ReduceStat called with tag: " + categoryTag + " amount: " + amount);

            if (categoryTag == "Home")
            {
                homeStat -= amount;
                ApplyRedTint();
                ApplyIntense();
                LowerMusicPitch();
            }
            else if (categoryTag == "Self")
            {
                selfStat -= amount;
                ApplyIntense();
                ApplyRedTint();
                LowerMusicPitch();
            }
            else if (categoryTag == "Family")
            {
                familyStat -= amount;
                ApplyIntense();
                ApplyRedTint();
                LowerMusicPitch();
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

        void ApplyRedTint()
        {
            if (directionalLight != null)
                directionalLight.color = Color.Lerp(directionalLight.color, Color.red, 0.1f);
        }

        void ApplyIntense()
        {
            if (directionalLight != null)
                directionalLight.intensity = Mathf.Max(0, directionalLight.intensity + 0.2f);
        }

        void LowerMusicPitch()
        {
            if (music == null)
                return;

            currentPitch -= pitchStep;

            // Clamp so it never goes below 0.4
            currentPitch = Mathf.Max(currentPitch, minPitch);

            music.pitch = currentPitch;

            Debug.Log("Music pitch: " + currentPitch);
        }

        public bool IsDead()
        {
            return homeStat <= 0 || selfStat <= 0 || familyStat <= 0;
        }
    }
}