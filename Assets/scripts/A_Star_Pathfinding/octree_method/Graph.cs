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

        Node FindNode(OctreeNode octreeNode)
        {
            nodes.TryGetValue(octreeNode, out Node node);
            return node;
        }
    }
}