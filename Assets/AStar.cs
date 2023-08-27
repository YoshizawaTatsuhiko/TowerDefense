using System;

// 日本語対応
public class AStar
{
    private Cell[,] _aStarMap = null;
    private (int, int) _start = (0, 0);
    private (int, int) _goal = (0, 0);

    public AStar(Cell[,] map, (int, int) start, (int, int) goal)
    {
        _aStarMap = map;
        _start = start;
        _goal = goal;
    }

    private void ChengeCellState(int a, int b, CellState state)
    {
        if (_aStarMap[a, b].State != state) _aStarMap[a, b].State = state;
    }

    private void ChengeCellState(ref Cell cell, CellState state)
    {
        if (cell.State != state) cell.State = state;
    }

    private float CalcDistance(int a1, int a2, int b1, int b2)
    {
        return MathF.Sqrt((a1 - b1) * (a1 - b1) + (a2 - b2) * (a2 - b2));
    }

    private bool TryGetCell(int r, int c, out Cell cell)
    {
        if (r < 0 || r >= _aStarMap.GetLength(0) || c < 0 || c >= _aStarMap.GetLength(1))
        {
            cell = null;
            return false;
        }
        cell = _aStarMap[r, c];
        return true;
    }
}
