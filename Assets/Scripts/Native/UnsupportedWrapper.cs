using System;
using UnityEngine;

namespace Native
{
    public class UnsupportedWrapper : INativeWrapper
    {
        public void OpenImagePicker(Action<string> onSuccess, Action<string> onError)
        {
            Debug.Log("Unsupported platform");
        }
    }
}