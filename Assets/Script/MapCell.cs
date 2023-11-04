using UnityEngine;

// 日本語対応
public class MapCell : MonoBehaviour
{
    public int Row { get; private set; } = 0;  // 行番号
    public int Column { get; private set; } = 0;  // 列番号
    public float VisitedCost { get; private set; } = 0.0f;  // このセルを訪れるのにかかるコスト
    public bool IsWalkable { get; private set; } = false;  // このCellが通れるかどうか
    public Vector3 ToDestination { get; set; } = Vector3.zero;  // 目的地の方向

    public MapCell SetInfo(int row, int column, float cost, bool isWalkable)
    {
        Row = row;
        Column = column;
        VisitedCost = cost;
        IsWalkable = isWalkable;
        return this;
    }
}
