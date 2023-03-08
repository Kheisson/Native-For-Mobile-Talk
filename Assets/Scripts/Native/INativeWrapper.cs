using System;

namespace Native
{
    public interface INativeWrapper
    {
        void OpenImagePicker(Action<string> onSuccess, Action<string> onError);
    }
}