using UnityEngine;


namespace PixelsHub
{
    /// <summary>
    /// Set enabled in a scene to notify of XR behaviour.
    /// Active XR might be checked to change processes in transformations, UI or custom logic. 
    /// </summary>
    public class XR : MonoBehaviour
    {
        public static bool IsActive => activeCount > 0;

        private static int activeCount = 0;


        private void OnEnable()
        {
            activeCount++;
        }

        private void OnDisable()
        {
            activeCount--;
        }
    }
}