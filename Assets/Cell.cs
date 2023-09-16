using UnityEngine;

// 日本語対応
public class Cell : MonoBehaviour
{
    public int Row => _row;
    public int Column => _column;
    public Cell Parent { get; set; } = null;  // 親ノード
    public bool IsWalkable => _isWalkable;  // このCellが通れるかどうか
    public float TotalCost => ActualCost + HeuristicCost;  // 合計コスト = 実コスト + 推定コスト
    public int ActualCost { get => _actualCost; set => _actualCost = value; }  // 実コスト = スタートからどのくらい進んだか
    public float HeuristicCost { get; set; } = 0f;  // 推定コスト = ゴールからどのくらい離れているか

    [Tooltip("このCellに移動するときにかかるコスト")]
    [SerializeField] private int _actualCost = 0;
    [Tooltip("このCellに移動できるかどうか")]
    [SerializeField] private bool _isWalkable = false;

    private int _row = 0;
    private int _column = 0;

    /// <summary>このCellに経路探索においての基本情報を入れる</summary>
    /// <param name="row">行番号</param>
    /// <param name="column">列番号</param>
    /// <param name="isWalkable">このCellに移動できるかどうか</param>
    public Cell(int row, int column, bool isWalkable)
    {
        _row = row;
        _column = column;
        _isWalkable = isWalkable;
    }
}

public enum CellState
{
    None = 0,
    Open = 1,
    Close = 2,
    Start = 4,
    Goal = 8,
}
