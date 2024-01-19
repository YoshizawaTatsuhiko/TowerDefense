using System.Numerics;

// 日本語対応
public class FlowField
{
    public Cell[,] Grid { get; private set; } = null;
    public (int Row, int Col) GridSize { get; private set; } = (0, 0);
    public Cell DestinationCell { get; private set; } = null;

    public readonly float CellRadius = 0.0f;
    public readonly float CellDiameter = 0.0f;

    public FlowField(float cellRadius, int width, int height)
    {
        Grid = new Cell[height, width];
        GridSize = (height, width);
        CellRadius = cellRadius;
        CellDiameter = cellRadius * 2.0f;
    }

    public void CreateGrid()
    {
        for (int r = 0; r < GridSize.Row; r++)
            for (int c = 0; c < GridSize.Col; c++)
            {
                Vector3 worldPos = GridIndex2WorldPos(r, c);
                Grid[r, c] = new Cell(worldPos, new Vector2(c, r));
            }
    }

    private Vector3 GridIndex2WorldPos(int r, int c)
    {
        return new Vector3(c - GridSize.Col / 2f - CellRadius, 0.0f, r + GridSize.Row / 2f + CellRadius);
    }
}