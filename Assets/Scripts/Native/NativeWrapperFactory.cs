using UnityEngine;

namespace Native
{
    public static class NativeWrapperFactory
    {
        #region Public Methods

        public static INativeWrapper GetInstance()
        {
            return Application.platform switch
            {
                RuntimePlatform.Android => new AndroidWrapper(),
                RuntimePlatform.IPhonePlayer => new IosWrapper(),
                _ => new UnsupportedWrapper(),
            };
        }

        #endregion
    }
    
}