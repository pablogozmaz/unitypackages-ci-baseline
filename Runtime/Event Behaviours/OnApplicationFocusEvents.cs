using UnityEngine;
using UnityEngine.Events;


namespace PixelsHub
{
	public class OnApplicationFocusEvents : MonoBehaviour
	{
		[SerializeField]
		private UnityEvent OnApplicationResume;

		[SerializeField]
		private UnityEvent OnApplicationPause;


		private void OnApplicationFocus(bool focus)
		{
			if(focus)
				OnApplicationResume?.Invoke();
			else
				OnApplicationPause?.Invoke();
		}
	}
}