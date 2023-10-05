public class Cell
{
    public int Row { get; private set; } = 0;  // 行番号
    public int Column { get; private set; } = 0;  // 列番号
    public Cell Parent { get; set; } = null;  // 親ノード
    public bool IsWalkable { get; private set; } = true;  // このCellが通れるかどうか
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