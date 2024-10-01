using System;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class BuildVersionProcessor : IPreprocessBuildWithReport
{
    public int callbackOrder => 0;

    private const string initialVersion = "0";
    private string baseVersion;
    public void OnPreprocessBuild(BuildReport report)
    {
        string currentVersion = FindCurrentVersion();
        UpdateVersion(currentVersion);
    }

    private string FindCurrentVersion()
    {
        string[] currentVersion = PlayerSettings.bundleVersion.Split('.');
        baseVersion = currentVersion[0] + "." + currentVersion[1];
        return currentVersion.Length == 1 ? initialVersion : currentVersion[2];
    }

    private void UpdateVersion(string version){
        
        if(float.TryParse(version, out float versionNumber)){
            float newVersion = versionNumber + 1f;
            //string date = DateTime.Now.ToString("yyddM.HHmmss");

            PlayerSettings.bundleVersion = string.Format("{0}.{1}",baseVersion,newVersion);
            Debug.Log(PlayerSettings.bundleVersion);
        }
        
    }
}
