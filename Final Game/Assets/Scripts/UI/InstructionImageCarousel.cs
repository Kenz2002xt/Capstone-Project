using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Hunger.UI
{
    public class InstructionImageCarousel : MonoBehaviour
    {
        [Header("UI References")]
        public Image displayImage;
        public Button previousButton;
        public Button nextButton;

        [Header("Images")]
        public Sprite[] instructionImages;

        private int currentIndex = 0;

        void Start()
        {
            UpdateDisplay();
        }

        public void ShowNext()
        {
            if (instructionImages == null || instructionImages.Length == 0)
                return;

            if (currentIndex < instructionImages.Length - 1)
            {
                currentIndex++;
                UpdateDisplay();
            }
        }

        public void ShowPrevious()
        {
            if (instructionImages == null || instructionImages.Length == 0)
                return;

            if (currentIndex > 0)
            {
                currentIndex--;
                UpdateDisplay();
            }
        }

        void UpdateDisplay()
        {
            if (displayImage == null || instructionImages == null || instructionImages.Length == 0)
                return;

            displayImage.sprite = instructionImages[currentIndex];

            UpdateButtonState(previousButton, currentIndex > 0);
            UpdateButtonState(nextButton, currentIndex < instructionImages.Length - 1);
        }

        void UpdateButtonState(Button button, bool isEnabled)
        {
            if (button == null)
                return;

            button.interactable = isEnabled;

            
            TextMeshProUGUI text = button.GetComponentInChildren<TextMeshProUGUI>();
            if (text != null)
            {
                Color c = text.color;
                c.a = isEnabled ? 1f : 0.35f;
                text.color = c;
            }
        }
    }
}