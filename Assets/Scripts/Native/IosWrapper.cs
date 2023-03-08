using System;
using UnityEngine;

namespace Native
{
    public class IosWrapper : INativeWrapper
    {
        public void OpenImagePicker(Action<string> onSuccess, Action<string> onError)
        {
            Debug.LogError("Not implemented");
        }
    }
}