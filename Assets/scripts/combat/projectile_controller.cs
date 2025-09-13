using System.Collections;
using GDK;
using UnityEngine;

public class projectile_controller : MonoBehaviour, IPoolable
{
    public float damage;
    public float speed;
    public float s_to_m;

    public bool destroy_or_recycle; //destroy is false, recycle is true.

    private ObjectPoolSO pool;
    private Rigidbody rb;

    public float lifespan;


    public bool frag_piece;
    public bool is_frag_master;
    public GameObject frag_master;
    public GameObject[] frag_pieces;

    public bool explosive;

    public bool touched;

    public string[] tags_to_ignore;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        s_to_m = speed / rb.mass;
    }

    

    public IEnumerator OnCollisionEnter(Collision collision)
    {
        touched = true;
        //Debug.Log(this.gameObject.name + " collided with 1 " + collision.gameObject.name);
        yield return new WaitForSeconds(1f);



        


        
    }

    private IEnumerator OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == tags_to_ignore[0] || collision.gameObject.tag == tags_to_ignore[1] || collision.gameObject.tag == tags_to_ignore[2])
        {

        }
        else
        {
            //Debug.Log(this.gameObject.name + " collided with 2  " + collision.gameObject.name);



            if (destroy_or_recycle == false)
            {

                Destroy(this.gameObject);
            }
            else
            {
                if (explosive == true)
                {
                    //Debug.Log("boom");
                    this.gameObject.GetComponent<SphereCollider>().enabled = true;
                    yield return new WaitForSeconds(.1f);
                    this.gameObject.GetComponent<SphereCollider>().enabled = false;
                }
                Release();
            }
        }
    }

    public void OnCollisionExit(Collision collision)
    {
        touched = false;
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
        //Debug.Log("resetting momentum");
        if (lifespan != 0)
        {
            Invoke(nameof(Release), lifespan);
        }
    }

    public void Release()
    {
        //Debug.Log("released");
        //Debug.LogWarning(message:"Projectile Release", context: this);
        //Debug.Log("released");
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
