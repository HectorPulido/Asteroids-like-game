using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    public float acceleration;
    public float maxSpeed;
    public float inertia;
    public float angularSpeed;
    public float shootRate = 0.5f;
    public GameObject bulletPrefab;
    public Transform bulletSpawner;

    private float vertical;
    private float horizontal;
    private bool shooting;
    private bool canShoot = true;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.drag = inertia;
    }

    private void Update()
    {
        vertical = InputManager.Vertical;
        horizontal = InputManager.Horizontal;
        shooting = InputManager.Fire;

        Rotate();
        Shoot();
    }

    private void Shoot()
    {
        if (shooting && canShoot)
        {
            StartCoroutine(FireRate());
        }
    }

    private void Rotate()
    {
        if (horizontal == 0)
        {
            return;
        }
        transform.Rotate(0, 0, -angularSpeed * horizontal * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        var forwardMotor = Mathf.Clamp(vertical, 0f, 1f);
        rb.AddForce(transform.up * acceleration * forwardMotor);
        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
    }

    public void Lose()
    {
        rb.velocity = Vector3.zero;
        transform.position = Vector3.zero;
    }

    private IEnumerator FireRate()
    {
        canShoot = false;
        var bullet = Instantiate(
            bulletPrefab,
            bulletSpawner.position,
            bulletSpawner.rotation
        );
        Destroy(bullet, 5);
        yield return new WaitForSeconds(shootRate);
        canShoot = true;
    }
}