using UnityEngine;


namespace PixelsHub
{
    public class SetActiveBehaviour : MonoBehaviour
    {
        public void Switch()
        {
            gameObject.SetActive(!gameObject.activeInHierarchy);
        }

        public void SetActive(bool active)
        {
            if(gameObject.activeSelf != active)
                gameObject.SetActive(active);
        }
    }
}
