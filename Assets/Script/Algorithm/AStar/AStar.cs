using System;
using System.Collections.Generic;
using System.Linq;

// 日本語対応
namespace PathFinding
{
    public class AStar
    {
        public class Cell
        {
            public int Row { get; private set; } = 0;  // 行番号
            public int Column { get; private set; } = 0;  // 列番号
            public Cell Parent { get; set; } = null;  // 親ノード
            public bool IsWalkable { get; set; } = true;  // このCellが通れるかどうか
            public float TotalCost => ActualCost + HeuristicCost;  // 合計コスト = 実コスト + 推定コスト
            public float ActualCost { get; set; } = 0f;  // 実コスト = スタートからどのくらい進んだか
            public float HeuristicCost { get; set; } = 0f;  // 推定コスト = ゴールからどのくらい離れているか

            public Cell(int row, int column, bool isWalkable)
            {
                Row = row;
                Column = column;
                IsWalkable = isWalkable;
            }
        }

        public Cell this[int row, int column] => _grid[row, column];

        /// <summary>経路探索に用いる情報が入った2次元配列</summary>
        private readonly Cell[,] _grid = null;
        /// <summary>次に探索する候補となるCellを入れる</summary>
        private List<Cell> _openCells = new List<Cell>();
        /// <summary>探索済みのCellを入れる</summary>
        private HashSet<Cell> _closedCells = new HashSet<Cell>();
        /// <summary>ある１つのCellの周囲４マス(または、8マス)のCellを入れる</summary>
        private Cell[] _neighborCells = new Cell[8];
        /// <summary>経路探索を行う際、斜め方向の探索も行うかを判定する</summary>
        private bool _hasConsiderDiagonal = false;
        /// <summary>探索する方向が入った配列</summary>
        private (int V, int H)[] _directions =
        {
            (-1, 0),   // Up
            (0, 1),    // Right
            (1, 0),    // Down
            (0, -1),   // Left
            (-1, 1),   // UpperRight
            (1, 1),    // LowerRight
            (1, -1),   // LowerLeft
            (-1, -1),  // UpperLeft

        };

        public AStar(int width, int height)
        {
            _grid = new Cell[width, height];
        }

        /// <summary>最短経路を探索する</summary>
        /// <param name="startX">開始地点の水平方向座標</param>
        /// <param name="startYZ">開始地点の垂直方向座標</param>
        /// <param name="targetX">目標地点の水平方向座標</param>
        /// <param name="targetYZ">目標地点の垂直方向座標</param>
        /// <returns>最短経路となるCellが格納されたリスト</returns>
        public PathResult FindPath(int startX, int startYZ, int targetX, int targetYZ, bool hasConsiderDiagonal = false)
        {
            if (!TryGetCell(startX, startYZ, out Cell startCell)
                || !TryGetCell(targetX, targetYZ, out Cell targetCell))  // 渡された座標のCellが取得できるか確認する
            {
                throw new ArgumentOutOfRangeException();
            }
            _hasConsiderDiagonal = hasConsiderDiagonal;
            _openCells.Add(startCell);  // 探索候補に追加する

            while (_openCells.Count > 0)  // 探索候補がなくなったらループをやめる
            {
                Cell currentCell = FindLowestCostCell();
                _openCells.Remove(currentCell);  // 探索候補から削除する
                _closedCells.Add(currentCell);  // 探索済みに追加する

                if (currentCell == targetCell)  // 目的のセルに到達したら、結果を返して関数を抜ける
                {
                    return ConstructPath(targetCell);
                }

                // currentCellに隣接したセルに探索候補となるセルがあるかを確認し、隣接したセルに各種情報を渡す
                foreach (var neighbor in FindNeighborCell(currentCell))
                {
                    if (neighbor == null) break;
                    if (!neighbor.IsWalkable || _closedCells.Contains(neighbor)) continue;

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
            if (!TryCheckCell(r, c)) return;

            _grid[r, c] = new Cell(r, c, isWalkable);
        }

        /// <summary>2次元配列から、受け取った行番号・列番号のCellが取得できるかを判定する</summary>
        /// <param name="row">行番号</param>
        /// <param name="col">列番号</param>
        /// <param name="cell">行番号・列番号目のCell</param>
        /// <returns>取得できる -> true | 取得できない -> false</returns>
        private bool TryGetCell(int row, int col, out Cell cell)
        {
            if (row < 0 || row >= _grid.GetLength(0) || col < 0 || col >= _grid.GetLength(1))
            {
                cell = null;
                return false;
            }
            cell = _grid[row, col];
            return true;
        }

        /// <summary>2次元配列から、受け取った行番号・列番号のCellが取得できるかを判定する</summary>
        /// <param name="row">行番号</param>
        /// <param name="col">列番号</param>
        /// <returns>取得できる -> true | 取得できない -> false</returns>
        private bool TryCheckCell(int row, int col) =>
            !(row < 0 || row >= _grid.GetLength(0) || col < 0 || col >= _grid.GetLength(1));

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
            int dirCount = _directions.Length / (_hasConsiderDiagonal ? 1 : 2);

            for (int i = 0, index = 0; i < dirCount; i++)
            {
                var dir = _directions[i];
                if (TryGetCell(r + dir.V, c + dir.H, out Cell cell)) _neighborCells[index++] = cell;
            }
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