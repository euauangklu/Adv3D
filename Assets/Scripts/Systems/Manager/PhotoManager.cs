using System;
using System.IO;
using GDD.Sinagleton;
using UnityEngine;

namespace GDD
{
    public class PhotoManager : CanDestroy_Sinagleton<PhotoManager>
    {
        public Texture2D LoadPhoto(string path)
        {
            return new Texture2D(100, 100);
        }

        public string SavePhoto(Texture2D image, string name, bool isUseCustomPath = false, string customPath = default)
        {
            byte[] imageByte = image.EncodeToPNG();
            string userFolderPhotoPath;

#if (UNITY_ANDROID || UNITY_IOS)
            NativeGallery.Permission _permission = NativeGallery.SaveImageToGallery(imageByte, "Tai Ethnic Culture", name + ".png");
            
            if(Application.platform == RuntimePlatform.Android)
                return "คลังรูปภาพ";
#endif
            
#if (UNITY_EDITOR || UNITY_EDITOR_WIN)
            if (isUseCustomPath)
                userFolderPhotoPath = customPath;
            else 
                userFolderPhotoPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), "GamePhoto");
            
            print(userFolderPhotoPath);
            string combinePath = userFolderPhotoPath;
            if (!Directory.Exists(combinePath))
            {
                print("Error Folder Not Found");
                Directory.CreateDirectory(combinePath);
            }
            combinePath = Path.Combine(combinePath, name + ".png");
            print(combinePath);
            File.WriteAllBytes(combinePath, imageByte);
            return combinePath;
#endif
            return "";
        }
        
        public string SavePhotoCustomPath(Texture2D image, string name, string path)
        {
            byte[] imageByte = image.EncodeToPNG();
            string userFolderPhotoPath = path;
            
            print(userFolderPhotoPath);
            string combinePath = userFolderPhotoPath;
            if (!Directory.Exists(combinePath))
            {
                print("Error Folder Not Found");
                Directory.CreateDirectory(combinePath);
            }
            combinePath = Path.Combine(combinePath, name + ".png");
            print(combinePath);
            File.WriteAllBytes(combinePath, imageByte);
            return combinePath;
        }

        public Texture2D ConvertCameraRenderTextureToTexture2D(Camera camera)
        {
            RenderTexture _renderTexture = RenderTexture.active;
            RenderTexture.active = camera.targetTexture;
            
            camera.Render();

            print($"Camera is null {camera.targetTexture == null}");
            Texture2D image = new Texture2D(camera.targetTexture.width, camera.targetTexture.height);
            image.ReadPixels(new Rect(0, 0, camera.targetTexture.width, camera.targetTexture.height), 0, 0);
            image.Apply();
            RenderTexture.active = _renderTexture;

            return image;
        }
        
        public Texture2D ConvertRenderTextureToTexture2D(RenderTexture RTexture)
        {
            RenderTexture _renderTexture = RenderTexture.active;
            RenderTexture.active = RTexture;
            
            Texture2D image = new Texture2D(RTexture.width, RTexture.height);
            image.ReadPixels(new Rect(0, 0, RTexture.width, RTexture.height), 0, 0);
            image.Apply();
            RenderTexture.active = _renderTexture;

            return image;
        }
        
        public Texture2D ConvertTextureToTexture2D(Texture texture)
        {
            Texture2D _texture2D = new Texture2D(texture.width, texture.height, TextureFormat.RGBA32, false);
            
            // Set Pixels From Texture To Texture2D
            RenderTexture renderTexture = RenderTexture.GetTemporary(texture.width, texture.height, 0);
            Graphics.Blit(texture, renderTexture);
            RenderTexture.active = renderTexture;
    
            _texture2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
            _texture2D.Apply();

            RenderTexture.active = null;
            RenderTexture.ReleaseTemporary(renderTexture);

            return _texture2D;
        }
    }
}