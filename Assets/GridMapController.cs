using System.Collections.Generic;
using UnityEngine;

// 日本語対応
public class GridMapController : MonoBehaviour
{
    [SerializeField] private int _row = 5;
    [SerializeField] private int _column = 5;
    [SerializeField] private Cell _wallTile = null;
    [SerializeField] private Cell _pathTile = null;
    private Cell[,] _cells = null;

    private void Start()
    {
        var maze = new HoleDigging(_row, _column);
        _cells = new Cell[maze.GetMazeWidth(), maze.GetMazeHeight()];
        var blueprint = maze.Get2DArray();
        InitCells(ref blueprint);

        for (int r = 0; r < _cells.GetLength(0); r++)
        {
            for (int c = 0; c < _cells.GetLength(1); c++)
            {
                Instantiate(_cells[r, c], new Vector2
                    (r - _cells.GetLength(0) / 2f + 0.5f, 
                     c - _cells.GetLength(1) / 2f + 0.5f), Quaternion.identity, transform);
            }
        }
    }

    private void InitCells(ref int[,] blueprint)
    {
        for (int r = 0; r < _cells.GetLength(0); r++)
            for (int c = 0; c < _cells.GetLength(1); c++)
            {
                InitCell(ref blueprint, r, c);
            }
    }

    private void InitCell(ref int[,] blueprint, int r, int c)
    {
        if (!TryGetCell(r, c, out Cell cell)) return;

        _cells[r, c] = blueprint[r, c] switch
        {
            0 => _wallTile,
            1 => _pathTile,
            _ => throw new System.Exception("Caseにない値が検出されました。"),
        };
    }

    private bool TryGetCell(int r, int c, out Cell cell)
    {
        if (r < 0 || r >= _cells.GetLength(0) || c < 0 || c >= _cells.GetLength(1))
        {
            cell = null;
            return false;
        }
        cell = _cells[r, c];
        return true;
    }

    private void GetMatchingElement(CellState target, CellState condition, int targetCount)
    {
        List<(int ,int)> matchList = new(8);

        for (int r = 0; r < _cells.GetLength(0); r++)
            for (int c = 0; c < _cells.GetLength(1); c++)
            {
                if (TryGetCell(r, c, out Cell cell))
                {
                    
                }
            }
    }
}
