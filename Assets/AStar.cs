using System;
using System.Collections.Generic;
using System.Linq;

// 日本語対応
public class AStar
{
    public Cell this[int row, int column]
    {
        get => _grid[row, column];
        set => _grid[row, column] = value;
    }

    private readonly Cell[,] _grid = null;
    private List<Cell> _openCells = new List<Cell>();
    private HashSet<Cell> _closeCells = new HashSet<Cell>();
    private Cell[] _neighborCells = new Cell[4];

    public AStar(int width, int height)
    {
        _grid = new Cell[width, height];
    }

    /// <summary>最短経路を探索する</summary>
    /// <param name="startX">開始地点の水平方向座標</param>
    /// <param name="startY">開始地点の垂直方向座標</param>
    /// <param name="targetX">目標地点の水平方向座標</param>
    /// <param name="targetY">目標地点の垂直方向座標</param>
    /// <returns>最短経路となるCellが格納されたリスト</returns>
    public List<Cell> FindPath(int startX, int startY, int targetX, int targetY)
    {
        if (!TryGetCell(startX, startY, out Cell startCell)
            || !TryGetCell(targetX, targetY, out Cell targetCell))
        {
            throw new ArgumentOutOfRangeException();
        }
        _openCells.Add(startCell);

        while (_openCells.Count > 0)
        {
            Cell currentCell = FindLowestCostCell();
            _openCells.Remove(currentCell);
            _closeCells.Add(currentCell);

            if (currentCell == targetCell)
            {
                return ConstructPath(targetCell);  // 最短経路を返す
            }
            FindNeighborCell(currentCell);

            foreach (var neighbor in _neighborCells)
            {
                if (neighbor == null) break;
                if (!neighbor.IsWalkable || _closeCells.Contains(neighbor)) continue;

                int tmpActualCost = neighbor.ActualCost + CalcDistance(currentCell, neighbor);

                if (!_openCells.Contains(neighbor) || /*←need?*/ tmpActualCost < neighbor.ActualCost)
                {
                    neighbor.Parent = currentCell;
                    neighbor.ActualCost = tmpActualCost;
                    neighbor.HeuristicCost = CalcDistance(neighbor, targetCell);

                    if (!_openCells.Contains(neighbor)) _openCells.Add(neighbor);
                }
            }
        }
        return null;
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
        Array.Fill(_neighborCells, null);
        int r = target.Row, c = target.Column;
        int index = 0;

        { if (TryGetCell(r + 1, c, out Cell neighbor)) { _neighborCells[index] = neighbor; index++; } } // Up
        { if (TryGetCell(r - 1, c, out Cell neighbor)) { _neighborCells[index] = neighbor; index++; } } // Down
        { if (TryGetCell(r, c - 1, out Cell neighbor)) { _neighborCells[index] = neighbor; index++; } } // Left
        { if (TryGetCell(r, c + 1, out Cell neighbor)) { _neighborCells[index] = neighbor; } }          // Right
    }

    /// <summary>2つのCellの距離を計算する</summary>
    /// <returns>計算されたCellのマンハッタン距離</returns>
    private int CalcDistance(Cell from, Cell to) =>
        (int)(MathF.Abs(from.Row - to.Row) + MathF.Abs(from.Column - to.Column));

    /// <summary>受け取ったCellからスタート地点までの経路を構築する</summary>
    /// <param name="targetCell">ここまでの経路を知りたいCell</param>
    /// <returns>最短経路</returns>
    private List<Cell> ConstructPath(Cell targetCell)
    {
        List<Cell> path = new List<Cell>();
        Cell currentCell = targetCell;

        while (currentCell != null)
        {
            path.Add(currentCell);
            currentCell = currentCell.Parent;
        }
        path.Reverse();
        return path;
    }
}
