using UnityEngine;

// 日本語対応
public class GridController : MonoBehaviour, IGridInfo
{
    public Vector2Int GridSize => _gridSize;
    public float CellRadius => _cellRadius;
    public FlowField FlowField => _flowField;

    [SerializeField] private Vector2Int _gridSize = Vector2Int.zero;
    [SerializeField] private float _cellRadius = 0.5f;

    private FlowField _flowField = null;

    private void Start()
    {
        _flowField = new FlowField(_cellRadius, _gridSize.x, _gridSize.y);
        _flowField.CreateGrid();
    }


}
