using UnityEngine;

//code from this tutorial: https://www.youtube.com/watch?v=gNmPmWR2vV4
namespace Octrees
{
    public class OctreeObject : MonoBehaviour
    {
        Bounds bounds;

        public OctreeObject(GameObject obj)
        {
            bounds = obj.GetComponent<Collider>().bounds;
        }
        
        public bool Intersects(Bounds boundsToCheck) => bounds.Intersects(boundsToCheck);
    }
}