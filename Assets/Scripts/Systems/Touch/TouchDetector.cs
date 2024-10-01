using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;
using UTouch = UnityEngine.InputSystem.Touchscreen;

namespace GDD
{
    public class TouchDetector : MonoBehaviour
    {
        protected Dictionary<int, TouchIdentifier> _touchPool;
        protected int _lastIndex = 0;

        public Camera _Camera;

        private void Awake()
        {
            _Camera ??= Camera.main;

            _Camera = _Camera == null ? Camera.main : _Camera;
        }

        // Use this for initialization
        protected virtual void Start()
        {
            _touchPool = new Dictionary<int, TouchIdentifier>();
        }

        // Update is called once per frame
        protected virtual void Update()
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                TouchControl t = UTouch.current.touches[i];
                switch (t.phase.ReadValue())
                {
                    case TouchPhase.Began:
                        OnTouchBegan(t);
                        break;
                    case TouchPhase.Ended:
                        OnTouchEnded(t);
                        break;
                    case TouchPhase.Moved:
                        OnTouchMoved(t);
                        break;
                    case TouchPhase.Stationary:
                        OnTouchStay(t);
                        break;
                    case TouchPhase.Canceled:
                        OnTouchCancel(t);
                        break;
                }
            }
        }

        public virtual void OnTouchBegan(TouchControl touch)
        {
            //Debug.Log("VAR");
            GetTouchIdentifierWithTouch(touch);
        }

        public virtual void OnTouchEnded(TouchControl touch)
        {
            RemoveTouchIdentifierWithTouch(touch);
        }

        public virtual void OnTouchMoved(TouchControl touch)
        {
            if(_touchPool.TryGetValue(touch.touchId.value, out TouchIdentifier value))
                UpdateTouchIdentifier(value, touch);
        }

        public virtual void OnTouchStay(TouchControl touch)
        {
            if(_touchPool.TryGetValue(touch.touchId.value, out TouchIdentifier value))
                UpdateTouchIdentifier(value, touch);
        }

        public virtual void OnTouchCancel(TouchControl touch)
        {
            RemoveTouchIdentifierWithTouch(touch);
        }

        public Vector3 convertScreenToWorld(Vector3 pos)
        {
            //if (_Camera.orthographic == false) print("Camera is not orthographic");
            pos = _Camera.ScreenToWorldPoint(pos);
            pos.z = 0;
            return pos;
        }

        void UpdateTouchIdentifier(TouchIdentifier touchid, TouchControl touch)
        {
            touchid.currentPosition = convertScreenToWorld(touch.position.value);
            touchid.deltaPosition = touch.delta.value;
        }

        public TouchIdentifier GetTouchIdentifierWithTouch(TouchControl touch)
        {
            //If Finded with fingerID
            if (_touchPool.ContainsKey(touch.touchId.value))
            {
                //Debug.Log($"contains {touch.fingerId}");
                if(_touchPool.TryGetValue(touch.touchId.value, out TouchIdentifier value))
                    UpdateTouchIdentifier(value, touch);
                return _touchPool[touch.touchId.value];
            }

            //Get a new Touch.
            TouchIdentifier t = new TouchIdentifier();
            t.fingerId = touch.touchId.value;
            t.timeCreated = Time.time;
            t.startPosition = touch.position.value;
            t.name = "Touch " + _lastIndex;
            UpdateTouchIdentifier(t, touch);
            _lastIndex++;
            _touchPool.Add(touch.touchId.value, t);
            return t;
        }

        public void RemoveTouchIdentifierWithTouch(TouchControl touch)
        {
            RemoveTouchIdentifierWithTouch(touch, _touchPool);
        }

        public bool RemoveTouchIdentifierWithTouch(TouchControl touch, Dictionary<int, TouchIdentifier> listTouch)
        {
            if (_touchPool.ContainsKey(touch.touchId.value))
            {
                return listTouch.Remove(touch.touchId.value);
            }

            return false;
        }
    }
}