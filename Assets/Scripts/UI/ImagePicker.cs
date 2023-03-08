using System;
using System.IO;
using Native;
using UnityEngine;

namespace UI
{
    public class ImagePicker : MonoBehaviour
    {
        
        #region Members

        private readonly INativeWrapper _nativeWrapper = NativeWrapperFactory.GetInstance();
        private string _savedImagePath;

        #endregion


        #region MyRegion

        public event Action<Sprite> OnImagePickedSuccessfully;

        #endregion


        #region Public Methods
        
        public void OpenImagePicker(Action<Sprite> callback)
        {
            OnImagePickedSuccessfully -= callback;
            OnImagePickedSuccessfully += callback;
            _nativeWrapper.OpenImagePicker(OnImagePickedSuccess, OnImagePickedError);
        }
        

        #endregion
        
        
        #region Private Methods

        private void OnImagePickedSuccess(string path)
        {
            Debug.Log($"Image picked: {path}");
            _savedImagePath = path;
        }
        
        private void OnImagePickedError(string log)
        {
            Debug.LogError(log);
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            //Loading image should only happen after Unity's native activity is resumed!
            if (!pauseStatus && IsPathValid(_savedImagePath))
            {
                LoadImage(_savedImagePath);
            }
        }

        private bool IsPathValid(string path)
        {
            return File.Exists(path);
        }
        
        private void LoadImage(string path)
        {
            Debug.Log($"{nameof(ImagePicker)}| {nameof(LoadImage)}");
            var bytes = File.ReadAllBytes(path);
            var texture = new Texture2D(2, 2);
            texture.LoadImage(bytes);
            var sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
            OnImagePickedSuccessfully?.Invoke(sprite);
        }

        #endregion
        
    }
}
