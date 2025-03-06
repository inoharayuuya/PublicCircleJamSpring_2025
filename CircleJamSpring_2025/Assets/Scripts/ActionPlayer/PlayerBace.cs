using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBace : MonoBehaviour
{

    protected float jumpAmount;
    protected float flightTime;
    protected bool isInSky;


    protected Rigidbody2D rb;
    protected int jumpLimit;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    protected void CheckKey()
    {
        // キーが押されたときジャンプ
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump(jumpAmount);
        }
    }

    protected void Jump(float jumpAmount)
    {
        if (isInSky == false)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpAmount);
            isInSky = true;
        }
        else if (rb.velocity.magnitude != 0 && isInSky == true && jumpLimit == 1)
        {
            jumpLimit = 0;
            StartCoroutine(StayingDown());
        }
    }



    IEnumerator StayingDown()
    {
        print("滞空開始");
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(rb.velocity.x, 0);
        yield return new WaitForSeconds(1);
        rb.gravityScale = 2.0f;
        print("滞空終了");
    }

}
