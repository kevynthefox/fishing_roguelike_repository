using System.Collections.Generic;
using UnityEngine;

//code from this tutorial: https://www.youtube.com/watch?v=gNmPmWR2vV4
namespace Octrees
{
    public class OctreeNode : MonoBehaviour
    {
        public List<OctreeObject> objects = new();

        static int nextId;
        public readonly int id;

        public Bounds bounds;
        Bounds[] childBounds = new Bounds[8];
        public OctreeNode[] children;
        public bool IsLeaf => children == null; //if this is a leaf it means it has no children
        
        float minNodeSize;

        public OctreeNode(Bounds bounds, float minNodeSize)
        {
            id = nextId++;
            
            this.bounds = bounds;
            this.minNodeSize = minNodeSize;
            Vector3 newSize = bounds.size * 0.5f; //halved size
            Vector3 centerOffset = bounds.size * 0.25f; //quarter offset
            Vector3 parentCenter = bounds.center;

            for (int i = 0; i < 8; i++)
            {
                Vector3 childCenter = parentCenter;
                childCenter.x += centerOffset.x * ((i & 1) == 0 ? -1 : 1); //first bit controls if it's positively offset or negatively offset in the x direction
                childCenter.y += centerOffset.y * ((i & 2) == 0 ? -1 : 1);
                childCenter.z += centerOffset.z * ((i & 4) == 0 ? -1 : 1); //shouldn't this be 3 instead of 4!?
                childBounds[i] = new Bounds(childCenter, newSize);
                
                
                
            }
        }

        public void Divide(GameObject obj) => Divide(new OctreeObject(obj));

        void Divide(OctreeObject octreeObject)
        {
            if (bounds.size.x <= minNodeSize) //stop recursing when bounds size is less than min node size
            {
                AddObject(octreeObject);
                return;
            }
            
            children ??= new OctreeNode[8]; //if children array hasn't been initialized yet, initialize it at a size of 8

            bool intersectedChild = false; //you want to stop if the object is fully contained in the node. if it isn't, keep subdividing.

            for (int i = 0; i < 8; i++)
            {
                children[i] ??= new OctreeNode(childBounds[i], minNodeSize); //assign octree nodes into children

                if (octreeObject.Intersects(childBounds[i])) //if it intersects, keep dividing
                {
                    children[i].Divide(octreeObject);
                    intersectedChild = true;
                }
            }

            if (!intersectedChild) //if after all of the child nodes have been done, if it doesn't fit in any, add the object to this node
            {
                AddObject(octreeObject);
            }
        }
        
        void AddObject(OctreeObject octreeObject) => objects.Add(octreeObject);
        
        
        public void DrawNode()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(bounds.center,bounds.size);
            if (children != null)
            {
                foreach (OctreeNode child in children)
                {
                    if(child != null) child.DrawNode();
                }
            }
        }
    }
}