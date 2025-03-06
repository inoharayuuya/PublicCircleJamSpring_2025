using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RunPlayer : PlayerBace
{
    public StartCount start;
    public SpriteRenderer sp;                    //点滅用
    public GameObject sun;                       //動かす太陽
    private Animator animator;                   //Animatorをキャッシュ
    public Vector3 move = new Vector3(1, 0, 0);  //動かす方向
    public float moveDistance = 1.0f;            //移動距離
    public float moveSpeed = 1.5f;               //移動速度（数値を小さくするとゆっくり）
    [SerializeField] float flashInterval;        //移動スピードと点滅の間隔
    [SerializeField] int loopCount;              //点滅させるときのループのカウント
    public int moveCount;                        //太陽の動いた数
    public bool isJump;                          //アニメーションジャンプ用
    public bool gliding;                         //アニメーション滑空用
    public bool jumpStart;                       //アニメーション対空用
    public float moveing;                        //アニメーション走り用
    public float animoveing;

    private bool moved;                          //1度動かすフラグ
    private bool animoved;                       //アニメーションを1度動かすフラグ
    private bool isMoving;                       //移動中フラグ
    private bool operation;                      //操作可能かのフラグ
    private bool hit;

    private void Start()
    {
        moveing = 0;
        rb = GetComponent<Rigidbody2D>();
        sp = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();     //Animatorを取得
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
        //moveCount = 0;     //太陽の動いた数
        moved = false;     //1度動かすフラグ
        animoved = false;  //アニメーションを1度動かすフラグ
        isMoving = false;  //移動中フラグ
        operation = false; //操作可能かのフラグ
        hit = false;       //当っている時（点滅）用
    }

    public void AnimatorMove()
    {
        if (animator == null)
        {
            Debug.LogError("Animator is null in AnimatorMove!");  //Animatorがnullなら処理しない
            return;
        }

        if (start.startCountdown <= 0)
        {
            moveing = 1f;
            return;
        }

        // キーが押されたときジャンプ
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isJump = true;
        }
        else if (rb.velocity.magnitude != 0 && isInSky == true && jumpLimit == 1)
        {
            jumpStart = true;  // 対空アニメーションを開始
            StartCoroutine(ResetJumpStart()); // 一定時間後に jumpStart をオフにする
        }
   
         // 滑空アニメーションの制御
         if (isInSky && rb.velocity.y < 0 && jumpLimit == 0)  // 空中で下降中なら滑空
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
        // ジャンプ開始アニメーションを一定時間表示するためのコルーチン
        yield return new WaitForSeconds(0.2f); // 0.2秒後にjumpStartをオフにする
        jumpStart = false;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        //Debug.Log("衝突検知: " + gameObject.name); // 確認用ログ
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
        //Debug.Log("無敵時間開始");

        for (int i = 0; i < loopCount; i++)
        {
            yield return new WaitForSeconds(flashInterval); // flashInterval分待つ
            sp.enabled = false;                             // レンダラーをオフ

            yield return new WaitForSeconds(flashInterval); // flashInterval分待つ
            sp.enabled = true;                              // レンダラーをオン
        }

        hit = false;                                        // フラグを当たっていない状態に
        collider.enabled = true;                            // 当たり判定をオン
        //Debug.Log("無敵時間終了");
    }
}
