using System.Collections.Generic;
using UnityEngine;

//code from this tutorial: https://www.youtube.com/watch?v=gNmPmWR2vV4
namespace Octrees
{
    public class Octree
    {
        public OctreeNode root;
        public Bounds bounds;

        private List<OctreeNode> emptyLeaves = new();

        public Octree(GameObject[] worldObjects, float minNodeSize)
        {
            CalculateBounds(worldObjects);
            CreateTree(worldObjects, minNodeSize);
            GetEmptyLeaves(root);
        }

        void GetEmptyLeaves(OctreeNode node)
        {
            
        }

        void CreateTree(GameObject[] worldObjects, float minNodeSize)
        {
            root = new OctreeNode(bounds, minNodeSize);

            foreach (var obj in worldObjects)
            {
                root.Divide(obj);
            }
        }

        void CalculateBounds(GameObject[] worldObjects)
        {
            foreach (var obj in worldObjects)
            {
                bounds.Encapsulate(obj.GetComponent<Collider>().bounds);
            }
            
            Vector3 size = Vector3.one * Mathf.Max(bounds.size.x, bounds.size.y,bounds.size.z) * 0.5f;
            bounds.SetMinMax(bounds.center - size, bounds.center + size);
        }
    }
    
    
}