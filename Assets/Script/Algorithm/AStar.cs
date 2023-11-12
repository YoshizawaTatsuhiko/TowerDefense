using System;
using System.Collections.Generic;
using System.Linq;

// 日本語対応
namespace PathFinding
{
    public class AStar
    {
        public Cell this[int row, int column] => _grid[row, column];

        /// <summary>経路探索に用いる情報が入った2次元配列</summary>
        private readonly Cell[,] _grid = null;
        /// <summary>次に探索する候補となるCellを入れる</summary>
        private List<Cell> _openCells = new List<Cell>();
        /// <summary>探索済みのCellを入れる</summary>
        private HashSet<Cell> _closeCells = new HashSet<Cell>();
        /// <summary>ある１つのCellの周囲４マス(または、8マス)のCellを入れる</summary>
        private Cell[] _neighborCells = new Cell[8];
        /// <summary>経路探索を行う際、斜め方向の探索も行うかを判定する</summary>
        private bool _hasUseDiagonal = false;
        private (int, int)[] _directions =
        {
            (-1, 0),     // Up
            (1, 0),    // Down
            (0, -1),    // Left
            (0, 1),     // Right
            (-1, 1),     // UpperRight
            (1, 1),    // LowerRight
            (1, -1),   // LowerLeft
            (-1, -1),    // UpperLeft

        };
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
        public PathResult FindPath(int startX, int startY, int targetX, int targetY, bool hasUseDiagonal = false)
        {
            if (!TryGetCell(startX, startY, out Cell startCell)
                || !TryGetCell(targetX, targetY, out Cell targetCell))  // 渡された座標のCellが取得できるか確認する
            {
                throw new ArgumentOutOfRangeException();
            }
            _hasUseDiagonal = hasUseDiagonal;
            _openCells.Add(startCell);  // 探索候補に追加する

            while (_openCells.Count > 0)  // 探索候補がなくなったらループをやめる
            {
                Cell currentCell = FindLowestCostCell();
                _openCells.Remove(currentCell);  // 探索候補から削除する
                _closeCells.Add(currentCell);  // 探索済みに追加する

                if (currentCell == targetCell)  // 目的のセルに到達したら、結果を返して関数を抜ける
                {
                    return ConstructPath(targetCell);
                }

                // currentCellに隣接したセルに探索候補となるセルがあるかを確認し、隣接したセルに各種情報を渡す
                foreach (var neighbor in FindNeighborCell(currentCell))
                {
                    if (neighbor == null) break;
                    if (!neighbor.IsWalkable || _closeCells.Contains(neighbor)) continue;

                    float tmpActualCost = neighbor.ActualCost + CalcDistance(currentCell, neighbor);

                    if (!_openCells.Contains(neighbor) || tmpActualCost < neighbor.ActualCost)
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

        /// <summary>引数で受け取った行番号・列番号目のセルを初期化する</summary>
        /// <param name="r">行番号</param>
        /// <param name="c">列番号</param>
        /// <param name="isWalkable">このセルが通行可能かどうか</param>
        public void InitCell(int r, int c, bool isWalkable)
        {
            if (!TryGetCell(r, c, out Cell cell)) return;

            _grid[r, c] = new Cell(r, c, isWalkable);
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

        /// <summary>最も探索コストの低いセルを探す</summary>
        /// <returns>次に開くセル</returns>
        private Cell FindLowestCostCell()
        {
            _openCells.OrderBy(t => t.TotalCost).ThenBy(h => h.HeuristicCost);
            return _openCells[0];
        }

        /// <summary>受け取ったセルの上下左右に隣接したCellを取得する</summary>
        /// <param name="target">基準となるセル</param>
        private Cell[] FindNeighborCell(in Cell target)
        {
            Array.Fill(_neighborCells, null);
            int r = target.Row, c = target.Column;
            int index = 0;

            { if (TryGetCell(r + 1, c, out Cell neighbor)) _neighborCells[index++] = neighbor; } // Up
            { if (TryGetCell(r - 1, c, out Cell neighbor)) _neighborCells[index++] = neighbor; } // Down
            { if (TryGetCell(r, c - 1, out Cell neighbor)) _neighborCells[index++] = neighbor; } // Left
            { if (TryGetCell(r, c + 1, out Cell neighbor)) _neighborCells[index]   = neighbor; } // Right

            return _neighborCells;
        }

        /// <summary>2つのセルの距離を計算する</summary>
        /// <returns>計算されたセルのマンハッタン距離</returns>
        private float CalcDistance(Cell from, Cell to) =>
            (MathF.Abs(from.Row - to.Row) + MathF.Abs(from.Column - to.Column));

        /// <summary>受け取ったセルからスタート地点までの経路を構築する</summary>
        /// <returns>最短経路</returns>
        private PathResult ConstructPath(in Cell targetCell)
        {
            PathResult result = new();
            Cell currentCell = targetCell;

            while (currentCell != null)
            {
                result.ShortestPath.Add((currentCell.Column, currentCell.Row, 0));
                currentCell = currentCell.Parent;
            }
            result.ShortestPath.Reverse();
            return result;
        }
    }
}