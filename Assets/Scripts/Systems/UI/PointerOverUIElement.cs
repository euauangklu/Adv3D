using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace GDD
{
    public static class PointerOverUIElement
    {
        public static bool OnPointerOverUIElement()
        {
            int UILayer = LayerMask.NameToLayer("UI");
            PointerEventData eventData = new PointerEventData(EventSystem.current);
            
#if UNITY_EDITOR
            eventData.position = Mouse.current.position.value;
#endif        
#if UNITY_ANDROID
            eventData.position = Touchscreen.current.position.value;
#endif
            
            List<RaycastResult> raysastResults = new List<RaycastResult>(1);
            EventSystem.current.RaycastAll(eventData, raysastResults);
            
            //Debug.Log($"RayCastAll : {raysastResults.Count}");
            for (int index = 0; index < raysastResults.Count; index++)
            {
                RaycastResult curRaysastResult = raysastResults[index];
                //Debug.Log(curRaysastResult.gameObject.layer + " : " + UILayer);
                if (curRaysastResult.gameObject.layer == UILayer && curRaysastResult.gameObject.activeSelf)
                {
                    //Debug.Log($"Detect UI : {curRaysastResult.gameObject.name}");
                    return true;
                }
            }
            //Debug.Log("Not Detect UI !!!!!!!!!!!!!");
            return false;
        }
    }
}