using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Asteroid : NetworkBehaviour {
    private Rigidbody2D rb;
    public float speed;

    public GameObject[] subAsteroids;
    public int numberOfAsteroids;

    [ServerCallback]
    private void Start () {
        rb = GetComponent<Rigidbody2D> ();
        rb.drag = 0;
        rb.angularDrag = 0;

        rb.velocity = new Vector3 (
            Random.Range (-1f, 1f),
            Random.Range (-1f, 1f),
            Random.Range (-1f, 1f)
        ).normalized * speed;

        rb.angularVelocity = Random.Range (-50f, 50f);
    }

    [ServerCallback]
    private void OnTriggerEnter2D (Collider2D col) {
        if (col.CompareTag ("Bullet")) {
            Destroy (gameObject);
            Destroy (col.gameObject);
            for (var i = 0; i < numberOfAsteroids; i++) {
                var subinstatiate = Instantiate (
                    subAsteroids[Random.Range (0, subAsteroids.Length)],
                    transform.position,
                    Quaternion.identity
                );
                NetworkServer.Spawn (subinstatiate);
            }
        }
    }

}