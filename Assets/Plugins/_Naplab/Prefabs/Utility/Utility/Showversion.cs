using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Showversion : MonoBehaviour
{
    void Awake(){
        if(TryGetComponent(out TextMeshProUGUI _outputText))
            _outputText.text = Application.version;
    }
}