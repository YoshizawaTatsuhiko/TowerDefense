using UnityEngine;

// 日本語対応
public class GridDebug : MonoBehaviour
{
    [SerializeField] private bool _isDisplayGrid = false;
    [SerializeField] private DebugDisplayType _displayType = DebugDisplayType.None;
    [SerializeField] private Sprite _debugIcons = null;

    private IGridDataContainer _gridData = null;

    private void OnDrawGizmos()
    {
        if (_gridData == null) _gridData = FindObjectsByType<GridController>(FindObjectsSortMode.None)[0];

        if (_isDisplayGrid)
        {
            if (_gridData.FlowField == null)
            {
                DrawGrid(_gridData.GridSize, Color.green, _gridData.CellRadius);
            }
            else
            {
                Vector2Int gridSize = new Vector2Int(_gridData.FlowField.GridSize.Col, _gridData.FlowField.GridSize.Row);
                DrawGrid(gridSize, Color.yellow, _gridData.FlowField.CellRadius);
            }
        }
    }

    private void DrawGrid(Vector2Int gridSize, Color gridColor, float cellRadius)
    {
        Gizmos.color = gridColor;

        for (int r = 0; r < gridSize.y; r++)
            for (int c = 0; c < gridSize.x; c++)
            {
                Vector3 center = _gridData.FlowField switch
                {
                    null => new Vector3(c - gridSize.x / 2f + cellRadius, 0.0f, r - gridSize.y / 2f + cellRadius),
                    _ => new Vector3(_gridData.FlowField.Grid[r, c].WorldPos.X,
                                     _gridData.FlowField.Grid[r, c].WorldPos.Y,
                                     _gridData.FlowField.Grid[r, c].WorldPos.Z),
                };
                Vector3 size = Vector3.one * cellRadius * 2;
                Gizmos.DrawWireCube(center, size);
            }
    }

    private enum DebugDisplayType
    {
        None,
        CostField,
        IntegrationField,
        DestinationIcon,
        AllIcon,
    }
}
