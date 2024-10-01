using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace GDD
{
    public class ChoiceCharacterUI : CharacterEditor
    {
        [Header("UI Elements")] 
        [SerializeField] private TextMeshProUGUI m_CharacterText;
        [SerializeField] private TMP_InputField _characterNameInput;
        [SerializeField] Button m_nextButton;
        
        [Header("Gender Button")] 
        [SerializeField]private Button m_maleButton;
        [SerializeField]private Sprite m_selectMale;
        [SerializeField]private Sprite m_deselectMale;
        [SerializeField]private Button m_femaleButton;
        [SerializeField]private Sprite m_selectFemale;
        [SerializeField]private Sprite m_deselectFemale;

        [Header("Event")] 
        [SerializeField] private UnityEvent m_onReset;
        private string _charName;
        private string _charGender;
        private bool isSetName;

        protected override void OnEnable()
        {
            base.OnEnable();
        }

        private void Start()
        {
            SM = SaveManager.Instance;
            GM = GameManager.Instance;
            
            if(m_CharacterText)
                m_CharacterText.text = GM.characterInstance.name;
            
            m_nextButton.interactable = false;
            
            //OnReset();
        }

        private void Update()
        {
            m_nextButton.interactable = isSetName;
        }

        public void OnChangeName(string name)
        {
            if (!String.IsNullOrEmpty(name) && name != "New Character")
            {
                _charName = name;
                isSetName = true;
            }
            else
            {
                isSetName = false;
            }
        }
        public void OnReset()
        {
            _charGender = m_gender;
            GM.SwitchGender(m_gender);
            _charName = "";
            _characterNameInput.text = "";
            isSetName = false;
            m_onReset?.Invoke();
        }

        public void OnSave()
        {
            /*SM.RenameSave(GM.defaultSavePath, $"Slot {GM.characterSlot + 1}-{GM.characterInstance.name}",
                $"Slot {GM.characterSlot + 1}-{GM.saveFileName}");*/

            //GM.characterInstance.gender = (byte)_charGender;
            GM.characterInstance.name = _charName;
            SM.SaveGameObjectData<CharacterInstance>(GM.characterInstance,
                GM.defaultSavePath + $"/Slot {GM.characterSlot + 1}-{GM.saveFileName}");
        }
    }
}