using System;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

//code from this tutorial: https://www.youtube.com/watch?v=gNmPmWR2vV4
namespace Octrees
{
    public class OctreeGenerator : MonoBehaviour
    {
        public GameObject[] objects;
        public float minNodeSize = 1f;
        Octree ot;

        void Awake() => ot = new Octree(objects,minNodeSize);

        void OnDrawGizmos()
        {
            if (!Application.isPlaying) return; //only run this code when the application is playing, to avoid issues with the awake function not being on or whatever
            
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(ot.bounds.center,ot.bounds.size);
            
            ot.root.DrawNode();
        }
    }
}