using System.Collections.Generic;

// 日本語対応
namespace PathFinding
{
    public class AStarPathResult
    {
        public AStarPathResult()
        {
            ShortestPath = new List<Cell>();
        }

        public List<Cell> ShortestPath = null;
    }
}