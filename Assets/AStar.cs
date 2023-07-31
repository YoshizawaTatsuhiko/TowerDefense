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

        for (int r = 0; r < _row; r++)
        {
            for (int c = 0; c < _column; c++)
            {

            }
        }
    }
}
