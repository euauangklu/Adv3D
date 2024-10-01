using System.Collections;
using System.Collections.Generic;
using GDD;
using Systems.Audio;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class InformationUI : MonoBehaviour
{
    [Header("Ethni Culture")]
    [SerializeField] private AudioPlayerUI m_audioPlayerUI;
    [SerializeField] private List<AudioInformationAssets> m_audioAssets = new List<AudioInformationAssets>();

    [Header("UI")] 
    [SerializeField] private Image m_Icon;
    [SerializeField] private TextMeshProUGUI m_nameCulture;
    [SerializeField] private TextMeshProUGUI m_Dialog;
    [SerializeField] private Image m_collectionImage;
    [SerializeField] private TextMeshProUGUI m_photoTitle;

    public void SelectEthniCulture(int index)
    {
        m_audioPlayerUI.audioClip = m_audioAssets[index].audioClips;
        m_audioPlayerUI.url = m_audioAssets[index].URLs;
        m_Icon.sprite = m_audioAssets[index].images;
        m_Dialog.text = m_audioAssets[index].dialogs;
        m_nameCulture.text = m_audioAssets[index].nameCultures;
        m_collectionImage.sprite = m_audioAssets[index].collectionImage;
        m_photoTitle.text = m_audioAssets[index].nameCultures;
    }
}
