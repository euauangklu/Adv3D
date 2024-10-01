using System;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace GDD
{
    class MeshFilterControlRig : MonoBehaviour
    {
        [SerializeField] private RigBuilder _rigBuilder;
        [SerializeField] private Rig _rigLayer;
        [SerializeField] private Transform _refLayer;
        [SerializeField] private Transform _mainLayer;
        [SerializeField] private MeshFilter _meshFilter;
        [SerializeField] private MeshRenderer _meshRenderer;

        private RigLayer _currentRigLayer;
        private Vector3 _currentPosition;
        private Vector3 _currentRefPosition;
        private Vector3 _currentOffsetPosition;
        private Vector3 _currentRotation;
        private Vector3 _currentRefRotation;
        private Vector3 _currentOffsetRotation;
        private Vector3 _currentSize;

        public RigBuilder rigBuilder
        {
            get => _rigBuilder;
        }
        
        public MeshFilter meshFilter
        {
            get => _meshFilter;
        }

        public MeshRenderer meshRenderer
        {
            get => _meshRenderer;
        }

        public Vector3 position
        {
            get => _currentPosition;
            set
            {
                _currentPosition = value;
                transform.localPosition = _currentPosition;
            }
        }
        
        public Vector3 refPosition
        {
            get => _currentRefPosition;
            set
            {
                _currentRefPosition = value;
                _refLayer.localPosition = _currentRefPosition;
            }
        }

        public Vector3 offsetPosition
        {
            get => _currentOffsetPosition;
            set
            {
                _currentOffsetPosition = value;
                _mainLayer.localPosition = _currentOffsetPosition;
            }
        }

        public Vector3 rotation
        {
            get => _currentRotation;
            set
            {
                _currentRotation = value;
                transform.localRotation = Quaternion.Euler(_currentRotation);
            }
        }
        
        public Vector3 refRotation
        {
            get => _currentRefRotation;
            set
            {
                _currentRefRotation = value;
                _refLayer.localRotation = Quaternion.Euler(_currentRefRotation);
            }
        }
        
        public Vector3 offsetRotation
        {
            get => _currentOffsetRotation;
            set
            {
                _currentOffsetRotation = value;
                _mainLayer.localRotation = Quaternion.Euler(_currentOffsetRotation);
            }
        }

        public Vector3 size
        {
            get => _currentSize;
            set
            {
                _currentSize = value;
                transform.localScale = _currentSize;
            }
        }
        
        public bool setEnableRig
        {
            get
            {
                _currentRigLayer ??= _rigBuilder.layers.Find(a => a.rig == _rigLayer);
                return _currentRigLayer.active;
            }
            set
            {
                _currentRigLayer ??= _rigBuilder.layers.Find(a => a.rig == _rigLayer);
                _currentRigLayer.active = value;
                _rigBuilder.enabled = true;
            }
        }
        
        private void Start()
        {
            _meshFilter ??= GetComponent<MeshFilter>();
            _meshRenderer ??= GetComponent<MeshRenderer>();
            _refLayer ??= transform.parent;
        }

        public void SetToOffset()
        {
            transform.localPosition = _currentOffsetPosition;
        }

        public void SetToParent()
        {
            transform.localPosition = Vector3.zero;
        }
    }
}