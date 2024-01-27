using UnityEngine;

// 日本語対応
public class GridDebug : MonoBehaviour
{
    [SerializeField] private bool _isDisplayGrid = false;
    [SerializeField] private DebugDisplayType _displayType = DebugDisplayType.None;
    [SerializeField] private Sprite _debugIcons = null;

    [SerializeReference, SubclassSelector]
    private IGridInfo _gridInfo = null;

    private void OnDrawGizmos()
    {
        if (_isDisplayGrid)
        {
            if (_gridInfo.FlowField == null)
            {
                //DrawGrid(_gridSize, Color.green, _cellRadius);
            }
            else
            {
                //DrawGrid()
            }
        }
    }

    private void DrawGrid(Vector2Int gridSize, Color gridColor, float cellRadius)
    {
        Gizmos.color = gridColor;

        for (int r = 0; r < gridSize.y; r++)
            for (int c = 0; c < gridSize.x; c++)
            {
                Vector3 center = _gridInfo.FlowField switch
                {
                    null => new Vector3(c - gridSize.x / 2f - cellRadius, 0.0f, c - gridSize.y / 2f - cellRadius),
                    _ => new Vector3(_gridInfo.FlowField.Grid[r, c].WorldPos.X,
                                     _gridInfo.FlowField.Grid[r, c].WorldPos.Y,
                                     _gridInfo.FlowField.Grid[r, c].WorldPos.Z),
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
