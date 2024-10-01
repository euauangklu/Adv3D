using System;
using UnityEngine;

namespace KongC
{
    public class AutoAssignCamera : MonoBehaviour
    {
        private Canvas _canvas;

        private void Start()
        {
            
        }

        private void OnEnable()
        {
            _canvas ??= GetComponent<Canvas>();
            _canvas.worldCamera = Camera.current;
        }
    }
}