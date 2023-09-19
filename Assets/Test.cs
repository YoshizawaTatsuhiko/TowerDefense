using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 日本語対応
public class Test : MonoBehaviour
{
    [SerializeField] private int _width = 10;
    [SerializeField] private int _height = 10;
    [SerializeField] private Cell _wall = null;
    [SerializeField] private Cell _path = null;

    private AStar _aStar = null;

    private void Start()
    {
        HoleDigging algo = new HoleDigging(_width, _height);
        int[,] map = algo.Get2DArray();
        _aStar = new AStar(_width, _height);
        ApplyMatching(in map);
    }

    /// <summary>受け取ったマップに対応する情報を経路探索に渡す</summary>
    /// <param name="map">ステージとなる地形の2次元配列</param>
    /// <exception cref="System.Exception"></exception>
    private void ApplyMatching(in int[,] map)
    {
        for(int r = 0; r < map.GetLength(0); r++)
            for (int c = 0; c < map.GetLength(1); c++)
            {
                _aStar[r, c] = map[r, c] switch
                {
                    0 => Instantiate(_wall, SetCenter(r, c), Quaternion.identity, transform),
                    1 => Instantiate(_path),
                    _ => throw new System.IndexOutOfRangeException()
                };
            }
    }

    private Vector2 SetCenter(int row, int column)
        => new Vector2(row - _width / 2 + 0.5f, column - _height / 2 + 0.5f);
}
