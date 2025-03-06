using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
//using UnityEngine.SceneManagement;

public class ObstaclesControl : MonoBehaviour
{
    public Soldier soldier;
    public GameObject sun;                       //動かす太陽
    public Vector3 move = new Vector3(1, 0, 0);  //動かす方向
    public float speed = 3.0f;                 //プレイヤー移動用
    public float moveDistance = 1.0f;            //移動距離
    public float moveSpeed = 1.5f;               //移動速度（数値を小さくするとゆっくり）
    public int moveCount;                        //太陽の動いた数

    private Rigidbody2D playerRigidbody;
    private bool moved;                          //1度動かすフラグ
    private bool isMoving;                       //移動中フラグ
    private bool operation;                      //操作可能かのフラグ

    void Update()
    {
        ObstaclesManager();
    }

    public void InitObstacles()
    {
        //moveCount = 0;     //太陽の動いた数
        moved     = false; //1度動かすフラグ
        isMoving  = false; //移動中フラグ
        operation = false; //操作可能かのフラグ
    }
    public void ObstaclesManager()
    {
        if (!operation)
        {
            //キャラクターのオブジェクトを情報を取得
            playerRigidbody = GetComponent<Rigidbody2D>();

            // Aキーが押されたら左に、Dキーが押されたら右に移動
            float moveHorizontal = Input.GetAxis("Horizontal");

            // キャラクターに移動する力を与える
            playerRigidbody.velocity = new Vector2(moveHorizontal * speed, playerRigidbody.velocity.y);
        }
        else
        {

        }
        
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        //Debug.Log("衝突検知: " + gameObject.name); // 確認用ログ
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
            moved = false; //離れたらフラグをリセット
        }
    }

    public void MoveSun()
    {
        //Debug.Log("sun: " + sun); // 確認用ログ

        if (sun != null)//sunが設定されているかをチェック
        {
            /*if(moveCount == 1)
            {
                sun.transform.position += move * moveDistance;
            }*/
            
            StartCoroutine(MoveSunCoroutine());
            //Debug.Log("移動しました");
            
        }
        else
        {
            //Debug.LogError("設定されていない");
        }
    }

    public IEnumerator MoveSunCoroutine()
    {
        isMoving = true; // 移動中フラグを立てる

        Vector3 startPos = sun.transform.position;
        Vector3 targetPos = startPos + move * moveDistance;
        float elapsedTime = 0f;

        while (elapsedTime < moveDistance / moveSpeed) // 速度を考慮して移動
        {
            sun.transform.position = Vector3.Lerp(startPos, targetPos, elapsedTime / (moveDistance / moveSpeed));
            elapsedTime += Time.deltaTime;
            yield return null; // 1フレーム待つ
        }

        sun.transform.position = targetPos; // 最終位置を補正
        isMoving = false; // 移動完了
    }
}