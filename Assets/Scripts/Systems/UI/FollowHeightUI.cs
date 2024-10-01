using System;
using UnityEngine;

namespace GDD
{
    public class FollowHeightUI : MonoBehaviour
    {
        [SerializeField] private RectTransform _Target;
        private RectTransform _rectTransform;

        private void Start()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        private void Update()
        {
            if(_Target == null)
                return;

            _rectTransform.sizeDelta = new Vector2(_rectTransform.sizeDelta.x, _Target.rect.height);
        }
    }
}