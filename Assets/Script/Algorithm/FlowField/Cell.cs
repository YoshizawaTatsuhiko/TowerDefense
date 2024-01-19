using System;
using System.Numerics;

// 日本語対応
public class Cell
{
    public Vector3 WorldPos { get; private set; } = Vector3.Zero;
    public Vector2 GridPos { get; private set; } = Vector2.Zero;
    public byte Cost { get; set; } = 1;
    public ushort BestCost { get; set; } = ushort.MaxValue;

    public Cell(Vector3 worldPos, Vector2 gridPos)
    {
        WorldPos = worldPos;
        GridPos = gridPos;
    }

    public void AddCost(byte cost)
    {
        if (Cost == byte.MaxValue) return;
        Cost = (byte)Math.Clamp(Cost + cost, 0, byte.MaxValue);
    }
}
