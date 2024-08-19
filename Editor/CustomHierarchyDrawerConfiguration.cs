using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelsHub.Editor
{
    [CreateAssetMenu(fileName = ("Custom Hierarchy Drawer"), menuName = ("PixelsHub/Custom Hierarchy Drawer"))]
    public class CustomHierarchyDrawerConfiguration : ScriptableObject
    {
        [SerializeField]
        private bool enabled = true;

        [Space(8)]
        [SerializeField]
        private ColorGroup[] colorGroups = new ColorGroup[0];

        public Dictionary<string, Color> GenerateDictionary()
        {
            if(!enabled) return null;

            var dictionary = new Dictionary<string, Color>(colorGroups.Length);
            
            for(int i = 0; i < colorGroups.Length; i++)
            {
                if(dictionary.ContainsKey(colorGroups[i].code))
                {
                    Debug.LogWarning("Found repeated color codes for custom hierarchy drawer: "+colorGroups[i].code);
                    continue;
                }

                dictionary.Add(colorGroups[i].code, colorGroups[i].color);
            }

            return dictionary;
        }
    }
}