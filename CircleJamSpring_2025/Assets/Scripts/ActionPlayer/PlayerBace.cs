using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBace : MonoBehaviour
{

    protected float jumpAmount;
    protected float flightTime;
    protected bool isInSky;
    protected bool isMove;


    protected Rigidbody2D rb;
    protected int jumpLimit;


    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
    }

    protected void Init()
    {
        isMove = true;
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
            StartCoroutine(StayingDown(1));
        }
    }



    protected IEnumerator StayingDown(float amount)
    {   
        print("滞空開始");
        isMove = false;
        rb.gravityScale = 0f;
        rb.velocity = Vector3.zero;
        var speed = 7.5f;
        rb.velocity = transform.right * speed * amount;
        yield return new WaitForSeconds(1);
        rb.gravityScale = 2.0f;
        isMove = true;
        print("滞空終了");
    }

}
