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
                    cell = CellInitialize(maze, i, j);
                }
                _cells[i, j] = Instantiate(cell, new Vector2
                        (i - maze.GetMazeHeight() / 2f + 0.5f, j - maze.GetMazeWidth() / 2f + 0.5f), Quaternion.identity, transform);
            }
        }
    }

    private Cell CellInitialize(HoleDigging maze, int i, int j)
    {
        if (!TryGetCell(i, j, out Cell cell)) return null;

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
        return cell;
    }

    private void GetStartAndGoal(ref (int, int) start, ref (int, int) goal)
    {
        byte isComplete = 0b_0000;

        for (int r = 0; r < _cells.GetLength(0); r++)
        {
            for (int c = 0; c < _cells.GetLength(1); c++)
            {
                if (_cells[r, c].State == CellState.Start)
                {
                    start = (r, c);
                    isComplete = 0b_0001;
                }
                else if (_cells[r, c].State == CellState.Goal)
                {
                    goal = (r, c);
                    isComplete = 0b_0010;
                }

                if (isComplete == 0b_0011) return;
            }
        }
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
