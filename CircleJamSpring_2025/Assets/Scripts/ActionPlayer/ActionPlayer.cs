using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ActionPlayer : PlayerBace
{
    float speed;


    // Start is called before the first frame update
    void Start()
    {
        InitActionPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        Jump();
        Move();
        Attack();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        print("衝突開始");
        if (collision.tag == "Obstacle")
        {
            collision.gameObject.GetComponent<Obstacle>().isFirst = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        print("衝突中");
        if (collision.tag == "Obstacle")
        {
            collision.gameObject.GetComponent<Obstacle>().Amount();
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        print("衝突終了");
        if (collision.tag == "Obstacle")
        {
            collision.gameObject.GetComponent<Obstacle>().isFirst = false;
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        print("接地");
        if (collision.gameObject.tag == "Ground")
        {
            isInSky = false;
            jumpLimit = 1;
        }
    }


    void InitActionPlayer()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    void Move()
    {
        // 移動処理
        if (Input.GetKey(KeyCode.D))
        {
            rb.velocity = new Vector2(3f, rb.velocity.y);
        }
        if (Input.GetKey(KeyCode.A))
        {
            rb.velocity = new Vector2(-3f, rb.velocity.y);
        }
    }


    void Attack()
    {
        print("攻撃");
        // 攻撃処理
        if (Input.GetMouseButtonDown(0))
        {
            
        }

    }


    void UseSkill()
    {
        // スキル発動
    }


    void SelectSkill()
    {
        //  スキル選択
    }


}
