using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Run_Player : MonoBehaviour
{
    Rigidbody2D rbody2D;
    public bool key = true;
    public float jump = 300;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector2(0, -2);
        rbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Playercn();
    }

    void Playercn()
    {
        if (Input.GetKeyDown(KeyCode.K) && key == true)
        {
            rbody2D.velocity = new Vector2(rbody2D.velocity.x, jump);
            key = false;
        }
        if (transform.position.y <= -2)
        {
            transform.position = new Vector2(0, -2);
            key = true;
        }
        if (transform.position.y >= -1.9)
        {
            key = false;
        }

    }
    void OnTriggerEnter2D(Collider2D other)
    {
        // Destroy the bullet on collision with the enemy
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("a");
        }
        if (other.CompareTag("Gole"))
        {
            Debug.Log("b");
        }
    }
}