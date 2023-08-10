using System;
using System.Collections.Generic;
using UnityEngine;

// 日本語対応
public class GridMapController : MonoBehaviour
{
    [SerializeField] private int _row = 5;
    [SerializeField] private int _column = 5;
    [SerializeField] private Cell _wallTile = null;
    [SerializeField] private Cell _floorTile = null;
    private Cell[,] _cells = null;

    private void Start()
    {
        _cells = new Cell[_row, _column];
        var maze = new HoleDigging(_row, _column);
        Cell cell = null;

        for (int i = 0; i < maze.GetMazeHeight(); i++)
        {
            for (int j = 0; j < maze.GetMazeWidth(); j++)
            {
                if (i * j == 1)
                {
                    cell = _floorTile;
                    cell.State = CellState.Start;
                    cell.GetComponent<SpriteRenderer>().color = Color.yellow;
                }
                else
                {
                    switch (maze[i, j])
                    {
                        case 0:
                            cell = _wallTile;
                            cell.State = CellState.CannotOpen;
                            cell.gameObject.GetComponent<SpriteRenderer>().color = Color.black;
                            break;
                        case 1:
                            cell = _floorTile;
                            cell.State = CellState.None;
                            cell.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
                            break;
                    }
                }
                _cells[i, j] = Instantiate(cell, new Vector2
                        (i - maze.GetMazeHeight() / 2f + 0.5f, j - maze.GetMazeWidth() / 2f + 0.5f), Quaternion.identity, transform);
            }
        }
    }

    private Cell SearchStartCell(Cell[,] cells)
    {
        for(int i = 0; i < cells.GetLength(0); i++)
        {
            for (int j = 0; j < cells.GetLength(1); j++)
            {
                if (cells[i, j].State == CellState.Start) return cells[i, j];
            }
        }
        return null;
    }
}
