using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GDD
{
    public class CloseScene : MonoBehaviour
    {
        public void OpenSceneIndex(int sceneIndex)
        {
            SceneManager.UnloadScene(sceneIndex);
        }
        
        public void OpenSceneName(string sceneName)
        {
            SceneManager.UnloadScene(sceneName);
        }
    }
}