using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Hunger.UI
{
    public class NarrativeUI : MonoBehaviour
    {
        [Header("Root UI")]
        public GameObject panel;

        [Header("Text")]
        public TextMeshProUGUI textBox;

        [Header("Choice UI")]
        public GameObject choicePanel;
        public Button choiceAButton;
        public Button choiceBButton;
        public TextMeshProUGUI choiceAText;
        public TextMeshProUGUI choiceBText;

        [Header("Timing")]
        public float typeSpeed = 0.02f;
        public float textHoldTime = 2f;

        bool isShowing;

        float defaultFontSize;

        public bool IsShowing() => isShowing;

        void Awake()
        {
            panel.SetActive(false);

            if (textBox != null)
                defaultFontSize = textBox.fontSize;
        }

        // ---------------- TEXT ----------------

        public void SetTextSize(float newSize)
        {
            if (textBox != null)
                textBox.fontSize = newSize;
        }

        public void ResetTextSize()
        {
            if (textBox != null)
                textBox.fontSize = defaultFontSize;
        }

        public IEnumerator ShowTextRoutine(string message, bool instant = false)
        {
            isShowing = true;

            panel.SetActive(true);
            choicePanel.SetActive(false);

            yield return TypeText(message);
            yield return new WaitForSeconds(textHoldTime);

            panel.SetActive(false);
            isShowing = false;
        }

        // ---------------- CHOICE ----------------
        public IEnumerator ShowChoiceRoutine(string text, string A, string B, Action onA, Action onB, bool instant = false)
        {
            isShowing = true;

            panel.SetActive(true);
            choicePanel.SetActive(true);

            choiceAText.text = A;
            choiceBText.text = B;

            yield return TypeText(text);

            bool madeChoice = false;

            choiceAButton.onClick.RemoveAllListeners();
            choiceBButton.onClick.RemoveAllListeners();

            choiceAButton.onClick.AddListener(() =>
            {
                onA?.Invoke();
                madeChoice = true;
            });

            choiceBButton.onClick.AddListener(() =>
            {
                onB?.Invoke();
                madeChoice = true;
            });

            yield return new WaitUntil(() => madeChoice);

            choicePanel.SetActive(false);
            panel.SetActive(false);

            isShowing = false;
        }

        // ---------------- CONSEQUENCE ----------------
        public IEnumerator ShowConsequenceRoutine(string message, bool instant = false)
        {
            isShowing = true;

            panel.SetActive(true);
            choicePanel.SetActive(false);

            yield return TypeText(message);
            yield return new WaitForSeconds(textHoldTime);

            panel.SetActive(false);
            isShowing = false;
        }

        // ---------------- TYPE ----------------
        IEnumerator TypeText(string message)
        {
            textBox.text = "";

            foreach (char c in message)
            {
                textBox.text += c;
                yield return new WaitForSeconds(typeSpeed);
            }
        }
    }
}