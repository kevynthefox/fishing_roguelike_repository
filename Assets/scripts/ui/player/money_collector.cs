using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]

public class money_collector : MonoBehaviour
{
    public Text money;
    public float money_value;
    public GameObject self;

    //public bool collectible;

    private GameObject money_spawner;
    private GameObject money_spawner_island;
    private GameObject bobber;

    public float wait;

    private void Start()
    {
        money_spawner = GameObject.Find("money_touch_spawner");
        money_spawner_island = GameObject.Find("money_island_spawner");
        bobber = GameObject.Find("bobber (1)");

        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];

        int i = 0;
        while (i < meshFilters.Length)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            meshFilters[i].gameObject.SetActive(false);

            i++;
        }

        Mesh mesh = new Mesh();
        mesh.CombineMeshes(combine);
        transform.GetComponent<MeshFilter>().sharedMesh = mesh;
        transform.gameObject.SetActive(true);
    }

    public void Update()
    {
        money.text = ":" + money_value;

        //collectible = money_spawner.GetComponent<fish_seller_checkout>().collectible;
    }

    public IEnumerator OnTriggerEnter(Collider other)
    {
        //if (collectible == true)
        //{

        if (other.gameObject.tag == "currency")
        {
            money_value += other.GetComponent<money_value_holder>().value;
            money.text = ":" + money_value;

            //Debug.Log(money_spawner_island.transform.position.y);

            //Destroy(other.gameObject);
            wait = (money_spawner_island.transform.position.y / 25);
            //yield return new WaitForSeconds(1/money_spawner.GetComponent<fish_seller_checkout>().money_owed);
            yield return new WaitForSeconds(wait);

            if (other.attachedRigidbody != null)
            {
                Destroy(other.attachedRigidbody);
            }
            if (other != null)
            {
                Destroy(other);
            }


            other.transform.parent = transform;

            //other.gameObject.isStatic = true;

            MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
            CombineInstance[] combine = new CombineInstance[meshFilters.Length];

            int i = 0;
            while (i < meshFilters.Length)
            {
                combine[i].mesh = meshFilters[i].sharedMesh;
                combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
                //meshFilters[i].gameObject.SetActive(false);
                yield return new WaitForSeconds(1 / bobber.GetComponent<bobber_impact>().fish_quantity_original);
                i++;
            }

            Mesh mesh = new Mesh();
            mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
            mesh.CombineMeshes(combine);
            transform.GetComponent<MeshFilter>().sharedMesh = mesh;
            transform.gameObject.SetActive(true);
            
            other.GetComponent<money_value_holder>().enabled = false;
            //yield return new WaitForSeconds(1f);
            //yield return new WaitForSeconds(100 / money_spawner.GetComponent<fish_seller_checkout>().money_owed);
        }

        //}
    }

    /*public static void CombineInstance(GameObject staticBatchRoot);
    {
       staticBatchRoot.combine
    }*/
}
