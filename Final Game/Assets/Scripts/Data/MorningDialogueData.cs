using UnityEngine;

namespace Hunger.Data
{
    [System.Serializable]
    public class MorningDialogueData
    {
        [TextArea(2, 4)]
        public string firstLine;
        public string firstChoiceA;
        public string firstChoiceB;

        [TextArea(2, 4)]
        public string secondLine;
        public string secondChoiceA;
        public string secondChoiceB;

        [TextArea(2, 4)]
        public string finalLine;
    }
}