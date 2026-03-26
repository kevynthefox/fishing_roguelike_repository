using System.Collections.Generic;
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
            
            var edge = new Edge(nodeA, nodeB);
            if (edges.Add(edge))
            {
                nodeA.edges.Add(edge);
                nodeB.edges.Add(edge);
            }
        }

        Node FindNode(OctreeNode octreeNode)
        {
            nodes.TryGetValue(octreeNode, out Node node);
            return node;
        }
    }
}