using System.Collections;
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
    }

    /// <summary>受け取ったマップに対応する情報を経路探索に渡す</summary>
    /// <param name="map">ステージとなる地形の2次元配列</param>
    /// <exception cref="System.IndexOutOfRangeException"></exception>
    private void ApplyMatching(in int[,] map)
    {
        for (int r = 0; r < _map.GetLength(0); r++)
            for (int c = 0; c < _map.GetLength(1); c++)
            {
                _aStar[r, c] = map[r, c] switch
                {
                    0 => Generate(_wall, r, c),
                    1 => Generate(_path, r, c),
                    _ => throw new System.IndexOutOfRangeException()
                };
            }
    }

    private T Generate<T>(T obj, int row, int column) where T : Object
        => Instantiate(obj, SetCenter(row, column), Quaternion.identity, transform);

    private Vector2 SetCenter(int row, int column)
        => new Vector2(row - _map.GetLength(0) / 2 + 0.5f, column - _map.GetLength(1) / 2 + 0.5f);

    private void ChangeCellColor(Cell cell, Color color)
    {
        if (cell.TryGetComponent(out SpriteRenderer renderer)) renderer.color = color;
    }
}
