using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// 日本語対応
namespace StageCreator
{
    public class StageLogic
    {
        Cell[,] cells = null;
    }

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