using UnityEngine;
using UnityEngine.Events;


namespace PixelsHub
{
    public class OnAwakeEvent : MonoBehaviour
    {
        [SerializeField]
        private UnityEvent onAwake;

        private void Awake()
        {
            onAwake.Invoke();
        }
    }
}