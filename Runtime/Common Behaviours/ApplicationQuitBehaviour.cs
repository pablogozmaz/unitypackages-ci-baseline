using UnityEngine;

namespace PixelsHub
{
    /// <summary>
    /// MonoBehaviour that provides a method for application quit.
    /// </summary>
    public class ApplicationQuitBehaviour : MonoBehaviour
    {
        public void Quit()
        {
            Application.Quit();
        }
    }
}