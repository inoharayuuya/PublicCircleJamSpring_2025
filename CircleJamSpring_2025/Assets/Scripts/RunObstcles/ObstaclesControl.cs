using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
//using UnityEngine.SceneManagement;

public class ObstaclesControl : MonoBehaviour
{
    public Soldier soldier;
    public GameObject sun;                       //���������z
    public Vector3 move = new Vector3(1, 0, 0);  //����������
    public float speed = 3.0f;                 //�v���C���[�ړ��p
    public float moveDistance = 1.0f;            //�ړ�����
    public float moveSpeed = 1.5f;               //�ړ����x�i���l������������Ƃ������j
    public int moveCount;                        //���z�̓�������

    private Rigidbody2D playerRigidbody;
    private bool moved;                          //1�x�������t���O
    private bool isMoving;                       //�ړ����t���O
    private bool operation;                      //����\���̃t���O

    void Update()
    {
        ObstaclesManager();
    }

    public void InitObstacles()
    {
        //moveCount = 0;     //���z�̓�������
        moved     = false; //1�x�������t���O
        isMoving  = false; //�ړ����t���O
        operation = false; //����\���̃t���O
    }
    public void ObstaclesManager()
    {
        if (!operation)
        {
            //�L�����N�^�[�̃I�u�W�F�N�g�������擾
            playerRigidbody = GetComponent<Rigidbody2D>();

            // A�L�[�������ꂽ�獶�ɁAD�L�[�������ꂽ��E�Ɉړ�
            float moveHorizontal = Input.GetAxis("Horizontal");

            // �L�����N�^�[�Ɉړ�����͂�^����
            playerRigidbody.velocity = new Vector2(moveHorizontal * speed, playerRigidbody.velocity.y);
        }
        else
        {

        }
        
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        //Debug.Log("�Փˌ��m: " + gameObject.name); // �m�F�p���O
        if (collider.gameObject.CompareTag("Obstacles") && !moved && !isMoving)
        {
            moved = true;
            moveCount += 1;
            Debug.Log(moveCount);
            moveDistance = 1;
            MoveSun();
        }
        
        if (collider.gameObject.CompareTag("Soldier") && !moved)
        {
            moved = true;
            moveCount += 2;
            Debug.Log(moveCount);
            moveDistance = 2;
            MoveSun();
        }

        if(moveCount >= 4)
        {
            operation = true;
            Debug.Log("gameover");
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
}