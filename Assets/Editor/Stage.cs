using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using StageEditor;

// 日本語対応
namespace StageEditor
{
    [CreateAssetMenu(menuName = "StageObject")]
    public class Stage : ScriptableObject
    {
        public int Width => _width;
        public int Height => _height;
        public Cell[,] Cells { get => _cells; private set => _cells = value; }

        [SerializeField] private int _width = 0;
        [SerializeField] private int _height = 0;
        [HideInInspector]
        [SerializeReference] private Cell[,] _cells = null;

        private void OnEnable()
        {
            _cells = new Cell[_width, _height];
        }

        public void Init2DArray()
        {
            for (int r = 0; r < _width; r++)
            {
                for (int c = 0; c < _height; c++)
                {
                    _cells[r, c] = new Cell(r, c, true);
                }
            }
        }

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
    private int _row = 0;
    private int _column = 0;

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
        _stage.Init2DArray();
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        _widthProperty.intValue = EditorGUILayout.IntField("Width", _widthCache);
        _heightProperty.intValue = EditorGUILayout.IntField("Height", _heightCache);

        int currentWidth = _stage.Cells.GetLength(0);
        int currentHeight = _stage.Cells.GetLength(1);

        _row = EditorGUILayout.IntSlider("Row" ,_row, 0, currentWidth);
        _column = EditorGUILayout.IntSlider("Column", _column, 0, currentHeight);

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

        GUILayout.BeginHorizontal();
        {
            if (GUILayout.Button("Resize"))
            {
                if (_stage.Cells.GetLength(0) == _widthCache || _stage.Cells.GetLength(1) == _heightCache)
                {
                    Debug.Log("There is no need to resize");
                    return;
                }
                _stage.Resize2DArray(_widthCache, _heightCache);
                Debug.Log("Resizeされた");
            }

            if (GUILayout.Button("Check"))
            {
                Debug.Log($"Current row = {_stage.Cells.GetLength(0)}, Current column = {_stage.Cells.GetLength(0)}");
            }
        }
        GUILayout.EndHorizontal();
        Cell currentCell = _stage.Cells[_row, _column];
        currentCell.IsWalkable = EditorGUILayout.Toggle("IsWalkable", currentCell.IsWalkable);
        currentCell.ActualCost = EditorGUILayout.FloatField("ActualCost", currentCell.ActualCost);
        serializedObject.ApplyModifiedProperties();
    }
}