using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using StageCreator;

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
        [SerializeReference] private Cell[,] _cells = null;

        public void Resize2DArray(int width, int height)
        {
            Cell[,] new2DArray = new Cell[width, height];
            int oldWidth = _cells.GetLength(0);
            int oldHeight = _cells.GetLength(1);

            for (int r = 0; r < width; r++)
            {
                for (int c = 0; c < height; c++)
                {
                    new2DArray[r, c] = IsIndexOutOfRange(r, c) ? new(r, c, true) : _cells[r, c];
                }
            }
            _cells = new2DArray;

            bool IsIndexOutOfRange(int r, int c) => r >= oldWidth || c >= oldHeight;
        }
    }
}

[CustomEditor(typeof(Stage))]
public class StageInspectorView : Editor
{
    private SerializedProperty _widthProperty = null;
    private SerializedProperty _heightProperty = null;
    private Stage _stage = null;

    // インスペクターからの操作検知用
    private int _widthCache = 0;
    private int _heightCache = 0;

    private void OnEnable()
    {
        _widthProperty = serializedObject.FindProperty("_width");
        _widthCache = _widthProperty.intValue;
        _heightProperty = serializedObject.FindProperty("_height");
        _heightCache = _heightProperty.intValue;
        _stage = target as Stage;
    }

    public override void OnInspectorGUI()
    {
        _widthProperty.intValue = EditorGUILayout.IntField("Width", _widthProperty.intValue);
        _heightProperty.intValue = EditorGUILayout.IntField("Height", _heightProperty.intValue);

        if (_widthCache != _widthProperty.intValue)
        {
            // 幅が変更されたときの処理を記述する
            _widthCache = _widthProperty.intValue;
            Debug.Log("幅が変更された。");
        }

        if (_heightCache != _heightProperty.intValue)
        {
            // 高さが変更されたときの処理を記述する
            _heightCache = _heightProperty.intValue;
            Debug.Log("高さが変更された。");
        }

        if (GUILayout.Button("Resize"))
        {
            _stage.Resize2DArray(_widthCache, _heightCache);
            Debug.Log("Resizeされた");
        }
    }
}