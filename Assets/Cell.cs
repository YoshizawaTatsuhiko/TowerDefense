// 日本語対応
public class Cell
{
    public int Row => _row;
    public int Column => _column;
    public Cell Parent { get; set; } = null;  // 親ノード
    public bool IsWalkable => _isWalkable;  // このCellが通れるかどうか
    public float TotalCost => ActualCost + HeuristicCost;  // 合計コスト = 実コスト + 推定コスト
    public int ActualCost { get; set; } = 0;  // 実コスト = スタートからどのくらい進んだか
    public float HeuristicCost { get; set; } = 0f;  // 推定コスト = ゴールからどのくらい離れているか

    private int _row = 0;
    private int _column = 0;
    private bool _isWalkable = false;

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
