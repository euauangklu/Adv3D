using System;
using UnityEngine;

namespace GDD
{
    public class PreInitialization : MonoBehaviour
    {
        private async void Start()
        {
            await CrashesDataManager.Instance.OnInitialization(null, false);
        }
    }
}