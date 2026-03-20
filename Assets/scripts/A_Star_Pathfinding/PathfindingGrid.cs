using UnityEngine;
using System.Collections.Generic;

// tutorial from https://generalistprogrammer.com/tutorials/a-star-pathfinding-algorithm-complete-tutorial
public class PathfindingGrid : MonoBehaviour
{
    [Header("Grid Settings")]
    public int gridWidth = 50;
    public int gridHeight = 50;
    public float nodeSize = 1.0f;
    public LayerMask unwalkableMask;

    [Header("Debugging")]
    public bool displayGridGizmos = true;
    public Color walkableColor = Color.white;
    public Color unwalkableColor = Color.red;
    public Color pathColor = Color.green;

    private AStarPathfinder pathfinder;
    private Vector3 gridWorldSize;
    private Vector3 gridBottomLeft;
    private List<PathNode> currentPath;

    private void Start()
    {
        gridWorldSize = new Vector3(gridWidth * nodeSize, 0, gridHeight * nodeSize);
        gridBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2
                                              - Vector3.forward * gridWorldSize.z / 2;

        pathfinder = new AStarPathfinder(gridWidth, gridHeight);
        GenerateGrid();
    }

    private void GenerateGrid()
    {
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                Vector3 worldPoint = gridBottomLeft + Vector3.right * (x * nodeSize + nodeSize / 2)
                                                      + Vector3.forward * (y * nodeSize + nodeSize / 2);

                bool walkable = !Physics.CheckSphere(worldPoint, nodeSize / 2, unwalkableMask);
                pathfinder.SetWalkable(x, y, walkable);
            }
        }
    }

    public List<Vector3> FindPath(Vector3 startWorldPos, Vector3 endWorldPos)
    {
        PathNode startNode = WorldPointToGridNode(startWorldPos);
        PathNode endNode = WorldPointToGridNode(endWorldPos);

        if (startNode == null || endNode == null)
        {
            Debug.LogWarning("Start or end position is outside grid bounds");
            return null;
        }

        currentPath = pathfinder.FindPath(startNode.X, startNode.Y, endNode.X, endNode.Y);

        if (currentPath == null || currentPath.Count == 0)
        {
            return null;
        }

        List<Vector3> worldPath = new List<Vector3>();
        foreach (PathNode node in currentPath)
        {
            worldPath.Add(GridNodeToWorldPoint(node));
        }

        return worldPath;
    }

    private PathNode WorldPointToGridNode(Vector3 worldPosition)
    {
        float percentX = (worldPosition.x - gridBottomLeft.x) / gridWorldSize.x;
        float percentY = (worldPosition.z - gridBottomLeft.z) / gridWorldSize.z;

        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.FloorToInt(percentX * gridWidth);
        int y = Mathf.FloorToInt(percentY * gridHeight);

        x = Mathf.Clamp(x, 0, gridWidth - 1);
        y = Mathf.Clamp(y, 0, gridHeight - 1);

        return pathfinder.GetNode(x, y);
    }

    private Vector3 GridNodeToWorldPoint(PathNode node)
    {
        return gridBottomLeft + Vector3.right * (node.X * nodeSize + nodeSize / 2)
                               + Vector3.forward * (node.Y * nodeSize + nodeSize / 2);
    }

    private void OnDrawGizmos()
    {
        if (!displayGridGizmos) return;

        Gizmos.DrawWireCube(transform.position, gridWorldSize);

        if (pathfinder != null && Application.isPlaying)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                for (int y = 0; y < gridHeight; y++)
                {
                    PathNode node = pathfinder.GetNode(x, y);
                    Gizmos.color = node.IsWalkable ? walkableColor : unwalkableColor;

                    if (currentPath != null && currentPath.Contains(node))
                    {
                        Gizmos.color = pathColor;
                    }

                    Vector3 worldPos = GridNodeToWorldPoint(node);
                    Gizmos.DrawCube(worldPos, Vector3.one * (nodeSize - 0.1f));
                }
            }
        }
    }
}