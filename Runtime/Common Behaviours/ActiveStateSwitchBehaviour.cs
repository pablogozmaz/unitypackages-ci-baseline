using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelsHub
{
    public class ActiveStateSwitchBehaviour : MonoBehaviour
    {
        public void Switch() 
        {
            gameObject.SetActive(!gameObject.activeSelf);
        }
    }
}
