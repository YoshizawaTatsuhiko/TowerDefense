using StageCreator;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// 日本語対応
namespace StageCreator
{
    [CreateAssetMenu(menuName = "StageObject")]
    public class Stage : ScriptableObject
    {
        public int Width => _width;
        public int Height => _height;
        public Cell[,] Cells { get => _cells; set => _cells = value; }

        [SerializeField] private int _width = 0;
        [SerializeField] private int _height = 0;
        [HideInInspector]
        [SerializeField] private Cell[,] _cells = null;

        //private void OnValidate()
        //{
        //    _cells = new Cell[Width, Height];
        //}
    }
}

[CustomEditor(typeof(Stage))]
public class StageInspectorView : Editor
{
    // インスペクターからの操作検知用
    private int _widthCache = 0;
    private int _heightCache = 0;

    private void OnEnable()
    {
        _widthCache = serializedObject.FindProperty("_width").intValue;
        _heightCache = serializedObject.FindProperty("_height").intValue;
    }

    public override void OnInspectorGUI()
    {
        SerializedProperty widthProperty = serializedObject.FindProperty("_width");
        SerializedProperty heightProperty = serializedObject.FindProperty("_height");

        widthProperty.intValue = EditorGUILayout.IntField("Width", widthProperty.intValue);
        heightProperty.intValue = EditorGUILayout.IntField("Height", heightProperty.intValue);

        if (_widthCache != widthProperty.intValue)
        {
            // 幅が変更されたときの処理を記述する
            _widthCache = widthProperty.intValue;
            Debug.Log("幅が変更された。");
            Cell[,] tmpCells = new Cell[_widthCache, _heightCache];
        }

        if (_heightCache != heightProperty.intValue)
        {
            // 高さが変更されたときの処理を記述する
            _heightCache = heightProperty.intValue;
            Debug.Log("高さが変更された。");
        }

        if (GUILayout.Button("RESIZE"))
        {
            Debug.Log("Resizeされた");
        }
    }

    private void Resize2DArray<T>(ref T[,] twoDimArray, int width, int height) where T : new()
    {
        T[,] new2DArray = new T[width, height];
        int oldWidth = twoDimArray.GetLength(0);
        int oldHeight = twoDimArray.GetLength(1);

        for (int r = 0; r < width; r++)
        {
            for (int c = 0; c < height; c++)
            {
                new2DArray[r, c] = IsIndexOutOfRange(r, c) ? new() : twoDimArray[r, c];
            }
        }
        twoDimArray = new2DArray;

        bool IsIndexOutOfRange(int r, int c) => r >= oldWidth || c >= oldHeight;
    }
}