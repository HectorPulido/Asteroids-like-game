using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{ 

    private Rigidbody2D rb;

    public float speed;




    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.drag = 0;
        rb.angularDrag = 0;



        rb.velocity = new Vector3 (
            Random.Range (-1f, 1f),
            Random.Range (-1, 1f),
            0
            ).normalized * speed;



        rb.angularVelocity = Random.Range (-50f, 50f);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
