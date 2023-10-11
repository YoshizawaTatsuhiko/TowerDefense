using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace StageEditor
{
    // 日本語対応
    public class SampleEditor : EditorWindow
    {
        private string _text = null;

        [MenuItem("Sample/Custum Editor")]
        private static void ShowWindow()
        {
            var editor = GetWindow<SampleEditor>();
            editor.titleContent = new GUIContent("Sample");
        }

        private void OnGUI()
        {
            GUILayout.Label("OUT PUT");
            _text = EditorGUILayout.TextField(_text, GUILayout.Height(100));

            if (GUILayout.Button("Write Down"))
            {
                Debug.Log(_text);
            }
        }
    }
}