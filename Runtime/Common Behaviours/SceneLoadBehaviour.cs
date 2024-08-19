using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace PixelsHub
{
    public sealed class SceneLoadBehaviour : MonoBehaviour
    {
        public static event Action OnNewSceneAllowed;
        public static event Action OnNewSceneLoaded;

        public static bool loadingNextScene { get; private set; }

        [SerializeField]
        private int sceneIndex = 0;


        // For MonoBehaviour as Components
        public void LoadSceneByIndex()
        {
            LoadSceneByIndex(sceneIndex);
        }

        public static void LoadNextScene(float delay = 0)
        {
            if(loadingNextScene) return;

            int index = SceneManager.GetActiveScene().buildIndex + 1;

            LoadSceneByIndex(index, delay);
        }

        public static void LoadPreviousScene(float delay = 0)
        {
            if(loadingNextScene) return;

            int index = SceneManager.GetActiveScene().buildIndex - 1;

            LoadSceneByIndex(index, delay);
        }

        public static bool LoadSceneByIndex(int buildIndex, float delay = 0)
        {
            if(loadingNextScene) return false;
            
            if(buildIndex < 0 || buildIndex >= SceneManager.sceneCountInBuildSettings)
            {
                Debug.Log("Attempted to load a scene with invalid build index: "+buildIndex);
                return false;
            }

            StaticCoroutineRunner.Start(LoadSceneCoroutine(buildIndex, delay));

            return true;
        }

        private static IEnumerator LoadSceneCoroutine(int index, float delay)
        {
            loadingNextScene = true;
            
            yield return new WaitForSecondsRealtime(delay);

            OnNewSceneAllowed?.Invoke();

            yield return new WaitForSecondsRealtime(0.75f); // Default delay

            AsyncOperation op = SceneManager.LoadSceneAsync(index);

            op.allowSceneActivation = false;

            yield return null;

            while(op.progress < 0.9f)
            {
                yield return null;
            }

            op.allowSceneActivation = true;

            while(op.progress < 1)
            {
                yield return null;
            }

            yield return null;

            OnNewSceneLoaded?.Invoke();

            loadingNextScene = false;
        }
    }
}