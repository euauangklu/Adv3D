using GDD.Sinagleton;
using KongC;
using Sirenix.OdinInspector;
using UnityEditor.Rendering;
using UnityEngine;

namespace GDD
{
    public class RenderObjectManager : CanDestroy_Sinagleton<RenderObjectManager>
    {
        //[SerializeField] private RawImage _captureImage;
        [SerializeField] private AnimationClip _defaultPose;
        [SerializeField] private GameObject _gameObject;
        [SerializeField] private float Intensity = 1;
        [SerializeField] private ProjectionType _projection = ProjectionType.Orthographic;
        [SerializeField, ShowIf("_projection", ProjectionType.Perspective)] private float fieldOfView = 60f;
        [SerializeField, ShowIf("_projection", ProjectionType.Orthographic)] private float orthographicSize = 1f;
        [SerializeField] private Vector3 cameraPosition = new Vector3(0, 0.5f, -1.5f);
        private PreviewRoomRuntime _previewRoom;
        private GameObject _objectInScene;
        private GameManager GM;
        private AnimatorOverrideController _animatorOverrideController;

        public enum ProjectionType
        {
            Perspective,
            Orthographic
        }
        
        private void Start()
        {
            GM ??= GameManager.Instance;
            
            if (_previewRoom == null)
            {
                _previewRoom = new PreviewRoomRuntime()
                {
                    cameraBackgroundColor = new Color(0,0,0,0),
                    renderTexture = new RenderTexture(720, 1280, 32),
                    lightIntensity = Intensity,
                    lightRange = 10,
                    cameraPosition = cameraPosition,
                    orthographic = _projection == ProjectionType.Orthographic,
                    fieldOfView = fieldOfView
                };
                
                _previewRoom.OnInitialization(new Color(0,0,0,0));
            }
        }

        [Sirenix.OdinInspector.Button]
        public Texture Render(Vector2Int size)
        {
            //Update Camera
            _previewRoom.SetCameraPosition(cameraPosition);
            _previewRoom.SetLookAt(cameraPosition);
            
            GameObject spawnObject = Instantiate(_gameObject);
            
            //Disable All Component
            Behaviour[] components = spawnObject.GetComponents<Behaviour>();
            foreach (var component in components)
            {
                if (component != null && component is Animator)
                {
                    if (_defaultPose != null)
                    {
                        Animator _animator = (Animator)component;
                        _animatorOverrideController =
                            new AnimatorOverrideController(_animator.runtimeAnimatorController);
                        _animatorOverrideController[_animator.GetCurrentAnimatorClipInfo(0)[0].clip.name] =
                            _defaultPose;
                        _animator.runtimeAnimatorController = _animatorOverrideController;
                        Debug.Log($"Change Animation To = {_animator.runtimeAnimatorController.name}");
                    }
                    else
                    {
                        component.enabled = false;
                    }
                }

                if (component != null && !(component is Transform) && !(component is Animator))
                    component.enabled = false;
            }
            
            _objectInScene = _previewRoom.AddGameObjectInScene(spawnObject, Vector3.zero);
            _objectInScene.transform.rotation = Quaternion.Euler(GM.rotationCharacter);
            
            Destroy(spawnObject);
            
            Texture _texture = _previewRoom.StartPreview(size);
            //_captureImage.texture = _previewRoom.CaptureScene();
            _previewRoom.previewRenderRuntimeUtility.lights[0].intensity = Intensity;
            
            if(_objectInScene != null)
                _previewRoom.Destroy(_objectInScene);

            return _texture;
        }
    }
}