using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RunPlayer : PlayerBace
{
    public StartCount start;
    public SpriteRenderer sp;                    //�_�ŗp
    public GameObject sun;                       //���������z
    private Animator animator;                   //Animator���L���b�V��
    public Vector3 move = new Vector3(1, 0, 0);  //����������
    public float moveDistance = 1.0f;            //�ړ�����
    public float moveSpeed = 1.5f;               //�ړ����x�i���l������������Ƃ������j
    [SerializeField] float flashInterval;        //�ړ��X�s�[�h�Ɠ_�ł̊Ԋu
    [SerializeField] int loopCount;              //�_�ł�����Ƃ��̃��[�v�̃J�E���g
    public int moveCount;                        //���z�̓�������
    public bool isJump;                          //�A�j���[�V�����W�����v�p
    public bool gliding;                         //�A�j���[�V��������p
    public bool jumpStart;                       //�A�j���[�V�����΋�p
    public float moveing;                        //�A�j���[�V��������p
    public float animoveing;

    private bool moved;                          //1�x�������t���O
    private bool animoved;                       //�A�j���[�V������1�x�������t���O
    private bool isMoving;                       //�ړ����t���O
    private bool operation;                      //����\���̃t���O
    private bool hit;

    private void Start()
    {
        moveing = 0;
        rb = GetComponent<Rigidbody2D>();
        sp = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();     //Animator���擾
        //start = new StartCount();
        InitObstacles();
    }
    void Update()
    {
        if(operation == false)
        {
            Jump(jumpAmount);
        }
        AnimatorMove();
    }

    public void InitObstacles()
    {
        //moveCount = 0;     //���z�̓�������
        moved = false;     //1�x�������t���O
        animoved = false;  //�A�j���[�V������1�x�������t���O
        isMoving = false;  //�ړ����t���O
        operation = false; //����\���̃t���O
        hit = false;       //�����Ă��鎞�i�_�Łj�p
    }

    public void AnimatorMove()
    {
        if (animator == null)
        {
            Debug.LogError("Animator is null in AnimatorMove!");  //Animator��null�Ȃ珈�����Ȃ�
            return;
        }

        if (start.startCountdown <= 0)
        {
            moveing = 1f;
            return;
        }

        // �L�[�������ꂽ�Ƃ��W�����v
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isJump = true;
        }
        else if (rb.velocity.magnitude != 0 && isInSky == true && jumpLimit == 1)
        {
            jumpStart = true;  // �΋�A�j���[�V�������J�n
            StartCoroutine(ResetJumpStart()); // ��莞�Ԍ�� jumpStart ���I�t�ɂ���
        }
   
         // ����A�j���[�V�����̐���
         if (isInSky && rb.velocity.y < 0 && jumpLimit == 0)  // �󒆂ŉ��~���Ȃ犊��
         {
             gliding = true;
         }
         else
         {
             gliding = false;
         }

        animator.SetBool("gliding", gliding);
        animator.SetBool("jumpStart", jumpStart);
        animator.SetBool("isJump", isJump);
        animator.SetFloat("move", moveing);
        animator.SetFloat("animove", animoveing);
    }
    
    private IEnumerator ResetJumpStart()
    {
        // �W�����v�J�n�A�j���[�V��������莞�ԕ\�����邽�߂̃R���[�`��
        yield return new WaitForSeconds(0.2f); // 0.2�b���jumpStart���I�t�ɂ���
        jumpStart = false;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        //Debug.Log("�Փˌ��m: " + gameObject.name); // �m�F�p���O
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
            animoved = false;
            gliding = false;
            isJump = false;
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
        //Debug.Log("���G���ԊJ�n");

        for (int i = 0; i < loopCount; i++)
        {
            yield return new WaitForSeconds(flashInterval); // flashInterval���҂�
            sp.enabled = false;                             // �����_���[���I�t

            yield return new WaitForSeconds(flashInterval); // flashInterval���҂�
            sp.enabled = true;                              // �����_���[���I��
        }

        hit = false;                                        // �t���O�𓖂����Ă��Ȃ���Ԃ�
        collider.enabled = true;                            // �����蔻����I��
        //Debug.Log("���G���ԏI��");
    }
}
