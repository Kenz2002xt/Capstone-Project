using UnityEngine;
using UnityEngine.UI;
using Hunger.Managers;

namespace Hunger.UI
{
    public class UIBackgroundController : MonoBehaviour
    {
        public Image backgroundImage;

        [Header("Day Backgrounds (Size = 10)")]
        public Sprite[] dayBackgrounds;

        [Header("Other Backgrounds")]
        public Sprite parentsBackground;
        public Sprite sisterBackground;
        public Sprite bathroomBackground;
        public Sprite consequenceBackground;
        public Sprite requestBackground;

        public void SetDay()
        {
            GameManager gm = FindFirstObjectByType<GameManager>();

            if (gm == null || dayBackgrounds.Length == 0)
                return;

            int index = Mathf.Clamp(gm.currentDay - 1, 0, dayBackgrounds.Length - 1);
            SetBackground(dayBackgrounds[index]);
        }

        public void SetParents() => SetBackground(parentsBackground);
        public void SetSister() => SetBackground(sisterBackground);
        public void SetBathroom() => SetBackground(bathroomBackground);
        public void SetConsequence() => SetBackground(consequenceBackground);
        public void SetRequest() => SetBackground(requestBackground);

        void SetBackground(Sprite sprite)
        {
            if (backgroundImage != null && sprite != null)
            {
                backgroundImage.sprite = sprite;
            }
        }
    }
}