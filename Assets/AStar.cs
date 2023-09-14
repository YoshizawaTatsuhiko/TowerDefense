using System;
using System.Collections.Generic;
using System.Linq;

// 日本語対応
public class AStar
{
    private readonly int _row = 0;
    private readonly int _column = 0;
    private readonly Cell[,] _grid = null;
    private List<Cell> _openCells = new List<Cell>();
    private HashSet<Cell> _closeCells = new HashSet<Cell>();
    private List<Cell> _neighborCells = new List<Cell>(4);

    public AStar(int row, int column)
    {
        _row = row;
        _column = column;
        _grid = new Cell[row, column];
    }

    public void FindPath(int startRow, int startColumn, int targetRow, int targetColumn)
    {
        if (!TryGetCell(startRow, startColumn, out Cell startCell)
            || !TryGetCell(targetRow, targetColumn, out Cell targetCell))
        {
            throw new ArgumentOutOfRangeException();
        }
        _openCells.Add(startCell);

        while(_openCells.Count > 0)
        {
            Cell currentCell = FindLowestCostCell();
            _openCells.Remove(currentCell);
            _closeCells.Add(currentCell);

            if (currentCell == targetCell)
            {
                return;  // 最短経路を返す
            }
            FindNeighborCell(currentCell);

            foreach (var neighbor in _neighborCells)
            {
                if (!neighbor.IsWalkable || _closeCells.Contains(neighbor)) continue;
            }
        }
    }

    /// <summary>2次元配列に受け取った行番号・列番号のCellが取得できるかを判定する</summary>
    /// <param name="row">行番号</param>
    /// <param name="column">列番号</param>
    /// <param name="cell">行番号・列番号目のCell</param>
    /// <returns>取得できる -> true | 取得できない -> false</returns>
    private bool TryGetCell(int row, int column, out Cell cell)
    {
        if (row < 0 || row >= _grid.GetLength(0) || column < 0 || column >= _grid.GetLength(1))
        {
            cell = null;
            return false;
        }
        cell = _grid[row, column];
        return true;
    }

    /// <summary>最も探索コストの低いCellを探す</summary>
    /// <returns>次に開くCell</returns>
    private Cell FindLowestCostCell()
    {
        _openCells.OrderBy(t => t.TotalCost).ThenBy(h => h.HeuristicCost);
        return _openCells[0];
    }

    /// <summary>受け取ったCellの上下左右に隣接したCellを取得する</summary>
    /// <param name="target">基準となるCell</param>
    private void FindNeighborCell(Cell target)
    {
        _neighborCells.Clear();
        int r = target.Row, c = target.Column;

        { if (TryGetCell(r + 1, c, out Cell neighbor)) _neighborCells.Add(neighbor); } // Up
        { if (TryGetCell(r - 1, c, out Cell neighbor)) _neighborCells.Add(neighbor); } // Down
        { if (TryGetCell(r, c - 1, out Cell neighbor)) _neighborCells.Add(neighbor); } // Left
        { if (TryGetCell(r, c + 1, out Cell neighbor)) _neighborCells.Add(neighbor); } // Right
    }

    /// <summary>2つのCellの距離を計算する</summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <returns>計算されたCellの直線距離</returns>
    private float CalcDistance(Cell from, Cell to) =>
        MathF.Abs(from.Row - to.Row) + MathF.Abs(from.Column - to.Column);
}
