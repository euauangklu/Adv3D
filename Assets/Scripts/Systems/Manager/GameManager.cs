using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GDD.Sinagleton;
using SolidUtilities.Collections;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace GDD
{
    public class GameManager : CanDestroy_Sinagleton<GameManager>
    {
        [Header("Character")] 
        [SerializeField] 
        private SerializableDictionary<string, GameObject> m_character = new SerializableDictionary<string, GameObject>();
        [SerializeField] private string m_gender = "Male";
        
        [Header("Save System")] 
        [SerializeField]
        private string m_defaultSavePath;
        [SerializeField] 
        private Vector2Int m_renderCharacterSize = new Vector2Int(720,1280);
        [SerializeField] private Vector3 m_rotationCharacter = new Vector3(0,180,0);
        
        [SerializeField]
        private CharacterInstance _characterInstance;
        private int _characterSlot;
        private QuestionTopicAsset _currentQuestionTopic;
        private string _fileName;
        private string _oldGender;

        public int characterSlot
        {
            get => _characterSlot;
            set => _characterSlot = value;
        }

        public string gender
        {
            get => m_gender;
        }

        public Vector2Int renderCharacterSize
        {
            get => m_renderCharacterSize;
            set => m_renderCharacterSize = value;
        }

        public Vector3 rotationCharacter
        {
            get => m_rotationCharacter;
        }
        
        public string fileName
        {
            get => _fileName;
        }

        public override void OnAwake()
        {
            base.OnAwake();
            
            if(m_defaultSavePath == "")
                m_defaultSavePath = Application.persistentDataPath;
        }

        public QuestionTopicAsset currentQuestionTopic
        {
            get => _currentQuestionTopic;
            set => _currentQuestionTopic = value;
        }

        private void Start()
        {
            
        }

        public string defaultSavePath
        {
            get => m_defaultSavePath;
        }
        public CharacterInstance characterInstance
        {
            get
            {
                if (_characterInstance == null)
                {
                    string[] listGuid = Guid.NewGuid().ToString().Split("-");
                    _fileName = listGuid[0];
                    _characterInstance = new  CharacterInstance();
                }

                return _characterInstance;
            }

            set => _characterInstance = value;
        }

        public string saveFileName
        {
            get => _fileName;
            set
            {
                _fileName = value;
            }
        }

        public void ResetCharacterInstance()
        {
            string[] listGuid = Guid.NewGuid().ToString().Split("-");
            _characterInstance = new CharacterInstance();
            _fileName = listGuid[0];
            string charName = $"New Character";
            characterInstance.name = charName;
        }

        public GameObject GetCharacter(string key)
        {
            return m_character[key];
        }

        public GameObject TryGetCharacter(string key, out bool hasCharacter)
        {
            GameObject characterObject = m_character[key];
            hasCharacter = characterObject != null;
            return characterObject;
        }

        public void SwitchGender(string key, UnityAction<GameObject> OnSwitchGender = default)
        {
            if(!String.IsNullOrEmpty(_oldGender))
                m_character[_oldGender].SetActive(false);

            m_gender = key;
            if(OnSwitchGender is not null)
                OnSwitchGender?.Invoke(m_character[key]);
            
            m_character[key].SetActive(true);
            _oldGender = key;
        }
        
        /*
        private void OnGUI()
        {
            if(_characterInstance == null)
                return;
                
            GUI.color = Color.black;
            GUI.Label(new Rect(10, 10, 500, 20), $"indexBodyPath : {_characterInstance.skinName}");
            GUI.Label(new Rect(10, 30, 500, 20), $"indexTopPath :  {_characterInstance.faceName}");
            GUI.Label(new Rect(10, 50, 500, 20), $"indexBottomPath :  {_characterInstance.trousersName}");
            GUI.Label(new Rect(10, 70, 500, 20), $"indexHairPath :  {_characterInstance.hairName}");
        }
        */
    }
}