using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace PixelsHub.UI
{
    /// <summary>
    /// Component inheriting from Unity's UI Graphic to allow UI raycasting inside a rect without draw calls.
    /// </summary>
    [RequireComponent(typeof(CanvasRenderer))]
    public class NonDrawingGraphic : Graphic
    {
        public override void SetMaterialDirty() { return; }
        public override void SetVerticesDirty() { return; }
        
        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();
            return;
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(NonDrawingGraphic))]
    public class NonDrawingGraphicEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.HelpBox("This component allows raycasting inside this rect without any draw calls.", MessageType.None);
        }
    }
#endif
}
