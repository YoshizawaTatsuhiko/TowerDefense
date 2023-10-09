using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace StageCreator
{
    // 日本語対応
    public class StageEditor : EditorWindow
    {
        [MenuItem("Sample/Stage Create")]
        private static void Open()
        {
            var window = GetWindow<StageEditor>();
            window.titleContent = new GUIContent("TD Stage Designer");
        }

        private void OnGUI()
        {
            GUILayout.Label("Stage");
            GUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("Create"))
                {
                    Debug.Log("Create");
                }
            }
            GUILayout.EndHorizontal();
        }
    }


}