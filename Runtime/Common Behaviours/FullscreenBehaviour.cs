using System;
using System.Collections;
using UnityEngine;


namespace PixelsHub
{
    public class FullscreenBehaviour : MonoBehaviour
    {
        public event Action OnChanged;

        public bool IsFullscreen { get; private set; }

        [SerializeField]
        private int windowedWidth = 1200;

        [SerializeField]
        private int windowedHeight = 800;

        [SerializeField]
        private bool allowKeySwitch;


        public void Maximize() 
        {
            if(Screen.fullScreen)
                return;

#if !UNITY_ANDROID && !UNITY_IOS
            SetFullscreen();
#endif
        }

        public void Minimize() 
        {
            if(!Screen.fullScreen)
                return;

            IsFullscreen = false;
            Screen.SetResolution(windowedWidth, windowedHeight, false);
            OnChanged?.Invoke();
        }

        private void Awake()
        {
#if UNITY_ANDROID || UNITY_IOS
            Screen.fullScreen = false;
        }
#else
            SetFullscreen();
        }

        private void SetFullscreen()
        {
            int width = Screen.mainWindowDisplayInfo.width;
            int height = Screen.mainWindowDisplayInfo.height;
            Screen.SetResolution(width, height, true);
            IsFullscreen = true;
            OnChanged?.Invoke();
        }

#if !UNITY_EDITOR
        private void Update()
        {
            if(!allowKeySwitch)
                return;

            if(Input.GetKeyDown(KeyCode.W) && Input.GetKey(KeyCode.LeftControl))
            {
                if(Screen.fullScreen)
                {
                    Minimize();
                }
                else
                {
                    Maximize();
                }
            }
        }
#endif
#endif

    }
}