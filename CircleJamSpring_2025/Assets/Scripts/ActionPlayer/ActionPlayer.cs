using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ActionPlayer : PlayerBace
{
    [Tooltip("速度")] public float speed;
    [Tooltip("HP")]   public float healthPoint;


    GameObject Child;
    public bool attackDelay;


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
            healthPoint = collision.gameObject.GetComponent<Obstacle>().Amount(healthPoint);
            if (collision.gameObject.GetComponent<Obstacle>().isObstacleDisabled == true)
            {
                Destroy(collision.gameObject);
            }
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
        Child = gameObject.transform.GetChild(0).gameObject;
    }


    void Move()
    {
        // 移動処理
        if (Input.GetKey(KeyCode.D))
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            rb.velocity = new Vector2(speed, rb.velocity.y);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
            rb.velocity = new Vector2(-speed, rb.velocity.y);
        }
    }


    void Attack()
    {
        // 攻撃処理
        if (Input.GetMouseButtonDown(0) && attackDelay == false)
        {
            attackDelay = true;
            print("攻撃");

            StartCoroutine(NailAttack());
        }
    }

    IEnumerator NailAttack()
    {
        Child.SetActive(true);
        // 攻撃処理


        yield return new WaitForSeconds(0.5f);
        Child.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        attackDelay = false;
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
