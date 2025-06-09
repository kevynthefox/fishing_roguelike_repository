using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class object_pooler : MonoBehaviour
{
    //lets you access this from any script or whatever
    public static object_pooler instance;

    private Dictionary<GameObject, Queue<GameObject>> poolDictionary = new Dictionary<GameObject, Queue<GameObject>>();
    private Dictionary<GameObject, GameObject> pooledObjectOrigin = new Dictionary<GameObject, GameObject>();

    [Header("currency to initialize")]
    [SerializeField] private GameObject thing;
    [SerializeField] private int poolSize;


    //destroys all other object pools in the game
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }    
    }

    private void Start()
    {
        InitializeNewPool(thing);
    }

    public GameObject GetObject(GameObject prefab, Transform target)
    {
        if(poolDictionary.ContainsKey(prefab) == false)
        {
            InitializeNewPool(prefab);
        }

        GameObject objectToGet = poolDictionary[prefab].Dequeue();

        if (poolDictionary[prefab].Count == 0) //creates new object if there are not enough
        {
            CreateNewObject(prefab);
        }
        objectToGet.transform.parent = null;
        objectToGet.transform.position = target.position;
        objectToGet.SetActive(true);

        return objectToGet;
    }

    public void ReturnObject(GameObject objectToReturn, float delay = 0.001f)
    {
        StartCoroutine(ReturnToPool(objectToReturn, delay));
    }


    private void InitializeNewPool(GameObject prefab)
    {
        for (int i = 0; i < poolSize; i++)
        {
            CreateNewObject(prefab);
        }
    }

    private void CreateNewObject(GameObject prefab)
    {
        GameObject newObject = Instantiate(prefab, transform);
        newObject.SetActive(false);

        poolDictionary[prefab].Enqueue(newObject);
        pooledObjectOrigin[newObject] = prefab;
    }

    private IEnumerator ReturnToPool(GameObject objectToReturn, float delay)
    {
        yield return new WaitForSeconds(delay);

        GameObject originalPrefab = pooledObjectOrigin[objectToReturn];

        objectToReturn.SetActive(false);
        objectToReturn.transform.parent = transform;

        poolDictionary[originalPrefab].Enqueue(objectToReturn);
    }

    //script from this video
    //https://www.youtube.com/watch?v=B86sH_II3MY
}
