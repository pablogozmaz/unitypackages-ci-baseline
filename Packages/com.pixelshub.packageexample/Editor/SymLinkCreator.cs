using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
#endif


namespace PixelsHub
{
    public class SymlinkCreator
#if UNITY_EDITOR
        : EditorWindow
#endif
    {
        public struct LinkInfo
        {
            public string newFolderPath;
            public string existingFolderPath;

            public LinkInfo(string newFolderPath, string existingFolderPath)
            {
                this.newFolderPath = newFolderPath; this.existingFolderPath = existingFolderPath;
            }
        }

        public static void CreateLink(LinkInfo info)
        {
#if UNITY_EDITOR
            string strCmdText = "/K";
            strCmdText += " mklink /D \"" + info.newFolderPath + "\" \"" + info.existingFolderPath + "\"";

            var proc1 = new ProcessStartInfo
            {
                UseShellExecute = true,
                WorkingDirectory = @"C:\Windows\System32",
                FileName = @"C:\Windows\System32\cmd.exe",
                Verb = "runas",
                Arguments = strCmdText,
                WindowStyle = ProcessWindowStyle.Normal
            };

            UnityEngine.Debug.Log("Creating symlink from "+info.existingFolderPath+" to "+info.newFolderPath);

            Process.Start(proc1);
#else
            Debug.LogError("Creating a symlink is only available in the editor.");
#endif
        }

        // Popup window

#if UNITY_EDITOR
        private static readonly Vector2 editorWindowSize = new Vector2(600, 128);

        private static string editorExistingFolder = null;
        private static string editorNewFolder = null;

        private static bool pendingMousePosSet;

        [MenuItem("Pixels-Hub/Symlink Creator")]
        private static void Init()
        {
            pendingMousePosSet = true;

            SymlinkCreator window = CreateInstance<SymlinkCreator>();
            Vector2 screenSize = new Vector2(Screen.width * 2, Screen.height * 2);
            window.maxSize = new Vector2(screenSize.x, editorWindowSize.y);
            window.minSize = editorWindowSize;
            window.ShowModalUtility();
        }

        private void OnGUI()
        {
            if(pendingMousePosSet)
            {
                var mousePos = Event.current.mousePosition;
                mousePos.y += minSize.y / 2;
                position = new Rect(mousePos, editorWindowSize);
                pendingMousePosSet = false;
            }

            if(string.IsNullOrEmpty(editorExistingFolder)) editorExistingFolder = Application.dataPath;
            if(string.IsNullOrEmpty(editorNewFolder))      editorNewFolder      = Application.dataPath;

            EditorGUILayout.LabelField("Existing folder path:");
            editorExistingFolder = EditorGUILayout.TextField(editorExistingFolder);

            GUILayout.Space(8);

            EditorGUILayout.LabelField("Target folder path:");
            editorNewFolder = EditorGUILayout.TextArea(editorNewFolder);

            GUILayout.Space(12);

            GUILayout.BeginHorizontal();
            if(GUILayout.Button("Cancel"))
            {
                Close();
            }
            if(GUILayout.Button("Create symlink"))
            {
                LinkInfo info = new LinkInfo(editorNewFolder, editorExistingFolder);
                CreateLink(info);

                Close();
            }
            GUILayout.EndHorizontal();
        }
#endif
    }
}