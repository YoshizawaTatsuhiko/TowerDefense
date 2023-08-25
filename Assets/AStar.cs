using System;
using System.Collections.Generic;

// 日本語対応
public class AStar<T>
{
    private Cell[,] _aStarMap = null;

    public AStar(T[,] map, (int, int) start, (int, int) goal)
    {
        _aStarMap = new Cell[map.GetLength(0), map.GetLength(1)];
    }

    private float CalcDistance(int a1, int a2, int b1, int b2)
    {
        return MathF.Sqrt((a1 - b1) * (a1 - b1) + (a2 - b2) * (a2 - b2));
    }
}
