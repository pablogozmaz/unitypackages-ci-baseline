using UnityEngine;
using UnityEngine.Events;


namespace PixelsHub
{
    public class OnDisableEvent : MonoBehaviour
    {
        [SerializeField]
        private UnityEvent onDisable;

        private void OnDisable()
        {
            onDisable.Invoke();
        }
    }
}