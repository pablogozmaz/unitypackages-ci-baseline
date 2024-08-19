using UnityEngine;

namespace PixelsHub
{
    public class OpenURLBehaviour : MonoBehaviour
    {
        public void DoOpenURL(string url)
        {
            Application.OpenURL(url);
        }
    }
}