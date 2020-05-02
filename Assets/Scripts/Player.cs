using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    private Rigidbody2D rb;
    public float acceleration;
    public float maxSpeed;
    public float inertia;
    public float angularSpeed;
    public GameObject bulletPrefab;
    public Transform bulletSpawner;

    // Start is called before the first frame update
    private void Start () {
        rb = GetComponent<Rigidbody2D> ();
        rb.drag = inertia;
    }

    float vertical;
    float horizontal;

    private void Update () {
        vertical = InputManager.Vertical;
        horizontal = InputManager.Horizontal;

        Rotate ();
        Shoot ();
    }

    private void Shoot () {
        if (InputManager.FireDown) {
            var bullet = Instantiate (
                bulletPrefab,
                bulletSpawner.position,
                bulletSpawner.rotation
            );
            Destroy (bullet, 5);
        }
    }

    private void Rotate () {
        if (horizontal == 0) {
            return;
        }
        transform.Rotate (0, 0, -angularSpeed * horizontal * Time.deltaTime);
    }

    // Update is called once per frame
    private void FixedUpdate () {
        rb.AddForce (transform.up * acceleration * vertical);
        if (rb.velocity.magnitude > maxSpeed) {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
    }

    public void Lose () {
        rb.velocity = Vector3.zero;
        transform.position = Vector3.zero;
    }
}