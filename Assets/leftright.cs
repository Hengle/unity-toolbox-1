using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class leftright : MonoBehaviour
{
    public Vector3 vel;
    public float waitTime = 3f;

    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = vel;
        StartCoroutine(Flip());
    }

    IEnumerator Flip()
    {
        yield return new WaitForSeconds(waitTime);
        rb.velocity *= -1f;
        StartCoroutine(Flip());
    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }
}
