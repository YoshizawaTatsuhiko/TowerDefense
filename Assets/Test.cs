using System.Collections.Generic;
using UnityEngine;

// 日本語対応
public class Test : MonoBehaviour
{
    [SerializeField] private Cell _wall = null;
    [SerializeField] private Cell _path = null;

    private int[,] _map =
    {
        { 0, 1, 1, 1, 0, 1 },
        { 1, 0, 1, 1, 1, 1 },
        { 1, 1, 0, 1, 1, 1 },
        { 1, 1, 1, 0, 1, 1 },
        { 1, 1, 0, 1, 1, 0 },
        { 0, 1, 1, 1, 1, 0 },
    };
    private AStar _aStar = null;
    private Cell _start = null;
    private Cell _goal = null;
    private List<Cell> _pathList = new List<Cell>();
    private List<Cell> _shortestPath = new List<Cell>();

    private void Start()
    {
        _aStar = new AStar(_map.GetLength(0), _map.GetLength(1));
        ApplyMatching(_map);
        _start = GetRandomPath();
        _goal = GetRandomPath(_start);
        _shortestPath = _aStar.FindPath(_start.Row, _start.Column, _goal.Row, _goal.Column);
        PaintShortestPath(_shortestPath);
        ChangeCellColor(_start, Color.yellow);
        ChangeCellColor(_goal, Color.blue);
    }

    /// <summary>受け取ったマップに対応する情報を経路探索に渡す</summary>
    /// <param name="map">ステージとなる地形の2次元配列</param>
    /// <exception cref="System.IndexOutOfRangeException"></exception>
    private void ApplyMatching(in int[,] map)
    {
        Cell cell = null;

        for (int r = 0; r < _map.GetLength(0); r++)
            for (int c = 0; c < _map.GetLength(1); c++)
            {
                cell = map[r, c] switch
                {
                    0 => Generate(_wall, r, c),
                    1 => Generate(_path, r, c),
                    _ => throw new System.IndexOutOfRangeException()
                };
                cell.Row = r;
                cell.Column = c;
                _aStar[r, c] = cell;
                if (cell.IsWalkable) _pathList.Add(cell);
            }
    }

    private T Generate<T>(T obj, int row, int column) where T : MonoBehaviour
    {
        var t = Instantiate(obj, SetCenter(row, column), Quaternion.identity, transform);
        t.gameObject.name += $"({row}, {column})";
        return t;
    }

    private Vector2 SetCenter(int row, int column)
        => new Vector2(column - _map.GetLength(1) / 2 + 0.5f, row - _map.GetLength(0) / 2 + 0.5f);

    private void ChangeCellColor(in Cell cell, Color color)
    {
        if (cell.TryGetComponent(out SpriteRenderer renderer)) renderer.color = color;
    }

    private Cell GetRandomPath(int excludeRow = -1, int excludeColumn = -1)
    {
        int n = 0;

        do
        {
            n = Random.Range(0, _pathList.Count);
        }
        while (_pathList[n].Row == excludeRow && _pathList[n].Column == excludeColumn);

        return _pathList[n];
    }

    private Cell GetRandomPath(in Cell excludeCell)
    {
        int n = 0;

        do
        {
            n = Random.Range(0, _pathList.Count);
        }
        while (_pathList[n] == excludeCell);

        return _pathList[n];
    }

    private void PaintShortestPath(in List<Cell> cellList)
    {
        if (cellList == null) return;

        foreach (var cell in cellList)
        {
            ChangeCellColor(cell, Color.red);
        }
    }
}
