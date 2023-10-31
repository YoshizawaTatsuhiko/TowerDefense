using UnityEngine;

// 日本語対応
public class MapCell : MonoBehaviour
{
    public int Row { get; private set; } = 0;
    public int Column { get; private set; } = 0;
    public float VisitedCost { get; private set; } = 0.0f;
    public bool IsWalkable { get; private set; } = false;  // このCellが通れるかどうか

    public MapCell SetInfo(int row, int column, float cost, bool isWalkable)
    {
        Row = row;
        Column = column;
        VisitedCost = cost;
        IsWalkable = isWalkable;
        return this;
    }
}
