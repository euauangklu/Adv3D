using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GDD.FileUtill;
using SolidUtilities.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace GDD
{
    public class CharacterInfoUI : CharacterEditor
    {
        [Header("Character Info")]
        [SerializeField] private TMP_InputField m_characterNameText;

        [Header("Character Show")]
        private SerializableDictionary<string, CanvasComponentList> m_characterShows;
        
        [Header("Character Element Paths")]
        [SerializeField] private ResourcesPathObject _hairPathObject;
        [SerializeField] private ResourcesPathObject _facePathObject;
        [SerializeField] private ResourcesPathObject _skinPathObject;
        [SerializeField] private ResourcesPathObject _TopPathObject;
        [SerializeField] private ResourcesPathObject _BottomPathObject;

        [Header("Gender")] 
        [SerializeField] private Image m_genderIcon;
        [SerializeField] private Sprite m_maleIcon;
        [SerializeField] private Sprite m_femaleIcon;

        protected override void OnEnable()
        {
            base.OnEnable();

            //print(GM.characterInstance.name);
            m_characterNameText.text = GM.characterInstance.name;

            LoadCharacterSave();
        }

        private async void LoadCharacterSave()
        {
            //Hide Character
            _character.gameObject.SetActive(false);
            
            //Show loading
            if(m_loading != null)
                m_loading.SetActive(true);
            
            await Task.Run(() =>
            {
                foreach (var organCharacter in m_characterShows)
                {
                    AM.LoadSingleAssetsWithLabels<CharacterMeshAsset>(GM.characterInstance.characterWardrobe[organCharacter.Key], asset =>
                    {
                        /*Debug.LogWarning($"{asset.Result == null}");*/
                        organCharacter.Value.image[0].sprite = asset.Result.icon;
                        CheckNullImage(organCharacter.Value.image[0]);
                    }, arg0 => { });
                }
            });

            if (GM.characterInstance.gender == m_gender)
                m_genderIcon.sprite = m_maleIcon;
            else
                m_genderIcon.sprite = m_femaleIcon;
            
            //Show Character
            _character.gameObject.SetActive(true);
            
            //Hide Loading
            if(m_loading != null)
                m_loading.SetActive(false);
        }

        private void CheckNullImage(Image image)
        {
            //Debug.LogWarning($"image : {image.sprite == null}");
            image.color = image.sprite == null ? new Color(0, 0, 0, 0) : Color.white;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            
            //Set Color
            foreach (var organCharacter in m_characterShows)
            {
                m_characterShows[organCharacter.Key].image[0].color = new Color(0,0,0,0);
            }
            
            //Set Sprite to null
            foreach (var organCharacter in m_characterShows)
            {
                m_characterShows[organCharacter.Key].image[0].sprite = null;
            }
        }
    }
}