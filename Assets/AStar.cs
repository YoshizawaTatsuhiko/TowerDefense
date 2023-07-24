using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 日本語対応
public class AStar : MonoBehaviour
{
    [SerializeField] private int _row = 5;
    [SerializeField] private int _column = 5;
    private Cell[,] _cells = null;

    private void Start()
    {
        _cells = new Cell[ _row, _column ];
    }
}
