using System;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

namespace GDD
{
    public class RenderCharacter : MonoBehaviour
    {
        private RenderObjectManager ROM;
        private GameManager GM;
        private PhotoManager PM;
        private Texture _texture;

        private void Start()
        {
            ROM ??= RenderObjectManager.Instance;
            GM ??= GameManager.Instance;
            PM ??= PhotoManager.Instance;
        }

        public void RenderAndSave(Vector2Int renderSize)
        {
            _texture = ROM.Render(renderSize);
            
            PM.SavePhotoCustomPath(PM.ConvertTextureToTexture2D(_texture), GM.saveFileName, Path.Combine(GM.defaultSavePath, "Images"));
        }

        public async Task<Texture2D> LoadImageFromSave(string fileName, Vector2Int renderSize)
        {
            string path = Path.Combine(GM.defaultSavePath, "Images", fileName + ".png");

            if (File.Exists(path))
            {
                Texture2D texture2D = new Texture2D(renderSize.x, renderSize.y);
                byte[] imageData = new byte[] { };
                
                await Task.Run(() =>
                {
                    imageData = File.ReadAllBytes(path);
                });
                
                if(texture2D.LoadImage(imageData))
                    return texture2D;
            }

            return null;
        }

        public void RemoveImageSave(string fileName)
        {
            string path = Path.Combine(GM.defaultSavePath, "Images", fileName + ".png");

            if (File.Exists(path))
                File.Delete(path);
            else 
                Debug.LogWarning($"Can't Delete File : {fileName}");
        }
    }
}