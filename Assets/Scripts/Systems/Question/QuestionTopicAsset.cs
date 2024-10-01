using System;
using System.Collections.Generic;
using GDD.Serialize;
using UnityEngine;
using UnityEngine.Serialization;

namespace GDD
{
    [CreateAssetMenu(fileName = "NewQuestionTopicAsset", menuName = "QuestionTopic/Question", order = 0)]
    public class QuestionTopicAsset : ScriptableObjectSerialize
    {
        [Header("Images")]
        [SerializeField] private Sprite m_questionImage;
        [SerializeField] private Sprite m_answerImage;
        
        [Header("Assets")]
        [SerializeField] private CharacterAsset m_shirt;
        [SerializeField] private CharacterAsset m_scarf;
        [SerializeField] private CharacterAsset m_trousers;
        [SerializeField] private CharacterAsset m_hair;

        [Header("Gender")] 
        [SerializeField] private string m_gender;

        [Header("Thai Ethnic Culture")] 
        [SerializeField] private ThaiEthnicCulture m_thaiEthnicCulture;

        //This question answer
        [Header("Answer")]
        [SerializeField] private bool m_isAnswer;

        public bool isAnswer
        {
            get => m_isAnswer;
            set
            {
                m_isAnswer = value;
                OnSave();
            }
        }

        public int Count
        {
            get
            {
                int number = 0;
                number += m_shirt != null ? 1 : 0;
                number += m_trousers != null ? 1 : 0;
                number += m_scarf != null ? 1 : 0;
                number += m_hair != null ? 1 : 0;

                return number;
            }
        }

        public List<CharacterAsset> GetCharacters
        {
            get
            {
                List<CharacterAsset> _characterAssets = new List<CharacterAsset>();
                if(m_shirt != null) _characterAssets.Add(m_shirt);
                if(m_trousers != null) _characterAssets.Add(m_trousers);
                if(m_scarf != null) _characterAssets.Add(m_scarf);
                if(m_hair != null) _characterAssets.Add(m_hair);

                return _characterAssets;
            }
        }
        
        public Sprite questionImage
        {
            get => m_questionImage;
        }
        
        public Sprite answerImage
        {
            get => m_answerImage;
        }

        public CharacterAsset shirt
        {
            get => m_shirt;
        }

        public CharacterAsset scarf
        {
            get => m_scarf;
        }

        public CharacterAsset trousers
        {
            get => m_trousers;
        }

        public CharacterAsset hair
        {
            get => m_hair;
        }

        public string gender
        {
            get => m_gender;
        }

        public ThaiEthnicCulture thaiEthnicCulture
        {
            get => m_thaiEthnicCulture;
        }

        protected override void OnEnable()
        {
            base.OnEnable();
        }

        protected override void OnAssetsLoad(object[] data)
        {
            m_isAnswer = (bool)data[0];
        }

        protected override object[] GetData()
        {
            return new object[]
            {
                m_isAnswer
            };
        } 

        private void OnSave()
        {
            OnSerialize(GetData());
        }

        public override void SaveDefaultValue()
        {
            base.SaveDefaultValue();
            m_isAnswer = false;
            OnSerialize(new object[]{m_isAnswer}); 
        }
    }
}