using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace GDD
{
    public class PhotoCharacter : MonoBehaviour
    {
        [SerializeField] private UnityEvent m_OnBeginSave;
        [SerializeField] private UnityEvent m_OnEndSave;
        [SerializeField] private Camera _renderCamera;
        [SerializeField] private GameObject _CamNoti;
        private PhotoManager PM;
        private GameManager GM;
        private CanvasComponentList _Noti;

        private void OnEnable()
        {
            
        }

        private void Start()
        {
            PM = PhotoManager.Instance;
            GM = GameManager.Instance;
            _Noti = _CamNoti.GetComponent<CanvasComponentList>();
        }

        public void OnSavePhoto()
        {
            CaptureCameraManager.Instance.SetEnableCamera(true);
            Texture2D photo = PM.ConvertCameraRenderTextureToTexture2D(_renderCamera);
            OnSave(photo);
            CaptureCameraManager.Instance.SetEnableCamera(false);
        }

        public void OnScreenShot()
        {
            StartCoroutine(RecordFrame());
        }

        public void OnSave(Texture2D photo)
        {
            string fileNameDate = DateTime.Now.ToString("yyyyMMddHHmmss");
            string path = PM.SavePhoto(photo, $"{GM.saveFileName}_{fileNameDate}");

            int maxNameCharacter = (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer ? 15 : 7);
            string shortPath = path.Length > 15 ? path.Split(@"\")[0] + @"\..." + path.Substring(path.Length - 15) + @"\name.png" : path;
            string shortName = String.IsNullOrEmpty(GM.saveFileName) ? fileNameDate : GM.saveFileName;
            shortName = shortName.Length > maxNameCharacter ? shortName.Substring(0, maxNameCharacter) + "..." : shortName;
            _Noti.texts[0].text = $"บันทึกรูป {shortName} ไปยัง ({shortPath}) แล้ว";
            _Noti.animators[0].SetTrigger("Play");
        }

        IEnumerator RecordFrame()
        {
            m_OnBeginSave?.Invoke();
            print("Begin Start");
            yield return new WaitForSeconds(1f);
            print($"Wait For Seconds");
            yield return new WaitForEndOfFrame();
            print($"Wait For End Of Frame");
            //Save ScreenShot
            var texture = ScreenCapture.CaptureScreenshotAsTexture();
            m_OnEndSave?.Invoke();
            OnSave(texture);
        }

        private void OnDisable()
        {
            CaptureCameraManager.Instance.SetEnableCamera(false);
        }
    }
}