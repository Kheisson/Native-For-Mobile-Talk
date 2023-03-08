#if UNITY_ANDROID
using System;
using UnityEngine;

namespace Native
{
    public class AndroidWrapper : INativeWrapper
    {
        #region Constants
        
        //Java Classes
        private const string JAVA_PICK_IMAGE_CLASS_NAME = "com.kson.imagepicker.ImagePicker";
        private const string JAVA_UNITY_CLASS_NAME = "com.unity3d.player.UnityPlayer";
        //Java Methods
        private const string JAVA_SET_CALLBACK_METHOD = "setUnityCallback";
        private const string JAVA_START_IMAGE_PICKER = "startImagePicker";
        private const string JAVA_CURRENT_ACTIVITY_FIELD = "currentActivity";

        #endregion


        #region Static Members
        
        private static AndroidJavaClass _imagePicker;
        private static AndroidJavaObject _unityPlayerActivity;

        #endregion
        
        
        #region Public Methods
        
        public void OpenImagePicker(Action<string> onSuccess, Action<string> onError)
        {
            Debug.Log($"ImagePicker | {nameof(OpenImagePicker)}");
            _imagePicker ??= new AndroidJavaClass(JAVA_PICK_IMAGE_CLASS_NAME);
            _unityPlayerActivity ??= new AndroidJavaClass(JAVA_UNITY_CLASS_NAME).GetStatic<AndroidJavaObject>(JAVA_CURRENT_ACTIVITY_FIELD);
            var callbackToCsharp = new AndroidCallback();
            callbackToCsharp.RegisterCallback(onSuccess, onError);
            _imagePicker.CallStatic(JAVA_SET_CALLBACK_METHOD, callbackToCsharp);
            _imagePicker.CallStatic(JAVA_START_IMAGE_PICKER, _unityPlayerActivity);
        }
        
        #endregion
    }
    
    #region Callback Class
    
    public class AndroidCallback : AndroidJavaProxy
    {
        private const string CALLBACK_INTERFACE_NAME = "com.kson.imagepicker.IUnityCallback";
        private Action<string> _onSuccess;
        private Action<string> _onError;
        public AndroidCallback() : base(CALLBACK_INTERFACE_NAME) { }

        //This callback is called from Native code
        public void onSuccess(string log)
        {
            Debug.Log(log);
            _onSuccess?.Invoke(log);
        }
        
        //This callback is called from Native code
        public void onError(string error)
        {
            Debug.LogError(error);
            _onError?.Invoke(error);
        }
        
        public void RegisterCallback(Action<string> onSuccess, Action<string> onError)
        {
            _onSuccess = onSuccess;
            _onError = onError;
        }
        
    }
    
    #endregion
}
#endif