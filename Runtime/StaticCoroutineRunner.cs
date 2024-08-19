using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PixelsHub
{
    /// <summary>
    /// Allows to statically start and stop coroutines through a gameobject that is always active.
    /// </summary>
    public static class StaticCoroutineRunner
    {
        // The MonoBehaviour attached to a GameObject that actually runs the coroutines.
        // Should be kept private to disallow any access from outside this class.
        private class CoroutineRunnerComponent : MonoBehaviour
        {
            private void Awake()
            {
                if(runner != null)
                {
                    Destroy(gameObject);
                    Debug.Assert(false);
                    return;
                }

                DontDestroyOnLoad(this);
            }

            private void OnDestroy()
            {
                runner = null;
            }
        }

        private static CoroutineRunnerComponent runner;

        private static HashSet<IEnumerator> pendingCoroutines = new HashSet<IEnumerator>();

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void CreateGameObject()
        {
            if(runner != null) return;

            runner = new GameObject("[Dynamically Instanced] Static Coroutine Runner").AddComponent<CoroutineRunnerComponent>();
            
            // Start all pending coroutines, if any
            foreach(var pendingCoroutine in pendingCoroutines)
            {
                Start(pendingCoroutine);
            }

            pendingCoroutines = null;
        }

        public static void Start(IEnumerator coroutine)
        {
            if(runner == null)
            {
                if(pendingCoroutines != null) pendingCoroutines.Add(coroutine);
                return;
            }

            runner.StartCoroutine(coroutine);
        }

        public static void Stop(IEnumerator coroutine)
        {
            if(runner == null)
            {
                if(pendingCoroutines != null) pendingCoroutines.Remove(coroutine);
                return;
            }

            runner.StopCoroutine(coroutine);
        }
    }
}
