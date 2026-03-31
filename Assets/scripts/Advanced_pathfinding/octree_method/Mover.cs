using System;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

//code from this tutorial: https://www.youtube.com/watch?v=gNmPmWR2vV4
namespace Octrees
{
    public class Mover : MonoBehaviour
    {
        public float speed = 5f;
        float accuracy = 1f; //how close we have to get to a target waypoint.
        public float turnSpeed = 5f;
        
        int currentWaypoint;
        OctreeNode currentNode;
        Vector3 destination;
        
        public OctreeGenerator octreeGenerator;
        Graph graph;


        public Transform target;

        void Start() //start because octree generator runs in awake, so by start it should have some waypoints created.
        {
            graph = octreeGenerator.waypoints;
            currentNode = GetClosestNode(transform.position);
            //GetRandomDestination(); // this one is for testing.
            //GetTargetDestination();
        }

        void Update() //might wanna replace later
        {
            if (graph == null) return; //if graph is null, bail

            if (graph.GetPathLength() == 0 || currentWaypoint >= graph.GetPathLength())
            { //if path length is 0 or we've exceeded our waypoints, get a new random destination.
                //GetRandomDestination();
                GetTargetDestination();
                return;
            }

            if (Vector3.Distance(graph.GetPathNode(currentWaypoint).bounds.center, transform.position) < accuracy)
            { //if still traveling along graph, check if close enough to current waypoint by checking against accuracy
                currentWaypoint++;//if we are, increment waypoint to start moving to the next one.
                Debug.Log($"Waypoint {currentWaypoint} reached");

            }
            //provided that the waypoint is still within the path, start moving towards it.
            if (currentWaypoint < graph.GetPathLength())
            {
                currentNode = graph.GetPathNode(currentWaypoint);
                destination = currentNode.bounds.center;

                Vector3 direction = destination - transform.position;
                direction.Normalize();
                
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), turnSpeed * Time.deltaTime);
                transform.Translate(0,0, speed * Time.deltaTime);
            }
            else //if we didn't do all of that, get another destination, but this is reduntant because it'll get picked up on the next frame anyway
            {
                //GetRandomDestination();
                GetTargetDestination();
            }
        }

        OctreeNode GetClosestNode(Vector3 position) // this is used to get any world position you want
        {
            return octreeGenerator.ot.FindClosestNode(transform.position);
        }

        void GetRandomDestination()
        {
            OctreeNode destinationNode;
            do //do while thing
            {
                destinationNode = graph.nodes.ElementAt(Random.Range(0, graph.nodes.Count)).Key;
                Debug.Log("destinationNode: " + destinationNode.id);
            } while (!graph.AStar(currentNode, destinationNode)); // this is the part that plots the path there.
            currentWaypoint = 0;
        }

        void GetTargetDestination()
        {
            OctreeNode destinationNode;
            do //do while thing
            {
                destinationNode = graph.nodes.ElementAt(GetClosestNode(target.position).id).Key;
                //destinationNode = GetClosestNode(target.position);
                Debug.Log("destinationNode: " + destinationNode.id);
            } while (!graph.AStar(currentNode, destinationNode)); // this is the part that plots the path there.
            currentWaypoint = 0;
        }
        
        void OnDrawGizmos()
        {
            if (graph == null || graph.GetPathLength() == 0) return; //if no path or length is 0, get out of here.

            Gizmos.color = Color.red; //start node
            Gizmos.DrawWireSphere(graph.GetPathNode(0).bounds.center, 0.7f);

            Gizmos.color = Color.blue; //end node
            Gizmos.DrawWireSphere(graph.GetPathNode(graph.GetPathLength() -1).bounds.center,0.7f);
            
            Gizmos.color = Color.purple; //all of the connecting points between each of them.
            for (int i = 0; i < graph.GetPathLength(); i++)
            {
                Gizmos.DrawWireSphere(graph.GetPathNode(i).bounds.center, 0.5f);
                if (i < graph.GetPathLength() - 1) //draw a line between the start and the end of each segment
                {
                    Vector3 start = graph.GetPathNode(i).bounds.center;
                    Vector3 end = graph.GetPathNode(i + 1).bounds.center;
                    Gizmos.DrawLine(start, end);
                }
            }
            
        }
    }
}