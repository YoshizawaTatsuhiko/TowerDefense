using System.Collections.Generic;
using UnityEngine;

// 日本語対応
public class Grid : MonoBehaviour
{
    public float TotalCost { get; set; } = 0f;  // 実コスト + 推定コスト
    public int ActualCost { get; set; } = 0;  // 実コスト
    public float HeuristicCost { get; set; } = 0f;  // 推定コスト
    public GridState State { get; set; } = GridState.None;  // セルの状態

    public enum GridState
    {
        None        = 0,
        Open        = 1,
        Close       = 2,
        CannotOpen  = 4,
    }
}
