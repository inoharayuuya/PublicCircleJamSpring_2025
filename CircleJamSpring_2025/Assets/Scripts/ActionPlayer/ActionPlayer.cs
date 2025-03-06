using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ActionPlayer : PlayerBace
{
    [Tooltip("���x")] public float speed;
    [Tooltip("HP")]   public float healthPoint;
    [Tooltip("Lv")]   public int   level;




    GameObject child;
    public bool attackDelay;
    public bool mutekiFlag;




    // Start is called before the first frame update
    void Start()
    {
        InitActionPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        CheckKey();
        Move();
        Attack();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        print("�ՓˊJ�n");
        if (collision.tag == "Obstacle")
        {
            collision.gameObject.GetComponent<Obstacle>().isFirst = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        print("�Փ˒�");
        if (collision.tag == "Obstacle" && mutekiFlag == false)
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
        print("�ՓˏI��");
        if (collision.tag == "Obstacle")
        {
            collision.gameObject.GetComponent<Obstacle>().isFirst = false;
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        print("�ڒn");
        if (collision.gameObject.tag == "Ground")
        {
            isInSky = false;
            jumpLimit = 1;
        }
    }


    void InitActionPlayer()
    {
        rb = GetComponent<Rigidbody2D>();
        child = gameObject.transform.GetChild(0).gameObject;

        jumpAmount = 15f;
        flightTime = 1f;

    }


    void Move()
    {
        // �ړ�����
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
        // �U������
        if (Input.GetMouseButtonDown(0) && attackDelay == false)
        {
            child.gameObject.transform.parent = null;
            attackDelay = true;
            print("�U��");

            StartCoroutine(NailAttack());
        }
    }

    IEnumerator NailAttack()
    {
        child.SetActive(true);
        // �U������


        yield return new WaitForSeconds(0.5f);
        child.SetActive(false);
        child.gameObject.transform.parent = gameObject.transform;
        child.transform.localPosition = new Vector2(1f, 0);
        yield return new WaitForSeconds(0.5f);
        attackDelay = false;
    }



    void UseSkill()
    {
        // �X�L������
    }


    void SelectSkill()
    {
        //  �X�L���I��
    }


    public float GetJumpAmount()
    {
        return jumpAmount;
    }

    public void StratJump(float jumpAmount)
    {
        Jump(jumpAmount);
    }

    public Rigidbody2D GetRigidBody()
    {
        return rb;
    }


}
