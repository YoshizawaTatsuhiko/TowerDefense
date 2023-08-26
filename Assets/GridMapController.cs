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
        GenerateMap(in blueprint);

        var matchList = GetMatchingElement(_pathTile.State, _wallTile.State, 3);
        CellState state = CellState.None;
        Color color = Color.white;
        (int, int) start = (-1, -1);
        (int, int) goal = (-1, -1);

        for (int i = 0, n = Random.Range(0, matchList.Count); i < 2; i++, n = Random.Range(0, matchList.Count))
        {
            var pair = matchList[n];

            if (i == 0)
            {
                state = CellState.Start;
                color = Color.yellow;
                start = pair;
            }
            else
            {
                state = CellState.Goal;
                color = Color.red;
                goal = pair;
            }
            SetCell(_cells[pair.Item1, pair.Item2], state, color);
            matchList.RemoveAt(n);
        }
    }

    private void GenerateMap(in int[,] blueprint)
    {
        for (int r = 0; r < _cells.GetLength(0); r++)
        {
            for (int c = 0; c < _cells.GetLength(1); c++)
            {
                _cells[r, c] = Instantiate(InitCell(in blueprint, r, c), new Vector2
                    (r - _cells.GetLength(0) / 2f + 0.5f,
                     c - _cells.GetLength(1) / 2f + 0.5f), Quaternion.identity, transform);
            }
        }
    }

    private Cell InitCell(in int[,] blueprint, int r, int c)
    {
        if (!TryGetCell(r, c, out Cell cell)) return null;

        cell = blueprint[r, c] switch
        {
            0 => _wallTile,
            1 => _pathTile,
            _ => throw new System.Exception("Caseにない値が検出されました。"),
        };
        cell.CellVector = new Vector2 (r, c);
        return cell;
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

    private void SetCell(Cell cell, CellState state = CellState.None, Color color = new Color())
    {
        cell.State = state;
        
        if(cell.TryGetComponent(out SpriteRenderer renderer)) renderer.color = color;
    }

    private List<(int, int)> GetMatchingElement(CellState target, 
        CellState aroundState = CellState.None, int targetCount = 0)
    {
        List<(int, int)> matchList = new(8);

        for (int r = 0; r < _cells.GetLength(0); r++)
            for (int c = 0, count = 0; c < _cells.GetLength(1); c++, count = 0)
            {
                if (TryGetCell(r, c, out Cell cell) && cell.State == target)
                {
                    { if (TryGetCell(r - 1, c, out Cell neighbor) && neighbor.State == aroundState) count++; }
                    { if (TryGetCell(r + 1, c, out Cell neighbor) && neighbor.State == aroundState) count++; }
                    { if (TryGetCell(r, c - 1, out Cell neighbor) && neighbor.State == aroundState) count++; }
                    { if (TryGetCell(r, c + 1, out Cell neighbor) && neighbor.State == aroundState) count++; }

                    if (count >= targetCount) matchList.Add((r, c));
                }
            }
        return matchList;
    }
}
