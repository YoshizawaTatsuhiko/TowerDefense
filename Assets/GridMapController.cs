using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 日本語対応
public class GridMapController : MonoBehaviour
{
    [SerializeField] private int _row = 5;
    [SerializeField] private int _column = 5;
    [SerializeField] private Cell _floorTile = null;
    private Cell[,] _cells = null;

    private void Start()
    {
        _cells = new Cell[ _row, _column ];

        for (int r = 0; r < _row; r++)
        {
            for (int c = 0; c < _column; c++)
            {
                _cells[r, c] = Instantiate(_floorTile,
                    new Vector2(r - (_row / 2f) + 0.5f, c - (_column / 2f) + 0.5f), Quaternion.identity, transform);
            }
        }
    }
}
