using System.Collections;
using GDK;
using UnityEngine;

public class projectile_controller : MonoBehaviour, IPoolable
{
    public float damage;
    public float speed;

    public bool destroy_or_recycle; //destroy is false, recycle is true.

    private ObjectPoolSO pool;
    private Rigidbody rb;

    public float lifespan;


    public bool frag_piece;
    public bool is_frag_master;
    public GameObject frag_master;
    public GameObject[] frag_pieces;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public IEnumerator OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
        yield return new WaitForSeconds(0.1f);

        if (collision.gameObject.name != this.gameObject.name)
        {

            if (destroy_or_recycle == false)
            {

                Destroy(this.gameObject);
            }
            else
            {
                Release();
            }
        }
        
    }

    /*private void OnEnable()
    {
        StartCoroutine(failsafe_disable());
    }*/

    /*private IEnumerator failsafe_disable()
    {
        Debug.Log("enabled");
        yield return new WaitForSeconds(1f);
        this.gameObject.SetActive(false);
    }*/

    public void shoot(Vector3 force)
    {
        rb.AddForce(force * speed, ForceMode.Impulse);

        if (is_frag_master == true)
        {
            foreach (GameObject piece in frag_pieces)
            {
                piece.GetComponent<projectile_controller>().rb.AddForce(force * speed, ForceMode.Impulse);
            }
        }
    }

    public void Reset_momentum()
    {
        rb.angularVelocity = Vector3.zero;
        rb.linearVelocity = Vector3.zero;

        if (is_frag_master == true)
        {
            foreach (GameObject piece in frag_pieces)
            {
                piece.GetComponent<Transform>().position = frag_master.transform.position;

                //piece.GetComponent<projectile_controller>().rb.angularVelocity = Vector3.zero;
                //piece.GetComponent<projectile_controller>().rb.linearVelocity = Vector3.zero;
                //Debug.Log("reset this piece: " + piece.gameObject.name);
            }
        }
        Invoke(nameof(Release), lifespan);
    }

    public void Release()
    {
        //Debug.LogWarning(message:"Projectile Release", context: this);
        CancelInvoke(); // bad
        
        if (frag_piece == false)
        {
            pool.Release(this.gameObject);
        }
    }

    public void RegisterPool(ObjectPoolSO pool)
    {
        this.pool = pool;
    }

    public void OnPoolObjectCreate() { }
    public void OnPoolObjectTake() { }
    public void OnPoolObjectRelease() { }
    public void OnPoolObjectDestroy() { }
}
