using UnityEngine;

namespace GDD
{
    public class OpenURL : MonoBehaviour
    {
        public void OnOpenURL(string url)
        {
            Application.OpenURL(url);
        }
    }
}