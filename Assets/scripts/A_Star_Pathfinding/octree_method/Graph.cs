using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//code from this tutorial: https://www.youtube.com/watch?v=gNmPmWR2vV4
namespace Octrees
{
    public class Node
    {
        static int nextId;
        public readonly int id;

        public float f, g, h;
        public Node from;
        
        
        public List<Edge> edges = new();
        
        public OctreeNode octreeNode;

        public Node(OctreeNode octreeNode)
        {
            this.id = nextId++;
            this.octreeNode = octreeNode;
        }

        public override bool Equals(object obj) => obj is Node other && id == other.id;
        public override int GetHashCode() => id.GetHashCode();
    }

    public class Edge
    {
        public readonly Node a, b;

        public Edge(Node a, Node b)
        {
            this.a = a;
            this.b = b;
        }

        public override bool Equals(object obj)
        {
            return obj is Edge other && ((a == other.a && b == other.b) || (a == other.b && b == other.a));
        }
        
        public override int GetHashCode() => a.GetHashCode() ^ b.GetHashCode();
    }

    public class Graph
    {
        public readonly Dictionary<OctreeNode, Node> nodes = new();
        public readonly HashSet<Edge> edges = new();
        
        List<Node> pathList = new();

        public bool AStar(OctreeNode startNode, OctreeNode endNode)
        {
            pathList.Clear();
            Node start = FindNode(startNode);
            Node end = FindNode(endNode);
            
            if (start == null || end == null)
            {
                Debug.LogError("Start or End node not found in the graph.");
                return false;
            }

            SortedSet<Node> openSet = new(new NodeComparer()); //least expensive nodes to travel through will always be the first ones we get out.
            HashSet<Node> closedSet = new();
            int iterationCount = 0;

            start.g = 0;
            start.h = Heuristic(start, end);
            start.f = start.g + start.h;
            start.from = null;
            openSet.Add(start);

            while (openSet.Count > 0)
            {
                if (++iterationCount > maxIterations)
                {
                    Debug.LogError("A* exceeded maximum iterations");
                    return false;
                }

                Node current = openSet.First();
                openSet.Remove(current);

                if (current.Equals(end))
                {
                    ReconstructPath(current);
                    return true;
                }
                
                closedSet.Add(current);
                foreach (Edge edge in current.edges)
                {
                    Node neighbor = Equals(edge.a, current) ? edge.b : edge.a;

                    if (closedSet.Contains(neighbor)) continue;
                }
            }
        }
        
        float Heuristic(Node a, Node b) => (a.octreeNode.bounds.center - b.octreeNode.bounds.center).sqrMagnitude;

        public class NodeComparer : IComparer<Node>
        {
            public int Compare(Node x, Node y)
            {
                if (x == null || y == null) return 0;

                int compare = x.f.CompareTo(y.f); // f value represents total estimated cost of a path that passes through a given node.
                if (compare == 0) //if 2 nodes have the same f value, fall back on id.
                {
                    return x.id.CompareTo(y.id);
                }

                return compare;
            }
        }
        
        public void AddNode(OctreeNode octreeNode)
        {
            if (!nodes.ContainsKey(octreeNode))
            {
                nodes.Add(octreeNode,new Node(octreeNode));
            }
        }

        public void AddEdge(OctreeNode a, OctreeNode b)
        {
            Node nodeA = FindNode(a);
            Node nodeB = FindNode(b);

            if (nodeA == null || nodeB == null) return;
            
            var edge = new Edge(nodeA, nodeB);
            if (edges.Add(edge))
            {
                nodeA.edges.Add(edge);
                nodeB.edges.Add(edge);
            }
        }
        
        public void DrawGraph()
        {
            Gizmos.color = Color.red; 
            foreach (Edge edge in edges)
            {
                Gizmos.DrawLine(edge.a.octreeNode.bounds.center, edge.b.octreeNode.bounds.center);
            }
            
            foreach (var node in nodes.Values)
            {
                Gizmos.DrawWireSphere(node.octreeNode.bounds.center, 0.2f);
            }
        }

        Node FindNode(OctreeNode octreeNode)
        {
            nodes.TryGetValue(octreeNode, out Node node);
            return node;
        }
    }
}