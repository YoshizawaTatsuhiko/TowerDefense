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

    private void Start()
    {
        _aStar = new AStar(_map.GetLength(0), _map.GetLength(1));
        ApplyMatching(in _map);
        _start = GetRandomPath();
        _goal = GetRandomPath(_start.Row, _start.Column);
        List<Cell> shortestPath = _aStar.FindPath(_start.Row, _start.Column, _goal.Row, _goal.Column);

        if (shortestPath == null) throw new System.NullReferenceException();
        else
        {
            Debug.Log($"Start Position = {_start.Row}:{_start.Column}");
            Debug.Log($"Goal Position = {_goal.Row}:{_goal.Column}");

            foreach (var cell in shortestPath)
            {
                ChangeCellColor(cell, Color.red);
            }
        }
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

    private void ChangeCellColor(Cell cell, Color color)
    {
        if (cell.TryGetComponent(out SpriteRenderer renderer)) renderer.color = color;
    }

    private Cell GetRandomPath(int row = -1, int column = -1)
    {
        int randomR = 0;
        int randomC = 0;

        do
        {
            randomR = Random.Range(0, _map.GetLength(0));
            randomC = Random.Range(0, _map.GetLength(1));
        }
        while (!_aStar[randomR, randomC].IsWalkable && randomR == row && randomC == column);

        return _aStar[randomR, randomC];
    }
}
