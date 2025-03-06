using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnemyController : MonoBehaviour
{/*==========================敵の移動変数==================================*/

	[SerializeField]
	private float dir = -1.0f;   // 敵の移動方向: 1.0fで右, -1.0fで左.
	[SerializeField]
	private float espeed = 1.0f; // 敵の移動速度.
	[SerializeField]
	private float timestop = 1.0f; // 敵が停止する時間.
	[SerializeField]
	private float knockbackForce = 5f; // ノックバックの力.
	[SerializeField]
	private float knockbackDuration = 0.5f; // ノックバック持続時間.
	[SerializeField]
	private float stapoAfterknockbackDuration = 1.0f; // ノックバック後に停止する時間.
	[SerializeField]
	private float fallSpeed = 5.0f; //落下速度.

	/*========================================================================*/

	/*=========================視野の変数=====================================*/
	// 自分自身
	[SerializeField]
	private Transform self;
	// ターゲット
	[SerializeField]
	private Transform target;
	// 視野角（度数法）
	[SerializeField]
	private float sightAngle;
	// 視界の最大距離
	[SerializeField]
	private float maxDistance = float.PositiveInfinity;
	[SerializeField]
	private LayerMask obstaclelayers;//視野を断裂する障害物のレイヤー.

	/*==========================================================================*/

	/*==============================プレイヤーを探索============================*/

	[SerializeField]
	private float minPatoroltime = 1f;
	[SerializeField]
	private float maxPatoroltime = 5f;
	[SerializeField]
	private float patrolRange = 3f;

	/*============================================================================*/

	/*==============================地面チェック用変数============================*/

	[SerializeField]
	private float groundCheckDistance = 0.1f;//地面チェックの距離.
	[SerializeField]
	private Vector2 groundCheckSize = new Vector2(0.9f, 0.1f);//地面チェックのサイズ.
	[SerializeField]
	private LayerMask groundLayer;//地面レイヤー.

	/*============================================================================*/


	public float soldieAttackDamage = 3.0f;//兵士の攻撃ダメージ.
	public float soldihealth = 100.0f;//兵士のHP.

	private bool isStopped = false;   // 敵が停止しているかを判定するフラグ.
	private bool isKnockback = false; // ノックバック中かを判定するフラグ.
	private bool isPatrolling = true; //パトロール中かを判定.
	private bool isGrounded = false;  //地面に接触しているかを判定するフラグ.
	private Rigidbody2D rb;
	private BoxCollider2D boxCollider;
	private Transform playerTransform;
	private Vector3 originalPosition;
	private float patrolTimer;
	void Start()
	{
		// Rigidbody2Dコンポーネントを取得、なければ追加.
		rb = GetComponent<Rigidbody2D>();
		if (rb == null)
		{
			rb = gameObject.AddComponent<Rigidbody2D>();
			rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;//連続的な突撃検出.

		}
		//BoxCollider2Dコンポーネントを習得.
		boxCollider = GetComponent<BoxCollider2D>();
		if(boxCollider ==null)
		{
			boxCollider = gameObject.AddComponent<BoxCollider2D>();
		}
		//初期状態では重力を完全に無効化
		DisableGravity();

		// 初期位置を保存
		originalPosition = transform.position;

		// パトロールタイマーを初期化
		StartPatrolTimer();
	}
	void FixedUpdate()
	{
		// 地面チェックを行う
		CheckGrounded();

		// 地面に接触していない場合は落下
		if (!isGrounded)
		{
			// 重力を有効化して落下
			rb.gravityScale = 1;
			rb.constraints = RigidbodyConstraints2D.FreezeRotation; // 回転のみ固定.

			//敵を落下させる.
			Vector3 movent = new Vector3(0, -fallSpeed * Time.deltaTime, 0);
			transform.position += movent;
		}
		else
		{
			// 地面にいる場合は重力を無効化し、Y位置を固定
			rb.gravityScale = 0;
			rb.velocity = new Vector2(rb.velocity.x, 0);
			rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
		}
	}
	void Update()
	{

		if (isGrounded && !isKnockback)
		{
			//プレイヤーが視野内にいるかどうかを確認.
			bool playerInSignt = IsVisible();
			if (playerInSignt)
			{
				// プレイヤーを追尾
				ChasePlayer();
			}
			else
			{
				// パトロール
				Patrol();
			}


			if (soldihealth == 0)
			{
				Destroy(this.gameObject);
			}
		}

	}
	/// <summary>
	/// 地面との接触をチェックする.
	/// </summary>
	private void CheckGrounded()
	{
		//BoxClliderの位置とサイズにづいて地面チェック用の位置を計算.
		Vector2 boxPosition = transform.position;
		boxPosition.y -= boxCollider.bounds.extents.y;

		//地面チェック用のオーバーラップボックスを作成.
		Collider2D[] colliders = Physics2D.OverlapBoxAll(
			boxPosition,
			groundCheckSize,
			0f
			);

		//衝突したオブジェクトがGroundタグを持っているかをチェック.
		isGrounded = false;
		foreach(var collider in colliders)
		{
			if(collider.CompareTag("Ground"))
			{
				isGrounded = true;
				break;
			}
		}
		Debug.DrawRay(
			new Vector3(boxPosition.x -groundCheckSize.x/2,boxPosition.y,0),
			new Vector3(groundCheckSize.x,0,0),
			isGrounded ? Color.green:Color.red
			);
	}
	/// <summary>
	/// トロールタイマーを設定.
	/// </summary>
	private void StartPatrolTimer()
	{
		patrolTimer = Random.Range(minPatoroltime, maxPatoroltime);
		dir *= -1;//方向を反転.
	}
	/// <summary>
	/// パトロール処理.
	/// </summary>
	private void Patrol()
	{
		if (!isStopped && !isKnockback)
		{
			//パトロールタイマーをデクリメント.
			patrolTimer -= Time.deltaTime;

			//移動.
			Vector3 movement = new Vector3(dir * espeed * Time.deltaTime, 0, 0);
			transform.position += movement;
			//範囲外に出ないようにチャック.
			if (Mathf.Abs(transform.position.x - originalPosition.x) > patrolRange)
			{
				dir *= -1;//方向を反転.

			}
			//タイマーが0以下になったら新しいタイマーをセット.
			if (patrolTimer <= 0)
			{
				StartPatrolTimer();
			}

			// スプライトの向きを設定.
			RotationAndSight();

		}
	}
	/// <summary>
	/// プレイヤーを追尾
	/// </summary>
	private void ChasePlayer()
	{
		if (!isStopped && !isKnockback)
		{
			//プレイヤーの方向を向く.
			dir = (target.position.x > transform.position.x) ? 1.0f : -1.0f;

			//プレイヤーに向かって移動.
			Vector3 movement = new Vector3(dir * espeed * Time.deltaTime, 0, 0);
			transform.position += movement;

			// スプライトの向きを設定
			RotationAndSight();

		}
	}

	/// <summary>
	/// 重力を完全に無効化する.
	/// </summary>
	public void DisableGravity()
	{
		if (rb != null)
		{
			rb.gravityScale = 0; // 重力スケールを0に.
			rb.isKinematic = true; // 物理シミュレーションを無効化.
			rb.constraints = RigidbodyConstraints2D.FreezePositionY; // Y軸の移動も固定.
		}
	}

	/// <summary>
	/// 重力を有効化する.
	/// </summary>
	public void EnbleGravity()
	{
		if(rb!= null)
		{
			rb.gravityScale = 1;//重力スケールを準備に設定.
			rb.constraints = RigidbodyConstraints2D.FreezeRotation;//回転のみ固定(Y軸の移動のみ)
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		
		// プレイヤーと衝突した場合.
		if (collision.gameObject.CompareTag("Player") && IsVisible())
		{

			// プレイヤー側の無敵フラグを参照してフォルスの場合のみ後続の処理を継続させる.

			collision.gameObject.GetComponent<ActionPlayer>().healthPoint -= soldieAttackDamage;

			

			Debug.Log("当たった!!");
			// プレイヤーの位置を取得.
			playerTransform = collision.transform;
			// ノックバック処理を開始.
			StartCoroutine(KnockbackRoutine());


			
		}
		//Groundタグのオブジェクトとの衝突を検出.
		if (collision.gameObject.CompareTag("tcck"))
		{
			soldihealth -= 100;

			if (soldihealth <= 0)
			{
				Destroy(this.gameObject);
			}
		}

	}
	private void OnCollisionEnter2D(Collision2D collision)
	{
		//Groundタグのオブジェクトとの衝突を検出.
		if (collision.gameObject.CompareTag("Ground"))
		{
			isGrounded = true;
			DisableGravity();//地面に接触したら重力を無効化.

		}

	}
	private void OnCollisionStay2D(Collision2D collision)
	{
		//Groundタグのオブジェクトとの衝突を検出.
		if (collision.gameObject.CompareTag("Ground"))
		{
			isGrounded = true;

		}
	}
	private void OnCollisionExit2D(Collision2D collision)
	{
		// Groundタグのオブジェクトとの接触が終了したことを検出
		if (collision.gameObject.CompareTag("Ground"))
		{
			isGrounded = false;

		}
	}

	/// <summary>
	///  移動方向に基づいて向きと視野を更新.
	/// </summary>
	private void RotationAndSight()
	{
		// 向き移動と回転
		if (dir > 0.1f)
		{
			// プラス方向に動いているとき (右へ移動)
			GetComponent<SpriteRenderer>().flipX = false; // flipX をチェックを外す

			// 視野の方向の右向きに設定
			self.right = Vector2.right;
		}
		else
		{
			// マイナス方向に動いているとき (左へ移動)
			GetComponent<SpriteRenderer>().flipX = false; // flipX をチェックする

			// 視野の方向の左向きに設定
			self.right = Vector2.left;
		}
	
    }
	
	private IEnumerator KnockbackRoutine()
	{
		// 移動を停止.
		isStopped = true;
		isKnockback = true;
		// ノックバックの方向を計算（プレイヤーから自分自身への方向）.
		Vector2 knockbackDirection = (Vector2)(transform.position - playerTransform.position).normalized;
		// ノックバック実行.
		rb.velocity = knockbackDirection * knockbackForce;
		// ノックバック持続時間待機.
		yield return new WaitForSeconds(knockbackDuration);
		// 速度をリセット.
		rb.velocity = Vector2.zero;
		yield return new WaitForSeconds(stapoAfterknockbackDuration);
		// 移動方向をプレイヤーに向ける.
		dir = (playerTransform.position.x > transform.position.x) ? 1.0f : -1.0f;
		// 移動再開
		isStopped = false;
		isKnockback = false;
	}
	#region Logic

	/// <summary>
	/// ターゲットが見えているかどうか
	/// </summary>
	public bool IsVisible()
	{
		//ターゲットがnullの場合はfalseを返す.
		if (target == null)
		{
			return false;
		}
		//自身の位置(レイキャストの開始地点).
		Vector2 selfPos = self.position;
		//ターゲットの位置(レイキャストの終点).
		Vector2 targetPos = target.position;

		//ターゲットへの方向とDistance計算.
		Vector2 directionToTarget = targetPos - selfPos;
		float distanceToTarget = directionToTarget.magnitude;

		//最大距離内かをチェック.
		if (distanceToTarget > maxDistance)
		{
			return false;
		}

		//敵の現在の向きに基づいて正しい方向を確定.
		Vector2 selfDir = self.right;

		//角度制約をチェック.
		float angleToTarget = Vector2.Angle(selfDir, directionToTarget);
		if (angleToTarget > sightAngle / 2f)
		{
			return false;
		}
		//障害物をチェックするためにレイキャストを実行.
		RaycastHit2D hit = Physics2D.Raycast(
			selfPos,
			directionToTarget.normalized,
			distanceToTarget,
			obstaclelayers
			);
		return hit.collider == null;
	}
	//レイキャストのデバック可視化.
	private void OnDrawGizmosSelected()
	{
		if (self == null || target == null)
		{
			return;
		}
		//レイキャストを描画.
		Vector2 selfpos = self.position;
		Vector2 targetPos = target.position;
		Vector2 directionToTarget = targetPos - selfpos;
		float distanceToTarget = directionToTarget.magnitude;

		//可視性にもとラインの色を変更.
		Gizmos.color = IsVisible() ? Color.green : Color.red;
		Gizmos.DrawCube(selfpos, targetPos);

	}

	#endregion

	#region Debug

	// 視界判定の結果をGUI出力
	private void OnGUI()
	{
		// 視界判定
		var isVisible = IsVisible();

		// 結果表示
		GUI.Box(new Rect(20, 20, 150, 23), $"当たり判定 = {isVisible}");
		GUI.Box(new Rect(20, 50, 150, 23), $"地面接触 = {isGrounded}");

	}

	#endregion
}