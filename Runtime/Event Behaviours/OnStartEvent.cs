using UnityEngine;
using UnityEngine.Events;


namespace PixelsHub
{
    public class OnStartEvent : MonoBehaviour
    {
        [SerializeField]
        private UnityEvent onStart;

        private void Start()
        {
            onStart.Invoke();
        }
    }
}