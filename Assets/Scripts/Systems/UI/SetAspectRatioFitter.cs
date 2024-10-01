using System;
using UnityEngine;
using UnityEngine.UI;

namespace GDD
{
    public class SetAspectRatioFitter : MonoBehaviour
    {
        private Image _image;
        private AspectRatioFitter _aspectRatioFitter;

        private void Awake()
        {
            _image ??= GetComponent<Image>();
            _aspectRatioFitter ??= GetComponent<AspectRatioFitter>();
        }

        private void OnEnable()
        {
            ResetScale();
        }

        public void ResetScale()
        {
            _aspectRatioFitter.aspectRatio = _image.sprite.rect.width / _image.sprite.rect.height;
        }
    }
}