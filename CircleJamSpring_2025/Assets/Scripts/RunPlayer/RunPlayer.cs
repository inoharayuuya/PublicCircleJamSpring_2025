using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class RunPlayer : PlayerBace
{
    public SpriteRenderer sp;                    //�_�ŗp
    public Soldier soldier;
    public GameObject sun;                       //���������z
    public Vector3 move = new Vector3(1, 0, 0);  //����������
    public float moveDistance = 1.0f;            //�ړ�����
    public float moveSpeed = 1.5f;               //�ړ����x�i���l������������Ƃ������j
    [SerializeField] float flashInterval;        //�ړ��X�s�[�h�Ɠ_�ł̊Ԋu
    [SerializeField] int loopCount;              //�_�ł�����Ƃ��̃��[�v�̃J�E���g
    public int moveCount;                        //���z�̓�������
    public GameObject runStage;
    public RunHaikei run_haikei;

    private Rigidbody2D playerRigidbody;
    private bool moved;                          //1�x�������t���O
    private bool isMoving;                       //�ړ����t���O
    private bool operation;                      //����\���̃t���O
    private bool hit;
    private float score = 0;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sp = GetComponent<SpriteRenderer>();
        InitObstacles();
    }
    void Update()
    {
        Jump();
    }

    public void InitObstacles()
    {
        moveCount = 0;     //���z�̓�������
        moved = false;     //1�x�������t���O
        isMoving = false;  //�ړ����t���O
        operation = false; //����\���̃t���O
        hit = false;       //�����Ă��鎞�i�_�Łj�p
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log("�Փˌ��m: " + gameObject.name); // �m�F�p���O
        if (collider.gameObject.CompareTag("Obstacles") && !moved && !isMoving)
        {
            moved = true;
            moveCount += 1;
            //Debug.Log(moveCount);
            moveDistance = 1;
            MoveSun();
            FlashPlayer(collider);
        }

        if (collider.gameObject.CompareTag("Soldier") && !moved)
        {
            moved = true;
            moveCount += 2;
            //Debug.Log(moveCount);
            moveDistance = 2;
            MoveSun();
            FlashPlayer(collider);
        }

        if (moveCount >= 4)
        {
            operation = true;
            Debug.Log("gameover");
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

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Obstacles") || collider.gameObject.CompareTag("Soldier"))
        {
            moved = false; //���ꂽ��t���O�����Z�b�g
        }
        
        if (collider.gameObject.CompareTag("Trigger"))
        {
            //score = run_haikei.distance * -1;
            if (score >= 500)
            {
                runStage.GetComponent<RunStage>().Goal();
            }
            else
            {
                runStage.GetComponent<RunStage>().RandomTwoPrefabs();
            }
        }
    }

    public void MoveSun()
    {
        //Debug.Log("sun: " + sun); // �m�F�p���O

        if (sun != null)//sun���ݒ肳��Ă��邩���`�F�b�N
        {
            /*if(moveCount == 1)
            {
                sun.transform.position += move * moveDistance;
            }*/

            StartCoroutine(MoveSunCoroutine());
            //Debug.Log("�ړ����܂���");
        }
        else
        {
            //Debug.LogError("�ݒ肳��Ă��Ȃ�");
        }
    }

    public IEnumerator MoveSunCoroutine()
    {
        isMoving = true; // �ړ����t���O�𗧂Ă�

        Vector3 startPos = sun.transform.position;
        Vector3 targetPos = startPos + move * moveDistance;
        float elapsedTime = 0f;

        while (elapsedTime < moveDistance / moveSpeed) // ���x���l�����Ĉړ�
        {
            sun.transform.position = Vector3.Lerp(startPos, targetPos, elapsedTime / (moveDistance / moveSpeed));
            elapsedTime += Time.deltaTime;
            yield return null; // 1�t���[���҂�
        }

        sun.transform.position = targetPos; // �ŏI�ʒu��␳
        isMoving = false; // �ړ�����
    }

    public void FlashPlayer(Collider2D collider)
    {
        if (hit)
        {
            return;
        }
        StartCoroutine(_hiting(collider));
    }

    IEnumerator _hiting(Collider2D collider)
    {
        hit = true;                                         // �t���O�𓖂����Ă����Ԃ�
        collider.enabled = false;                           // �����蔻����I�t
        Debug.Log("���G���ԊJ�n");

        for (int i = 0; i < loopCount; i++)
        {
            yield return new WaitForSeconds(flashInterval); // flashInterval���҂�
            sp.enabled = false;                             // �����_���[���I�t

            yield return new WaitForSeconds(flashInterval); // flashInterval���҂�
            sp.enabled = true;                              // �����_���[���I��
        }

        hit = false;                                        // �t���O�𓖂����Ă��Ȃ���Ԃ�
        collider.enabled = true;                            // �����蔻����I��
        Debug.Log("���G���ԏI��");
    }
}
