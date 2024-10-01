using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using GDD.Sinagleton;
using Newtonsoft.Json;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace GDD
{
    public class SaveManager : CanDestroy_Sinagleton<SaveManager>
    {
        private UnityAction _onClearData;

        public UnityAction onClearData
        {
            get => _onClearData;
            set => _onClearData = value;
        }

        public T LoadGameObjectData<T>(string location)
        {
            location += ".json";
            string json = "";
            StreamReader sr = new StreamReader(location);
            json = sr.ReadToEnd();
            sr.Close();
            
            var data = JsonConvert.DeserializeObject(json, typeof(T));
            //Debug.LogWarning($"Data {json}");
            return (T)data;
        }

        public void SaveGameObjectData<T>(object save, string location)
        {
            GameManager gm = FindObjectOfType<GameManager>();
            
            location += ".json";
            string json = "";
            json = JsonConvert.SerializeObject((T)save);
            
            StreamWriter sw = new StreamWriter(location);
            sw.Write(json);
            sw.Close();
        }

        public async Task<List<FileInfo>> GetAllFileSavesAsync(string location = default, bool isDefaultPath = false)
        {
            return await Task.Run(() => GetAllFileSaves(location, isDefaultPath));
        }
        
        public List<FileInfo> GetAllFileSaves(string location = default, bool isDefaultPath = false)
        {
            if(isDefaultPath)
                location = Application.persistentDataPath;
            
            var Info = new DirectoryInfo(location);
            var SaveGameInfo = Info.GetFiles("*.json*").OrderByDescending(f => f.LastWriteTime.Year <= 1601 ? f.CreationTime : f.LastWriteTime).ToList();
            
            return SaveGameInfo;
        }
        
        public void DeleteSave(string path, bool addFileJsonExtension = true)
        {
            if(addFileJsonExtension)
                File.Delete(path + ".json");
            else
                File.Delete(path);
            
            print($"Del : {path}");
        }

        public void RenameSave(string path, string oldName, string newName, bool addFileJsonExtension = true)
        {
            FileInfo saveFile;
            if(addFileJsonExtension)
                saveFile = new FileInfo(path + $"/{oldName}" + ".json");
            else
                saveFile = new FileInfo(path + $"/{oldName}");
            
            saveFile.MoveTo(path + $"/{newName}.json");
            
            print($"Rename : {path + $"/{newName}.json"}");
        }
    }
}
