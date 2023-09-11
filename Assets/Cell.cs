using UnityEngine;

// 日本語対応
public class Cell : MonoBehaviour
{
    public int Row { get; set; } = 0;
    public int Column { get; set; } = 0;
    public Cell Parent { get; set; } = null;  // 親ノード
    public float TotalCost => _actualCost + _heuristicCost;  // 実コスト + 推定コスト
    public float ActualCost { get => _actualCost; set => _actualCost = value; }  // 実コスト
    public float HeuristicCost { get => _heuristicCost; set => _heuristicCost = value; }  // 推定コスト
    public CellState State { get => _cellState; set => _cellState = value; }  // セルの状態

    [SerializeField] private CellState _cellState = CellState.None;
    private float _actualCost = 0f;
    private float _heuristicCost = 0f;
}

public enum CellState
{
    None = 0,
    Open = 1,
    Close = 2,
    Start = 4,
    Goal = 8,
    CannotOpen = 16,
}
