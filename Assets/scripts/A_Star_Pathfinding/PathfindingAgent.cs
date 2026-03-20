using UnityEngine;
using System.Collections.Generic;

// tutorial from https://generalistprogrammer.com/tutorials/a-star-pathfinding-algorithm-complete-tutorial
public class PathfindingAgent : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5.0f;
    public float rotationSpeed = 10.0f;
    public float pathUpdateInterval = 0.5f;
    public float waypointReachDistance = 0.5f;

    [Header("Target")]
    public Transform target;

    private PathfindingGrid grid;
    private List<Vector3> currentPath;
    private int currentWaypointIndex = 0;
    private float lastPathUpdateTime;

    private void Start()
    {
        grid = FindObjectOfType<PathfindingGrid>();
        if (grid == null)
        {
            Debug.LogError("PathfindingGrid not found in scene!");
        }
    }

    private void Update()
    {
        if (target == null || grid == null) return;

        // Update path periodically
        if (Time.time - lastPathUpdateTime > pathUpdateInterval)
        {
            UpdatePath();
            lastPathUpdateTime = Time.time;
        }

        // Move along path
        if (currentPath != null && currentPath.Count > 0)
        {
            FollowPath();
        }
    }

    private void UpdatePath()
    {
        currentPath = grid.FindPath(transform.position, target.position);
        currentWaypointIndex = 0;
    }

    private void FollowPath()
    {
        if (currentWaypointIndex >= currentPath.Count) return;

        Vector3 targetWaypoint = currentPath[currentWaypointIndex];
        Vector3 direction = (targetWaypoint - transform.position).normalized;

        // Move toward waypoint
        transform.position += direction * moveSpeed * Time.deltaTime;

        // Rotate toward movement direction
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation,
                                                   rotationSpeed * Time.deltaTime);
        }

        // Check if reached waypoint
        float distanceToWaypoint = Vector3.Distance(transform.position, targetWaypoint);
        if (distanceToWaypoint < waypointReachDistance)
        {
            currentWaypointIndex++;
        }
    }

    private void OnDrawGizmos()
    {
        if (currentPath == null || currentPath.Count == 0) return;

        Gizmos.color = Color.cyan;
        for (int i = 0; i < currentPath.Count - 1; i++)
        {
            Gizmos.DrawLine(currentPath[i], currentPath[i + 1]);
        }
    }
}