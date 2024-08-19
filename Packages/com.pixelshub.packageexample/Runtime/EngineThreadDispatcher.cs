using System;
using System.Collections.Concurrent;
using UnityEngine;


namespace PixelsHub
{
    /// <summary>
    /// Allows invoking Actions on the next engine thread's Update call.
    /// </summary>
    public class EngineThreadDispatcher
    {
        private class EngineThreadDispatcherBehaviour : MonoBehaviour
        {
            private void Update()
            {
                InvokeAllActions();
            }
        }

        private static readonly ConcurrentQueue<Action> pendingActions = new ConcurrentQueue<Action>();


        public static void AddAction(Action action)
        {
            if(action == null || action.GetInvocationList() == null) return;

            pendingActions.Enqueue(action);
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void CreateBehaviour()
        {
            GameObject go = new GameObject("Engine Thread Dispatcher");
            UnityEngine.Object.DontDestroyOnLoad(go);
            go.AddComponent<EngineThreadDispatcherBehaviour>();
        }

        private static void InvokeAllActions()
        {
            if(pendingActions.Count == 0) return;

            for(int i = 0; i < pendingActions.Count; i++)
            {
                if(pendingActions.TryDequeue(out Action action))
                {
                    try
                    {
                        action.Invoke();
                    }
                    catch(Exception ex)
                    {
                        Debug.LogError(ex.Message + "\n" + ex.StackTrace);
                    }
                }
            }
        }
    }
}