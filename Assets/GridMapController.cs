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
    HoleDigging _maze = null;

    private void Start()
    {
        _maze = new HoleDigging(_row, _column);
        _cells = new Cell[_maze.GetMazeWidth(), _maze.GetMazeHeight()];
        CellInitializes(_maze.Get2DArray());

        for (int i = 0; i < _maze.GetMazeWidth(); i++)
        {
            for (int j = 0; j < _maze.GetMazeHeight(); j++)
            {
                Instantiate(_cells[i, j], new Vector2
                    (i - _maze.GetMazeWidth() / 2f + 0.5f, 
                     j - _maze.GetMazeHeight() / 2f + 0.5f), Quaternion.identity, transform);
            }
        }
    }

    private void CellInitializes(int[,] blueprint)
    {
        for (int i = 0; i < _cells.GetLength(0); i++)
        {
            for (int j = 0; j < _cells.GetLength(1); j++)
            {
                CellInitialize(blueprint, i, j);
            }
        }
    }

    private void CellInitialize(int[,] blueprint, int i, int j)
    {
        if (!TryGetCell(i, j, out Cell cell)) return;

        switch (blueprint[i, j])
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
        _cells[i, j] = cell;
    }

    //private void GetStartAndGoal(out (int, int) start, out (int, int) goal)
    //{
    //    start = (-1, -1);
    //    goal = (-1, -1);

    //    for (int r = 0; r < _cells.GetLength(0); r++)
    //    {
    //        for (int c = 0; c < _cells.GetLength(1); c++)
    //        {
    //            if (_cells[r, c].State == CellState.Start)
    //            {
    //                start = (r, c);
    //            }
    //            else if (_cells[r, c].State == CellState.Goal)
    //            {
    //                goal = (r, c);
    //            }

    //            if (start != (-1, -1) && goal != (-1, -1)) return;
    //        }
    //    }
    //}

    private void SetCellState(int r, int c, CellState state)
    {
        if (!TryGetCell(r, c, out Cell cell)) return;

        cell.State = state;
    }

    private bool TryGetCell(int r, int c, out Cell cell)
    {
        if (r < 0 || r >= _cells.GetLength(0) || c < 0 || c >= _cells.GetLength(1))
        {
            cell = null;
            return false;
        }
        cell = _cells[r, c];
        return true;
    }
}
