using System;
using System.Collections;
using System.Linq;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

//code from this tutorial: https://www.youtube.com/watch?v=gNmPmWR2vV4
namespace Octrees
{
    public class OctreeGenerator : MonoBehaviour
    {
        public GameObject[] objects;
        public float minNodeSize = 1f;
        public Octree ot;

        public readonly Graph waypoints = new();

        void Awake() => ot = new Octree(objects,minNodeSize,waypoints);

        private void Start()
        {
            this.GetComponent<BoxCollider>().center = ot.bounds.center - this.transform.position;
            this.GetComponent<BoxCollider>().size = ot.bounds.size;

            
        }

        private void OnTriggerStay(Collider other)
        {
            
        }

        

        void OnDrawGizmos()
        {
            if (!Application.isPlaying) return; //only run this code when the application is playing, to avoid issues with the awake function not being on or whatever
            
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(ot.bounds.center,ot.bounds.size);
            
            ot.root.DrawNode();
            ot.graph.DrawGraph();
        }
    }
}