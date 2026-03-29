using UnityEngine;
using UnityEngine.UI;

namespace Hunger.UI
{
    public class UIBackgroundController : MonoBehaviour
    {
        public Image backgroundImage;

        [Header("Backgrounds")]
        public Sprite dayBackground;
        public Sprite parentsBackground;
        public Sprite sisterBackground;
        public Sprite bathroomBackground;
        public Sprite consequenceBackground;

        public void SetDay() => SetBackground(dayBackground);
        public void SetParents() => SetBackground(parentsBackground);
        public void SetSister() => SetBackground(sisterBackground);
        public void SetBathroom() => SetBackground(bathroomBackground);
        public void SetConsequence() => SetBackground(consequenceBackground);

        void SetBackground(Sprite sprite)
        {
            backgroundImage.sprite = sprite;
        }
    }
}