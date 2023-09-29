using UnityEngine;

// 日本語対応
public class Cell : MonoBehaviour
{
    public int Row { get; set; } = 0;
    public int Column { get; set; } = 0;
    public Cell Parent { get; set; } = null;  // 親ノード
    public bool IsWalkable => _isWalkable;  // このCellが通れるかどうか
    public float TotalCost => ActualCost + HeuristicCost;  // 合計コスト = 実コスト + 推定コスト
    public int ActualCost { get => _actualCost; set => _actualCost = value; }  // 実コスト = スタートからどのくらい進んだか
    public float HeuristicCost { get; set; } = 0f;  // 推定コスト = ゴールからどのくらい離れているか

    [Tooltip("このCellに移動するときにかかるコスト")]
    [SerializeField] private int _actualCost = 0;
    [Tooltip("このCellに移動できるかどうか")]
    [SerializeField] private bool _isWalkable = false;
}

public enum CellState
{
    None = 0,
    Open = 1,
    Close = 2,
    Start = 4,
    Goal = 8,
}
