using System.Collections.Generic;
using UnityEngine;

// 日本語対応
public class Cell : MonoBehaviour
{
    public float TotalCost { get; set; } = 0f;  // 実コスト + 推定コスト
    public float ActualCost { get; set; } = 0f;  // 実コスト
    public float HeuristicCost { get; set; } = 0f;  // 推定コスト
    public CellState State => _cellState;  // セルの状態

    [SerializeField ] private CellState _cellState = CellState.None;
}

public enum CellState
{
    None = 0,
    Open = 1,
    Close = 2,
    CannotOpen = 4,
}
