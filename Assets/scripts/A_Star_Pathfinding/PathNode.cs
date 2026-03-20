using System.Collections.Generic;
using UnityEngine;

// tutorial from https://generalistprogrammer.com/tutorials/a-star-pathfinding-algorithm-complete-tutorial
public class PathNode : MonoBehaviour
{
    public int X { get; set; }
    public int Y { get; set; }

    public float GCost { get; set; }  // Distance from start
    public float HCost { get; set; }  // Heuristic to goal
    public float FCost => GCost + HCost;  // Total estimated cost

    public PathNode Parent { get; set; }
    public bool IsWalkable { get; set; }

    public PathNode(int x, int y, bool walkable = true)
    {
        X = x;
        Y = y;
        IsWalkable = walkable;
        GCost = float.MaxValue;
        HCost = 0;
        Parent = null;
    }

    public void Reset()
    {
        GCost = float.MaxValue;
        HCost = 0;
        Parent = null;
    }

    // Get neighbors for 8-directional movement
    public List<PathNode> GetNeighbors(PathNode[,] grid)
    {
        var neighbors = new List<PathNode>();
        int gridWidth = grid.GetLength(0);
        int gridHeight = grid.GetLength(1);

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) continue;

                int checkX = X + x;
                int checkY = Y + y;

                if (checkX >= 0 && checkX < gridWidth &&
                    checkY >= 0 && checkY < gridHeight)
                {
                    neighbors.Add(grid[checkX, checkY]);
                }
            }
        }

        return neighbors;
    }
}
