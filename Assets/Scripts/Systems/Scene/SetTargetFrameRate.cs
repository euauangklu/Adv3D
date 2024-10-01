using System;
using UnityEngine;

namespace GDD
{
    public class SetTargetFrameRate : MonoBehaviour
    {
        [SerializeField] private int _frameRate = 60;
        private void Start()
        {
            Application.targetFrameRate = _frameRate;
        }
    }
}