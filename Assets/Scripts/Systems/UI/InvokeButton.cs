using System;
using UnityEngine;
using UnityEngine.UI;

namespace GDD
{
    public class InvokeButton : MonoBehaviour
    {
        [SerializeField] private Button _button;

        public void Invoke()
        {
            print("Invoke BackButton");
            _button.onClick?.Invoke();
        }
    }
}