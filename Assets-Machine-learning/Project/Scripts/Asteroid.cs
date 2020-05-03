using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour {
    private Rigidbody2D rb;
    public float speed;

    public GameObject[] subAsteroids;
    public int numberOfAsteroids;

    // Start is called before the first frame update
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

    private void OnTriggerEnter2D (Collider2D col) {
        if (col.CompareTag ("Bullet")) {
            Destroy (gameObject);
            Destroy (col.gameObject);
            for (var i = 0; i < numberOfAsteroids; i++) {
                Instantiate (
                    subAsteroids[Random.Range (0, subAsteroids.Length)],
                    transform.position,
                    Quaternion.identity
                );
            }
        }
        if (col.CompareTag ("Player")) {
            var asteroids = FindObjectsOfType<Asteroid>();
            for(var i = 0; i < asteroids.Length; i++) {
                Destroy(asteroids[i].gameObject);
            }
            var bullets = GameObject.FindGameObjectsWithTag("Bullet");
            for(var i = 0; i < bullets.Length; i++) {
                Destroy(bullets[i]);
            }
            col.GetComponent<ShipPlayer>().Lose();
        }
    }

}