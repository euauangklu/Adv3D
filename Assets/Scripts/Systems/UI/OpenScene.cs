using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GDD
{
    public class OpenScene : MonoBehaviour
    {
        public void OpenSceneIndex(string sceneIndex = "scene index = 0, mode = 0")
        {
            List<string> sceneInfo = sceneIndex.Split(",").ToList();

            LoadSceneMode mode = LoadSceneMode.Single;
            if (sceneInfo.Count > 1)
                mode = (LoadSceneMode)int.Parse(sceneInfo[1]);
                
            SceneManager.LoadScene(int.Parse(sceneInfo[0]), mode);
        }
        
        public void OpenSceneName(string sceneName = "scene name = scene, mode = 0")
        {
            List<string> sceneInfo = sceneName.Split(",").ToList();
            
            LoadSceneMode mode = LoadSceneMode.Single;
            if (sceneInfo.Count > 1)
                mode = (LoadSceneMode)int.Parse(sceneInfo[1]);
            
            SceneManager.LoadScene(sceneInfo[0], mode);
        }
    }
}