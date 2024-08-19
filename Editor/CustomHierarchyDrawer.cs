using System;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace PixelsHub.Editor
{
    [Serializable]
    public struct ColorGroup
    {
        public string code;
        public Color color;
    }

    /// <summary>
    /// Used to extend hierarchy window drawing and allow custom visualization of items based on tag content.
    /// </summary>
    [InitializeOnLoad]
    public class CustomHierarchyDrawer : MonoBehaviour
    {
        private const char codeBegin = '#';
        
        private const float disabledAlpha = 0.8f;
        private const float selectedMultiply = 1.2f;

        private static readonly Color fontColor = new Color(0.15f, 0.15f, 0.15f);
        private static readonly Color hoverColor = new Color(0.234f, 0.49f, 0.9f);
        private static readonly Color colorDisabledSubtract = new Color(0.1f, 0.1f, 0.1f, 0);

        private static readonly Vector2 offset = new Vector2(18, 2);
        
        private static Dictionary<string, Color> colorGroups = new Dictionary<string, Color>();


        static CustomHierarchyDrawer()
        {
            EditorApplication.hierarchyWindowItemOnGUI += HandleHierarchyWindowItemOnGUI;
        }

        private static void HandleHierarchyWindowItemOnGUI(int instanceID, Rect selectionRect)
        {
            colorGroups = GenerateColorGroups();

            if(colorGroups == null || colorGroups.Count == 0) return;

            UnityEngine.Object obj = EditorUtility.InstanceIDToObject(instanceID);

            if(!IsObjectValid(obj)) return;

            bool isEnabled = EditorUtility.GetObjectEnabled(obj) == 1;
            bool isHovered = Event.current != null ? selectionRect.Contains(Event.current.mousePosition) : false;
            bool isSelected = Selection.instanceIDs.Contains(instanceID);

            Vector2 size = selectionRect.size;
            Rect offsetRect = new Rect(selectionRect.position + offset, size);
            Rect fullSelectionRect = new Rect(selectionRect.position, size);


            string code = GetCodeFromName(obj.name);

            if(code == null) return;

            Color newFontColor = fontColor;
            Color backgroundColor = GetColor(code, isSelected, isHovered);

            FontStyle fontStyle = FontStyle.Normal;

            if(!isEnabled)
            {
                newFontColor.a = disabledAlpha;
                backgroundColor -= colorDisabledSubtract;
                fontStyle = FontStyle.Italic;
            }

            EditorGUI.DrawRect(fullSelectionRect, backgroundColor);
            string label = obj.name.Remove(0, code.Length + 1);
            label = label.TrimStart();

            offsetRect.position -= new Vector2(0, 2);
            EditorGUI.LabelField(offsetRect, label, new GUIStyle()
            {
                normal = new GUIStyleState() { textColor = newFontColor },
                fontStyle = fontStyle
            });
        }

        private static Dictionary<string, Color> GenerateColorGroups()
        {
            CustomHierarchyDrawerConfiguration[] configurations = Resources.LoadAll<CustomHierarchyDrawerConfiguration>("");

            if(configurations.Length == 0) return null;

            
            Dictionary<string, Color> colorGroups = configurations[0].GenerateDictionary();

            // Ensure dictionary is not null
            if(colorGroups == null) colorGroups = new Dictionary<string, Color>();

            // Add any subsequent dictionaries from other configurations
            for(int i = 1; i < configurations.Length; i++)
            {
                var dictionary = configurations[i].GenerateDictionary();

                if(dictionary == null) continue;

                foreach(var keypair in dictionary)
                {
                    if(colorGroups.ContainsKey(keypair.Key))
                    {
                        colorGroups[keypair.Key] = keypair.Value;
                    }
                    else colorGroups.Add(keypair.Key, keypair.Value);
                }
            }

            return colorGroups;
        }
        
        private static string GetCodeFromName(string name)
        {
            StringBuilder sb = new StringBuilder();

            for(int i = 0; i < name.Length; i++)
            {
                if(name[i] != codeBegin)
                {
                    sb.Append(name[i]);

                    string result = sb.ToString();
                    if(colorGroups.ContainsKey(result)) return result;
                }
            }

            return null;
        }

        private static Color GetColor(string code, bool isSelected, bool isHovered)
        {
            if(colorGroups.TryGetValue(code, out Color color))
            {
                if(isSelected)
                {
                    color *= selectedMultiply;
                    color.a = 1;
                }
                else if(isHovered) return hoverColor;

                return color;
            }

            return Color.red;
        }

        private static bool IsObjectValid(UnityEngine.Object obj)
        {
            return obj != null && obj.name.Length > 0 && obj.name[0] == codeBegin;
        }
    }
}