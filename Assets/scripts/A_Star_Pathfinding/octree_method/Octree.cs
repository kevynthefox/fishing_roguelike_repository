using UnityEngine;

//code from this tutorial: https://www.youtube.com/watch?v=gNmPmWR2vV4
namespace Octrees
{
    public class Octree
    {
        public OctreeNode root;
        public Bounds bounds;

        public Octree(GameObject[] worldObjects, float minNodeSize)
        {
            CalculateBounds(worldObjects);
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