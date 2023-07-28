using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 日本語対応
public class AStar : MonoBehaviour
{
    [SerializeField] private int _row = 5;
    [SerializeField] private int _column = 5;
    [SerializeField] private Cell _floorTile = null;
    private Cell[,] _cells = null;

    private void Start()
    {
        _cells = new Cell[ _row, _column ];

        for (int y = 0; y < _row; y++)
        {
            for (int x = 0; x < _column; x++)
            {
                _cells[y, x] = Instantiate(_floorTile, 
                    new Vector2(x - (_column / 2f), y - (_row / 2f)), Quaternion.identity, transform);
            }
        }
    }
}
