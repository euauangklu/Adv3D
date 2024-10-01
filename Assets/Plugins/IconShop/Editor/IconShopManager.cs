using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace KongC
{
    public class IconShopManager : Editor
    {
        //Setting Data
        public Dictionary<string, GameObject>[] roomPreviewData = new Dictionary<string, GameObject>[]{};
        private Color _CameraBgColor = Color.black;
        private Vector2 _resolution = new Vector2(1080, 1080);
        private Vector2 _previewPosition = new Vector2(0, 0);
        private float _previewZoom = 1;
        private float _previewZoomSpeed = 0.1f;
        private AspectRatio _aspectRatio = AspectRatio.Square;
        private Orientation _orientation = Orientation.Landscape;
        private bool _isShowPreviewInfo = true;
        
        //Class
        private Dictionary<float, AspectRatio> _aspectRatioCheckers;
        public bool isChangeAspectWindow = false;
        
        //Aspect Ratio Landscape Select
        private Dictionary<AspectRatio, string> _aspectRatioLandscape = new Dictionary<AspectRatio, string>()
        {
            { AspectRatio.Square, "1:1" },
            { AspectRatio.ClassicPhoto, "3:2" },
            { AspectRatio.FullScreen, "4:3" },
            { AspectRatio.Widescreen, "16:9" },
            { AspectRatio.Custom, "Custom" }
        };
        
        //Aspect Ratio Portrait Select
        private Dictionary<AspectRatio, string> _aspectRatioPortrait = new Dictionary<AspectRatio, string>()
        {
            { AspectRatio.Square, "1:1" },
            { AspectRatio.ClassicPhoto, "2:3" },
            { AspectRatio.FullScreen, "3:4" },
            { AspectRatio.Widescreen, "9:16" },
            { AspectRatio.Custom, "Custom" }
        };
        
        //Aspect Ratio Value
        private Dictionary<AspectRatio, Vector2> _aspectRatioValues = new Dictionary<AspectRatio, Vector2>()
        {
            { AspectRatio.Square, new Vector2(1,1)},
            { AspectRatio.ClassicPhoto, new Vector2(3,2) },
            { AspectRatio.FullScreen, new Vector2(4,3)},
            { AspectRatio.Widescreen, new Vector2(16,9)},
            { AspectRatio.Custom, new Vector2(0,0)}
        };
        
        //Keys
        private string CamBGColor = "CameraBackgroundColor";

        public Color defaultCameraBGColor
        {
            get => _CameraBgColor;
            set => _CameraBgColor = value;
        }

        public AspectRatio aspectRatio
        {
            get => _aspectRatio;
            set => _aspectRatio = value;
        }

        public Orientation orientation
        {
            get => _orientation;
            set => _orientation = value;
        }

        public Vector2 resolution
        {
            get => _resolution;
        }

        private Vector2 resolutionSet
        {
            set
            {
                if (value.x >= 8 && value.y >= 8)
                    _resolution = value;
            }
        }

        public Dictionary<AspectRatio, string> aspectRatioLandscape
        {
            get => _aspectRatioLandscape;
        }
        
        public Dictionary<AspectRatio, string> aspectRatioPortrait
        {
            get => _aspectRatioPortrait;
        }

        public Vector2 previewPosition
        {
            get => _previewPosition;
            set => _previewPosition = value;
        }

        public float previewZoom
        {
            get => _previewZoom;
            set
            {
                if (value <= 0.1f)
                {
                    _previewZoom = 0.1f;
                }
                else
                {
                    _previewZoom = value;
                }
            }
        }
        
        public float previewZoomSpeed
        {
            get => _previewZoomSpeed;
            set
            {
                if (value <= 0.001f)
                {
                    _previewZoomSpeed = 0.001f;
                }
                else if(value >= 1)
                {
                    _previewZoomSpeed = 1;
                }
                else
                {
                    _previewZoomSpeed = value;
                }
            }
        }

        public bool isShowPreviewInfo
        {
            get => _isShowPreviewInfo;
            set => _isShowPreviewInfo = value;
        }
        
        private static IconShopManager _instance;
    
        public static IconShopManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = CreateInstance<IconShopManager>();
                    Debug.LogError($"Create new instance!");
                }
                return _instance;
            }
        }
        
        private void SaveSettings()
        {
            EditorPrefs.SetString(CamBGColor, JsonUtility.ToJson(_CameraBgColor));
        }

        private void LoadSettings()
        {
            if (EditorPrefs.HasKey(CamBGColor))
            {
                _CameraBgColor = JsonUtility.FromJson<Color>(EditorPrefs.GetString(CamBGColor));
            }
        }

        public Vector2 SetResolution(Vector2 value)
        {
            if (value.x < 8 || value.y < 8)
                return _resolution;
            
            //Auto Set AspectRatio
            _aspectRatioCheckers ??= SetAspectRatioCheckers();

            if (!isChangeAspectWindow)
                if (!_aspectRatioCheckers.TryGetValue(value.x / value.y, out _aspectRatio))
                    _aspectRatio = AspectRatio.Custom;

            //Auto Set Orientation
            _orientation = value.x >= value.y ? Orientation.Landscape : Orientation.Portrait;
            resolutionSet = value;
            return value;
        }

        private Dictionary<float, AspectRatio> SetAspectRatioCheckers()
        {
            Dictionary<float, AspectRatio> value = new Dictionary<float, AspectRatio>();

            for (int i = 0; i < Enum.GetNames(typeof(AspectRatio)).Length; i++)
            {
                AspectRatio aspect = (AspectRatio)i;
                float aspectX = _aspectRatioValues[aspect].x;
                float aspectY = _aspectRatioValues[aspect].y;
                value.Add(aspectX / aspectY, aspect);
                
                if(aspectX != aspectY)
                    value.Add(aspectY / aspectX, aspect);
            }
            return value;
        }

        public AspectRatio SetAspectRatio(AspectRatio value)
        {
            if (value == AspectRatio.Custom)
            {
                isChangeAspectWindow = true;
                _aspectRatio = value;
                return value;
            }

            isChangeAspectWindow = false;

            //Auto Set Resolution
            if (_orientation == Orientation.Landscape)
            {
                resolutionSet = new Vector2(_resolution.x, UtilityPreview.AspectRatio2DToResolution(_aspectRatioValues[value], _resolution, true));
            }
            else
            {
                resolutionSet = new Vector2(UtilityPreview.AspectRatio2DToResolution(new Vector2(_aspectRatioValues[value].y, _aspectRatioValues[value].x), _resolution, false), _resolution.y);
            }

            _aspectRatio = value;
            return value;
        }

        public Orientation SetOrientation(Orientation value)
        {
            //Auto Set Resolution
            resolutionSet = (value == Orientation.Landscape && _resolution.y > _resolution.x) ||
                          (value == Orientation.Portrait && _resolution.y < _resolution.x) ? new Vector2(_resolution.y, _resolution.x) : _resolution;
            
            _orientation = value;
            return value;
        }
    }

    public enum AspectRatio
    {
        Square,
        ClassicPhoto,
        FullScreen,
        Widescreen,
        Custom
    }

    public enum Orientation
    {
        Landscape,
        Portrait
    }
}