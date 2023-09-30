using UnityEngine;

// 日本語対応
public class CellInfo : MonoBehaviour
{
    public float VisitedCost => _visitedCost;
    public bool IsWalkable => _isWalkable;  // このCellが通れるかどうか

    [Tooltip("このCellに移動するときにかかるコスト")]
    [SerializeField] private int _visitedCost = 0;
    [Tooltip("このCellに移動できるかどうか")]
    [SerializeField] private bool _isWalkable = false;
}
