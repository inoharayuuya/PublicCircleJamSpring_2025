using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class RunPlayer : PlayerBace
{
    public SpriteRenderer sp;                    //点滅用
    public Soldier soldier;
    public GameObject sun;                       //動かす太陽
    public Vector3 move = new Vector3(1, 0, 0);  //動かす方向
    public float moveDistance = 1.0f;            //移動距離
    public float moveSpeed = 1.5f;               //移動速度（数値を小さくするとゆっくり）
    [SerializeField] float flashInterval;        //移動スピードと点滅の間隔
    [SerializeField] int loopCount;              //点滅させるときのループのカウント
    public int moveCount;                        //太陽の動いた数
    public GameObject runStage;
    public RunHaikei run_haikei;

    private Rigidbody2D playerRigidbody;
    private bool moved;                          //1度動かすフラグ
    private bool isMoving;                       //移動中フラグ
    private bool operation;                      //操作可能かのフラグ
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
        moveCount = 0;     //太陽の動いた数
        moved = false;     //1度動かすフラグ
        isMoving = false;  //移動中フラグ
        operation = false; //操作可能かのフラグ
        hit = false;       //当っている時（点滅）用
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log("衝突検知: " + gameObject.name); // 確認用ログ
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
        print("接地");
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
            moved = false; //離れたらフラグをリセット
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
        hit = true;                                         // フラグを当たっている状態に
        collider.enabled = false;                           // 当たり判定をオフ
        Debug.Log("無敵時間開始");

        for (int i = 0; i < loopCount; i++)
        {
            yield return new WaitForSeconds(flashInterval); // flashInterval分待つ
            sp.enabled = false;                             // レンダラーをオフ

            yield return new WaitForSeconds(flashInterval); // flashInterval分待つ
            sp.enabled = true;                              // レンダラーをオン
        }

        hit = false;                                        // フラグを当たっていない状態に
        collider.enabled = true;                            // 当たり判定をオン
        Debug.Log("無敵時間終了");
    }
}
