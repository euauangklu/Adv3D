using System;
using StarterAssets;
using UnityEngine;
using UnityEngine.Events;

namespace GDD
{
    public class TouchRaycast : MonoBehaviour
    {
        [SerializeField] private UnityEvent m_OnTouch;
        [SerializeField] protected LayerMask _layerMask;
        [SerializeField] protected AssetsInputs _assetsInputs;
        
        protected bool _isPress;
        
        protected virtual void OnEnable()
        {
            if(_assetsInputs == null)
                _assetsInputs = GetComponent<AssetsInputs>();

            _assetsInputs.onPressScreen += OnPress;
        }

        protected virtual void Start()
        {
            
        }

        protected virtual void Update()
        {
            Ray ray = Camera.main.ScreenPointToRay(_assetsInputs.screenPosition);
            
            bool isHit = Physics.Raycast(ray, out RaycastHit hit, 10f, _layerMask);
            Debug.DrawRay(ray.origin, ray.direction, Color.blue);
            
            if (isHit && _isPress && !PointerOverUIElement.OnPointerOverUIElement())
            {
                print($"Hit Object : {hit.collider.gameObject.name}");
                OnTouch();
                _isPress = false;
            }
        }

        protected virtual void OnTouch()
        {
            m_OnTouch?.Invoke();
        }
        
        protected void OnPress(bool isPress)
        {
            _isPress = isPress;
        }

        protected virtual void OnDisable()
        {
            _assetsInputs.onPressScreen -= OnPress;
        }
    }
}