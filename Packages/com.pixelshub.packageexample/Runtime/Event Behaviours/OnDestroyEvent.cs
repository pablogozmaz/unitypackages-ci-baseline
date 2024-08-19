using UnityEngine;
using UnityEngine.Events;


namespace PixelsHub
{
    public class OnDestroyEvent : MonoBehaviour
    {
        [SerializeField]
        private UnityEvent onDestroy;

        private void OnDestroy()
        {
            onDestroy.Invoke();
        }
    }
}