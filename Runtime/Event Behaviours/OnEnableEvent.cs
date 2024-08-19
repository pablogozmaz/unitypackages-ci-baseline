using UnityEngine;
using UnityEngine.Events;


namespace PixelsHub
{
    public class OnEnableEvent : MonoBehaviour
    {
        [SerializeField]
        private UnityEvent onEnable;

        private void OnEnable()
        {
            onEnable.Invoke();
        }
    }
}