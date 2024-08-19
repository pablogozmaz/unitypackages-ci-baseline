using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace PixelsHub
{
    /// <summary>
    /// Component that updates each frame all instances of objects which derive from Updateable System.
    /// <para>
    /// Does not require external initialization, and will always be present in scene.
    /// </para>
    /// <para>
    /// Any new System instance will be automatically added to the static systems cache in order to be updated each frame.
    /// </para>
    /// </summary>
    public class UpdateProcessor : MonoBehaviour
    {
        /// <summary>
        /// Used to identify non-MonoBehaviour Systems that will be updated each frame by the Update Processor.
        /// <para>
        /// Any new System instance will be automatically added to the static systems cache in order to be updated each frame.
        /// </para>
        /// </summary>
        public abstract class System
        {
            public System() { AddSystem(this); }

            public void ClearSystem() { RemoveSystem(this); }

            public abstract void DoUpdate();
        }

        private static readonly List<System> systems = new List<System>();

        private static UpdateProcessor instance;


        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Create() 
        {
            if(instance == null)
            {
                instance = new GameObject("Update Processor").AddComponent<UpdateProcessor>();
                DontDestroyOnLoad(instance.gameObject);
            }
        }

        private static void AddSystem(System system)
        {
            Debug.Assert(!systems.Contains(system));

            systems.Add(system);
        }

        private static bool RemoveSystem(System system)
        {
            return systems.Remove(system);
        }

        private void Awake()
        {
            if(instance != null)
            {
                Debug.Assert(false, "There cannot be more than one instanced Update Processors on scene.");
                DestroyImmediate(this);
            }
        }

        private void Update()
        {
            for(int i = 0; i < systems.Count; i++)
            {
                systems[i].DoUpdate();
            }
        }

        private void OnDestroy()
        {
            instance = null;
        }

#if UNITY_EDITOR
        bool destructionFlag;

        private void OnValidate()
        {
            if(!Application.isPlaying && !destructionFlag)
            {
                Debug.LogError("<color=red>Update Processor is dynamically instanced and will be destroyed, as it should not be added to any gameobject on editor.</color>");

                destructionFlag = true;

                EditorApplication.delayCall += () =>
                {
                    DestroyImmediate(this);
                };
            }
        }
#endif
    }
}