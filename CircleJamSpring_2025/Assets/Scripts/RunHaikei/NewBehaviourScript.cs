using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : PlayerBace
{
    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Jump();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        print("ê⁄ín");
        if (collision.gameObject.tag == "Ground")
        {
            isInSky = false;
            jumpLimit = 1;
        }
    }
}
