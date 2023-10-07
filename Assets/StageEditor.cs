using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace StageCreator
{
    // 日本語対応
    public class StageEditor : EditorWindow
    {
        [MenuItem("Sample/Create")]
        private static void Open()
        {
            var window = GetWindow<StageEditor>();
            window.titleContent = new GUIContent("Stage Manager");
        }
    }
}