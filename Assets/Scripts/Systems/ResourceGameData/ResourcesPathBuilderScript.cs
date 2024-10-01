using System.Linq;
using GDD.Helper;
using UnityEditor;
using UnityEngine;

namespace GDD
{
    public class ResourcesPathBuilderScript : MonoBehaviour
    {
        [Header("Path Save Resources Object")]
        [SerializeField] private string m_FileName;
        [SerializeField] private string m_ResourcesPathsObject = "/ResourcesPaths/";

        [Header("Path Resources Folder To Create Resources Object")] [SerializeField]
        private string m_FolderPath = "/Resources/Folder To Create Resources Object";
        
        private string _path;
        
        private void OnEnable()
        {
            _path = m_ResourcesPathsObject + m_FileName;
        }
        
        public void BuildResourcesFile()
        {
#if UNITY_EDITOR
            string[,] _paths;
            _paths = ResourcesManager.GetAllResourcePathSeparateFolder(m_FolderPath);

            for (int i = 0; i < _paths.GetLength(0); i++)
            {
                ResourcesPathObject rpobject = ScriptableObject.CreateInstance<ResourcesPathObject>();
                rpobject.paths = ArrayHelper.GetRow(_paths, i).Where(x => !string.IsNullOrEmpty(x)).ToList();

                string f_name = "";
                if (rpobject.paths != null && rpobject.paths.Count > 0)
                {
                    var s_path = rpobject.paths[0].Split("/");
                    if(s_path.Length > 1)
                        f_name = rpobject.paths[0].Split("/")[1];
                    
                    AssetDatabase.CreateAsset(rpobject, "Assets/Resources" + m_ResourcesPathsObject + f_name + m_FileName + ".asset");
                    AssetDatabase.SaveAssets();
                }
            }
#endif
        }
    }
}