using System.Collections.Generic;

// 日本語対応
namespace PathFinding
{
    public class PathResult
    {
        public PathResult()
        {
            ShortestPath = new();
        }

        /// <summary>最短経路となるセルが格納される</summary>
        public List<(int X, int Y, int Z)> ShortestPath = null;
    }
}