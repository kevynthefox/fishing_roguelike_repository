using UnityEngine;

public class debugforward : MonoBehaviour
{
    public bool display;
    public bool display_reverse;
    public bool move;

    void Update()
    {
        if (move)
        {
            transform.Translate(transform.forward * Time.deltaTime);
        }

        if (display)
        {
            Debug.DrawLine(transform.position, Vector3.forward * 10, color: Color.purple);
            Debug.DrawLine(transform.position, transform.forward * 10, color: Color.green);

            Debug.DrawRay(transform.position, Vector3.forward * 10, color: Color.orange);
            Debug.DrawRay(transform.position, transform.forward * 10, color: Color.red);
        }

        if (display_reverse)
        {
            Debug.DrawLine(transform.position, -Vector3.forward * 10, color: Color.purple);
            Debug.DrawLine(transform.position, -transform.forward * 10, color: Color.green);

            Debug.DrawRay(transform.position, -Vector3.forward * 10, color: Color.orange);
            Debug.DrawRay(transform.position, -transform.forward * 10, color: Color.red);
        }
    }
}
