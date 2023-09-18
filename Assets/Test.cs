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
        for(int i = 0; i < map.GetLength(0); i++)
            for (int j = 0; j < map.GetLength(1); j++)
            {
                _aStar[i, j] = map[i, j] switch
                {
                    0 => new Cell(i, j, isWalkable: false),
                    1 => new Cell(i, j, isWalkable: true),
                    _ => throw new System.Exception()
                };
            }
    }
}
