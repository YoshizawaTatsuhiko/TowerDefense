using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using PathFinding;

// 日本語対応
public class Test : MonoBehaviour
{
    [SerializeField] private MapCell _wall = null;
    [SerializeField] private MapCell _path = null;

    private int[,] _map =
    {
        {0,1,1,1,1,1,1,0,1,1,1,0,0,1,1,0,1,1,1,0},
        {0,1,0,0,0,0,1,0,1,0,1,1,1,1,1,1,1,0,1,0},
        {0,1,0,1,1,0,1,0,1,0,0,0,0,1,0,0,1,0,1,0},
        {0,1,0,1,0,0,1,1,1,1,0,1,0,1,1,1,1,0,1,0},
        {0,1,1,1,0,1,1,0,0,1,1,1,0,1,0,0,1,0,1,0},
        {0,0,0,1,1,0,1,1,0,0,0,1,0,1,1,0,1,0,1,0},
        {1,1,1,1,1,1,0,1,1,0,1,1,0,0,0,0,1,0,1,0},
        {1,1,0,1,0,1,0,0,1,0,0,0,0,0,1,0,1,0,1,0},
        {0,1,0,0,0,0,0,0,1,1,1,1,1,0,1,0,1,0,1,0},
        {0,1,1,1,1,0,1,1,1,0,0,1,1,0,1,0,1,0,1,0},
        {0,1,0,0,1,0,0,0,1,1,0,1,0,1,1,0,1,1,1,0},
        {0,1,1,0,1,1,1,0,1,0,0,1,0,1,1,0,1,0,0,0},
        {0,0,1,0,1,0,1,1,1,1,0,1,0,1,0,0,0,0,1,0},
        {1,0,0,0,1,0,1,0,0,1,1,1,0,1,0,1,1,0,1,0},
        {1,1,1,1,1,1,1,0,1,1,0,1,0,1,1,1,0,1,1,0},
        {0,0,1,0,0,0,0,0,0,0,0,1,0,0,0,1,0,0,1,0},
        {0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0},
        {0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,1,0},
        {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
    };
    private AStar _aStar = null;
    private MapCell[,] _mapCells = null;
    private MapCell _start = null;
    private MapCell _goal = null;
    private List<MapCell> _pathList = new List<MapCell>();

    private void Start()
    {
        _aStar = new AStar(_map.GetLength(0), _map.GetLength(1));
        _mapCells = new MapCell[_map.GetLength(0), _map.GetLength(1)];
        ApplyMatching(_map);
        _start = GetRandomPath();
        _goal = GetRandomPath(_start);
        var result = _aStar.FindPath(_start.Row, _start.Column, _goal.Row, _goal.Column);
        PaintPath(result.ShortestPath);
        ChangeCellColor(_start, Color.yellow);
        ChangeCellColor(_goal, Color.blue);
    }

    /// <summary>受け取ったマップに対応する情報を経路探索に渡す</summary>
    /// <param name="map">ステージとなる地形の2次元配列</param>
    /// <exception cref="System.Exception"></exception>
    private void ApplyMatching(in int[,] map)
    {
        MapCell cell = null;

        for (int r = 0; r < _map.GetLength(0); r++)
            for (int c = 0; c < _map.GetLength(1); c++)
            {
                cell = map[r, c] switch
                {
                    0 => Generate(_wall, r, c),
                    1 => Generate(_path, r, c),
                    _ => throw new System.Exception()
                };
                cell.SetCell(r, c);
                _aStar[r, c] = new Cell(r, c, cell.IsWalkable);
                _mapCells[r, c] = cell;
                if (cell.IsWalkable) _pathList.Add(cell);
            }
    }

    private MapCell GetRandomPath(in MapCell excludeCell = null)
    {
        int n = 0;

        do
        {
            n = Random.Range(0, _pathList.Count);
        }
        while (excludeCell ? _pathList[n] == excludeCell : false);

        return _pathList[n];
    }

    private MapCell GetRandomPath(in MapCell[] excludeCells)
    {
        int n = 0;

        do
        {
            n = Random.Range(0, _pathList.Count);
        }
        while (excludeCells.Contains(_pathList[n]));

        return _pathList[n];
    }

    private T Generate<T>(T obj, int row, int column) where T : MonoBehaviour
    {
        var t = Instantiate(obj, ConvertCenter(row, column), Quaternion.identity, transform);
        t.gameObject.name += $"({row}, {column})";
        return t;
    }

    private Vector2 ConvertCenter(int row, int column)
        => new Vector2(column - _map.GetLength(1) / 2 + 0.5f, -row + _map.GetLength(0) / 2 - 0.5f);

    private void ChangeCellColor<T>(in T gameObject, Color color) where T : MonoBehaviour
    {
        if (gameObject.TryGetComponent(out SpriteRenderer renderer)) renderer.color = color;
    }

    private void PaintPath(in List<Cell> pathList)
    {
        if (pathList == null) return;

        foreach (var cell in pathList)
        {
            ChangeCellColor(_mapCells[cell.Row, cell.Column], Color.red);
        }
    }
}
