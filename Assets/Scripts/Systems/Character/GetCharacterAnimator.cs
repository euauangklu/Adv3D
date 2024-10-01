using UnityEngine;

namespace GDD
{
    public class GetCharacterAnimator : MonoBehaviour
    {
        [SerializeField] private Animator m_femaleAnimator;
        [SerializeField] private Animator m_maleAnimator;

        public Animator animator
        {
            get
            {
                if (m_femaleAnimator.gameObject.activeSelf)
                    return m_femaleAnimator;
                else
                    return m_maleAnimator;
            }
        }
    }
}