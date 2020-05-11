using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Player : NetworkBehaviour {
    private Rigidbody2D rb;
    public float acceleration;
    public float maxSpeed;
    public float inertia;
    public float angularSpeed;
    public float shootRate = 0.5f;
    public GameObject bulletPrefab;

    public float offsetBullet;

    private float vertical;
    private float horizontal;
    private bool shooting;
    private bool canShoot = true;

    private void Start () {
        rb = GetComponent<Rigidbody2D> ();
        rb.drag = inertia;
    }

    private void Update () {
        if (!isLocalPlayer) {
            return;
        }

        vertical = InputManager.Vertical;
        horizontal = InputManager.Horizontal;
        shooting = InputManager.Fire;

        Rotate ();
        Shoot ();
    }

    private void Shoot () {
        if (shooting) {
            CmdShoot ();
        }
    }

    [Command]
    public void CmdShoot () {
        if (canShoot) {
            StartCoroutine (FireRate ());
        }
    }

    private void Rotate () {
        if (horizontal == 0) {
            return;
        }
        transform.Rotate (0, 0, -angularSpeed * horizontal * Time.deltaTime);
    }

    private void FixedUpdate () {
        var forwardMotor = Mathf.Clamp (vertical, 0f, 1f);
        rb.AddForce (transform.up * acceleration * forwardMotor);
        if (rb.velocity.magnitude > maxSpeed) {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
    }

    void Lose () {
        var nsp = FindObjectsOfType<NetworkStartPosition> ();
        var pos = nsp[Random.Range (0, nsp.Length)].transform.position;

        rb.velocity = Vector3.zero;
        transform.position = pos;
    }

    private void OnTriggerEnter2D (Collider2D col) {
        if (!isLocalPlayer) {
            return;
        }
        if (col.CompareTag ("Bullet") || col.CompareTag ("Obstacle")) {
            Destroy (col.gameObject);
            Lose ();
        }
    }

    [ServerCallback]
    private IEnumerator FireRate () {
        canShoot = false;
        var pos = transform.up * offsetBullet + transform.position;
        var bullet = Instantiate (
            bulletPrefab,
            pos, 
            transform.rotation
        );

        NetworkServer.Spawn (bullet);
        Destroy (bullet, 5);
        yield return new WaitForSeconds (shootRate);
        canShoot = true;
    }
}