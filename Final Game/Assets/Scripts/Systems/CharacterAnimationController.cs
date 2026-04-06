using UnityEngine;

namespace Hunger.Systems
{
    public class CharacterAnimationController : MonoBehaviour
    {
        public Animator animator;

        private static readonly int StateLevel = Animator.StringToHash("StateLevel");

        public void UpdateAnimation(float statValue)
        {
            if (animator == null)
            {
                Debug.LogWarning($"{gameObject.name}: Animator is missing.");
                return;
            }

            animator.SetFloat(StateLevel, statValue);

            Debug.Log($"{gameObject.name}: StateLevel = {statValue}, Current State = " +
                      animator.GetCurrentAnimatorStateInfo(0).shortNameHash);
        }
    }
}