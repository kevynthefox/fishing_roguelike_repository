using System.Collections.Generic;
using UnityEngine;

// tutorial from https://generalistprogrammer.com/tutorials/a-star-pathfinding-algorithm-complete-tutorial
public class AStarPathfinder : MonoBehaviour
{
    private PathNode[,] grid;
    private int width;
    private int height;

    public AStarPathfinder(int width, int height)
    {
        this.width = width;
        this.height = height;
        grid = new PathNode[width, height];

        // Initialize grid
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                grid[x, y] = new PathNode(x, y);
            }
        }
    }

    public void SetWalkable(int x, int y, bool walkable)
    {
        if (x >= 0 && x < width && y >= 0 && y < height)
        {
            grid[x, y].IsWalkable = walkable;
        }
    }

    public List<PathNode> FindPath(int startX, int startY, int goalX, int goalY)
    {
        // Reset all nodes
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                grid[x, y].Reset();
            }
        }

        PathNode startNode = grid[startX, startY];
        PathNode goalNode = grid[goalX, goalY];

        if (!startNode.IsWalkable || !goalNode.IsWalkable)
        {
            return null;  // Invalid start or goal
        }

        var openSet = new SortedSet<PathNode>(new NodeComparer());
        var closedSet = new HashSet<PathNode>();

        startNode.GCost = 0;
        startNode.HCost = CalculateHeuristic(startNode, goalNode);
        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            PathNode current = openSet.Min;
            openSet.Remove(current);

            if (current == goalNode)
            {
                return ReconstructPath(goalNode);
            }

            closedSet.Add(current);

            foreach (PathNode neighbor in current.GetNeighbors(grid))
            {
                if (!neighbor.IsWalkable || closedSet.Contains(neighbor))
                {
                    continue;
                }

                float movementCost = GetMovementCost(current, neighbor);
                float tentativeGCost = current.GCost + movementCost;

                if (tentativeGCost < neighbor.GCost)
                {
                    neighbor.Parent = current;
                    neighbor.GCost = tentativeGCost;
                    neighbor.HCost = CalculateHeuristic(neighbor, goalNode);

                    if (!openSet.Contains(neighbor))
                    {
                        openSet.Add(neighbor);
                    }
                }
            }
        }

        return null;  // No path found
    }

    private float CalculateHeuristic(PathNode from, PathNode to)
    {
        // Octile distance for 8-directional movement
        int dx = Mathf.Abs(from.X - to.X);
        int dy = Mathf.Abs(from.Y - to.Y);

        float straightCost = 1.0f;
        float diagonalCost = 1.414f; // sqrt(2)

        return straightCost * (dx + dy) + (diagonalCost - 2 * straightCost) * Mathf.Min(dx, dy);
    }

    private float GetMovementCost(PathNode from, PathNode to)
    {
        // Diagonal movement costs more than straight
        bool isDiagonal = from.X != to.X && from.Y != to.Y;
        return isDiagonal ? 1.414f : 1.0f;
    }

    private List<PathNode> ReconstructPath(PathNode goalNode)
    {
        var path = new List<PathNode>();
        PathNode current = goalNode;

        while (current != null)
        {
            path.Add(current);
            current = current.Parent;
        }

        path.Reverse();
        return path;
    }

    private class NodeComparer : IComparer<PathNode>
    {
        public int Compare(PathNode a, PathNode b)
        {
            int compare = a.FCost.CompareTo(b.FCost);
            if (compare == 0)
            {
                compare = a.HCost.CompareTo(b.HCost);
            }
            if (compare == 0)
            {
                // Ensure different nodes aren't considered equal
                compare = (a.X * 10000 + a.Y).CompareTo(b.X * 10000 + b.Y);
            }
            return compare;
        }
    }
}
