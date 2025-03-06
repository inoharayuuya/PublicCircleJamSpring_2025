using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnemyController : MonoBehaviour
{/*==========================�G�̈ړ��ϐ�==================================*/

	[SerializeField]
	private float dir = -1.0f;   // �G�̈ړ�����: 1.0f�ŉE, -1.0f�ō�.
	[SerializeField]
	private float espeed = 1.0f; // �G�̈ړ����x.
	[SerializeField]
	private float timestop = 1.0f; // �G����~���鎞��.
	[SerializeField]
	private float knockbackForce = 5f; // �m�b�N�o�b�N�̗�.
	[SerializeField]
	private float knockbackDuration = 0.5f; // �m�b�N�o�b�N��������.
	[SerializeField]
	private float stapoAfterknockbackDuration = 1.0f; // �m�b�N�o�b�N��ɒ�~���鎞��.
	[SerializeField]
	private float fallSpeed = 5.0f; //�������x.

	/*========================================================================*/

	/*=========================����̕ϐ�=====================================*/
	// �������g
	[SerializeField]
	private Transform self;
	// �^�[�Q�b�g
	[SerializeField]
	private Transform target;
	// ����p�i�x���@�j
	[SerializeField]
	private float sightAngle;
	// ���E�̍ő勗��
	[SerializeField]
	private float maxDistance = float.PositiveInfinity;
	[SerializeField]
	private LayerMask obstaclelayers;//�����f�􂷂��Q���̃��C���[.

	/*==========================================================================*/

	/*==============================�v���C���[��T��============================*/

	[SerializeField]
	private float minPatoroltime = 1f;
	[SerializeField]
	private float maxPatoroltime = 5f;
	[SerializeField]
	private float patrolRange = 3f;

	/*============================================================================*/

	/*==============================�n�ʃ`�F�b�N�p�ϐ�============================*/

	[SerializeField]
	private float groundCheckDistance = 0.1f;//�n�ʃ`�F�b�N�̋���.
	[SerializeField]
	private Vector2 groundCheckSize = new Vector2(0.9f, 0.1f);//�n�ʃ`�F�b�N�̃T�C�Y.
	[SerializeField]
	private LayerMask groundLayer;//�n�ʃ��C���[.

	/*============================================================================*/


	public float soldieAttackDamage = 3.0f;//���m�̍U���_���[�W.
	public float soldihealth = 100.0f;//���m��HP.

	private bool isStopped = false;   // �G����~���Ă��邩�𔻒肷��t���O.
	private bool isKnockback = false; // �m�b�N�o�b�N�����𔻒肷��t���O.
	private bool isPatrolling = true; //�p�g���[�������𔻒�.
	private bool isGrounded = false;  //�n�ʂɐڐG���Ă��邩�𔻒肷��t���O.
	private Rigidbody2D rb;
	private BoxCollider2D boxCollider;
	private Transform playerTransform;
	private Vector3 originalPosition;
	private float patrolTimer;
	void Start()
	{
		// Rigidbody2D�R���|�[�l���g���擾�A�Ȃ���Βǉ�.
		rb = GetComponent<Rigidbody2D>();
		if (rb == null)
		{
			rb = gameObject.AddComponent<Rigidbody2D>();
			rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;//�A���I�ȓˌ����o.

		}
		//BoxCollider2D�R���|�[�l���g���K��.
		boxCollider = GetComponent<BoxCollider2D>();
		if(boxCollider ==null)
		{
			boxCollider = gameObject.AddComponent<BoxCollider2D>();
		}
		//������Ԃł͏d�͂����S�ɖ�����
		DisableGravity();

		// �����ʒu��ۑ�
		originalPosition = transform.position;

		// �p�g���[���^�C�}�[��������
		StartPatrolTimer();
	}
	void FixedUpdate()
	{
		// �n�ʃ`�F�b�N���s��
		CheckGrounded();

		// �n�ʂɐڐG���Ă��Ȃ��ꍇ�͗���
		if (!isGrounded)
		{
			// �d�͂�L�������ė���
			rb.gravityScale = 1;
			rb.constraints = RigidbodyConstraints2D.FreezeRotation; // ��]�̂݌Œ�.

			//�G�𗎉�������.
			Vector3 movent = new Vector3(0, -fallSpeed * Time.deltaTime, 0);
			transform.position += movent;
		}
		else
		{
			// �n�ʂɂ���ꍇ�͏d�͂𖳌������AY�ʒu���Œ�
			rb.gravityScale = 0;
			rb.velocity = new Vector2(rb.velocity.x, 0);
			rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
		}
	}
	void Update()
	{

		if (isGrounded && !isKnockback)
		{
			//�v���C���[��������ɂ��邩�ǂ������m�F.
			bool playerInSignt = IsVisible();
			if (playerInSignt)
			{
				// �v���C���[��ǔ�
				ChasePlayer();
			}
			else
			{
				// �p�g���[��
				Patrol();
			}


			if (soldihealth == 0)
			{
				Destroy(this.gameObject);
			}
		}

	}
	/// <summary>
	/// �n�ʂƂ̐ڐG���`�F�b�N����.
	/// </summary>
	private void CheckGrounded()
	{
		//BoxCllider�̈ʒu�ƃT�C�Y�ɂÂ��Ēn�ʃ`�F�b�N�p�̈ʒu���v�Z.
		Vector2 boxPosition = transform.position;
		boxPosition.y -= boxCollider.bounds.extents.y;

		//�n�ʃ`�F�b�N�p�̃I�[�o�[���b�v�{�b�N�X���쐬.
		Collider2D[] colliders = Physics2D.OverlapBoxAll(
			boxPosition,
			groundCheckSize,
			0f
			);

		//�Փ˂����I�u�W�F�N�g��Ground�^�O�������Ă��邩���`�F�b�N.
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
	/// �g���[���^�C�}�[��ݒ�.
	/// </summary>
	private void StartPatrolTimer()
	{
		patrolTimer = Random.Range(minPatoroltime, maxPatoroltime);
		dir *= -1;//�����𔽓].
	}
	/// <summary>
	/// �p�g���[������.
	/// </summary>
	private void Patrol()
	{
		if (!isStopped && !isKnockback)
		{
			//�p�g���[���^�C�}�[���f�N�������g.
			patrolTimer -= Time.deltaTime;

			//�ړ�.
			Vector3 movement = new Vector3(dir * espeed * Time.deltaTime, 0, 0);
			transform.position += movement;
			//�͈͊O�ɏo�Ȃ��悤�Ƀ`���b�N.
			if (Mathf.Abs(transform.position.x - originalPosition.x) > patrolRange)
			{
				dir *= -1;//�����𔽓].

			}
			//�^�C�}�[��0�ȉ��ɂȂ�����V�����^�C�}�[���Z�b�g.
			if (patrolTimer <= 0)
			{
				StartPatrolTimer();
			}

			// �X�v���C�g�̌�����ݒ�.
			RotationAndSight();

		}
	}
	/// <summary>
	/// �v���C���[��ǔ�
	/// </summary>
	private void ChasePlayer()
	{
		if (!isStopped && !isKnockback)
		{
			//�v���C���[�̕���������.
			dir = (target.position.x > transform.position.x) ? 1.0f : -1.0f;

			//�v���C���[�Ɍ������Ĉړ�.
			Vector3 movement = new Vector3(dir * espeed * Time.deltaTime, 0, 0);
			transform.position += movement;

			// �X�v���C�g�̌�����ݒ�
			RotationAndSight();

		}
	}

	/// <summary>
	/// �d�͂����S�ɖ���������.
	/// </summary>
	public void DisableGravity()
	{
		if (rb != null)
		{
			rb.gravityScale = 0; // �d�̓X�P�[����0��.
			rb.isKinematic = true; // �����V�~�����[�V�����𖳌���.
			rb.constraints = RigidbodyConstraints2D.FreezePositionY; // Y���̈ړ����Œ�.
		}
	}

	/// <summary>
	/// �d�͂�L��������.
	/// </summary>
	public void EnbleGravity()
	{
		if(rb!= null)
		{
			rb.gravityScale = 1;//�d�̓X�P�[���������ɐݒ�.
			rb.constraints = RigidbodyConstraints2D.FreezeRotation;//��]�̂݌Œ�(Y���̈ړ��̂�)
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		
		// �v���C���[�ƏՓ˂����ꍇ.
		if (collision.gameObject.CompareTag("Player") && IsVisible())
		{

			// �v���C���[���̖��G�t���O���Q�Ƃ��ăt�H���X�̏ꍇ�̂݌㑱�̏������p��������.

			collision.gameObject.GetComponent<ActionPlayer>().healthPoint -= soldieAttackDamage;

			

			Debug.Log("��������!!");
			// �v���C���[�̈ʒu���擾.
			playerTransform = collision.transform;
			// �m�b�N�o�b�N�������J�n.
			StartCoroutine(KnockbackRoutine());


			
		}
		//Ground�^�O�̃I�u�W�F�N�g�Ƃ̏Փ˂����o.
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
		//Ground�^�O�̃I�u�W�F�N�g�Ƃ̏Փ˂����o.
		if (collision.gameObject.CompareTag("Ground"))
		{
			isGrounded = true;
			DisableGravity();//�n�ʂɐڐG������d�͂𖳌���.

		}

	}
	private void OnCollisionStay2D(Collision2D collision)
	{
		//Ground�^�O�̃I�u�W�F�N�g�Ƃ̏Փ˂����o.
		if (collision.gameObject.CompareTag("Ground"))
		{
			isGrounded = true;

		}
	}
	private void OnCollisionExit2D(Collision2D collision)
	{
		// Ground�^�O�̃I�u�W�F�N�g�Ƃ̐ڐG���I���������Ƃ����o
		if (collision.gameObject.CompareTag("Ground"))
		{
			isGrounded = false;

		}
	}

	/// <summary>
	///  �ړ������Ɋ�Â��Č����Ǝ�����X�V.
	/// </summary>
	private void RotationAndSight()
	{
		// �����ړ��Ɖ�]
		if (dir > 0.1f)
		{
			// �v���X�����ɓ����Ă���Ƃ� (�E�ֈړ�)
			GetComponent<SpriteRenderer>().flipX = false; // flipX ���`�F�b�N���O��

			// ����̕����̉E�����ɐݒ�
			self.right = Vector2.right;
		}
		else
		{
			// �}�C�i�X�����ɓ����Ă���Ƃ� (���ֈړ�)
			GetComponent<SpriteRenderer>().flipX = false; // flipX ���`�F�b�N����

			// ����̕����̍������ɐݒ�
			self.right = Vector2.left;
		}
	
    }
	
	private IEnumerator KnockbackRoutine()
	{
		// �ړ����~.
		isStopped = true;
		isKnockback = true;
		// �m�b�N�o�b�N�̕������v�Z�i�v���C���[���玩�����g�ւ̕����j.
		Vector2 knockbackDirection = (Vector2)(transform.position - playerTransform.position).normalized;
		// �m�b�N�o�b�N���s.
		rb.velocity = knockbackDirection * knockbackForce;
		// �m�b�N�o�b�N�������ԑҋ@.
		yield return new WaitForSeconds(knockbackDuration);
		// ���x�����Z�b�g.
		rb.velocity = Vector2.zero;
		yield return new WaitForSeconds(stapoAfterknockbackDuration);
		// �ړ��������v���C���[�Ɍ�����.
		dir = (playerTransform.position.x > transform.position.x) ? 1.0f : -1.0f;
		// �ړ��ĊJ
		isStopped = false;
		isKnockback = false;
	}
	#region Logic

	/// <summary>
	/// �^�[�Q�b�g�������Ă��邩�ǂ���
	/// </summary>
	public bool IsVisible()
	{
		//�^�[�Q�b�g��null�̏ꍇ��false��Ԃ�.
		if (target == null)
		{
			return false;
		}
		//���g�̈ʒu(���C�L���X�g�̊J�n�n�_).
		Vector2 selfPos = self.position;
		//�^�[�Q�b�g�̈ʒu(���C�L���X�g�̏I�_).
		Vector2 targetPos = target.position;

		//�^�[�Q�b�g�ւ̕�����Distance�v�Z.
		Vector2 directionToTarget = targetPos - selfPos;
		float distanceToTarget = directionToTarget.magnitude;

		//�ő勗���������`�F�b�N.
		if (distanceToTarget > maxDistance)
		{
			return false;
		}

		//�G�̌��݂̌����Ɋ�Â��Đ������������m��.
		Vector2 selfDir = self.right;

		//�p�x������`�F�b�N.
		float angleToTarget = Vector2.Angle(selfDir, directionToTarget);
		if (angleToTarget > sightAngle / 2f)
		{
			return false;
		}
		//��Q�����`�F�b�N���邽�߂Ƀ��C�L���X�g�����s.
		RaycastHit2D hit = Physics2D.Raycast(
			selfPos,
			directionToTarget.normalized,
			distanceToTarget,
			obstaclelayers
			);
		return hit.collider == null;
	}
	//���C�L���X�g�̃f�o�b�N����.
	private void OnDrawGizmosSelected()
	{
		if (self == null || target == null)
		{
			return;
		}
		//���C�L���X�g��`��.
		Vector2 selfpos = self.position;
		Vector2 targetPos = target.position;
		Vector2 directionToTarget = targetPos - selfpos;
		float distanceToTarget = directionToTarget.magnitude;

		//�����ɂ��ƃ��C���̐F��ύX.
		Gizmos.color = IsVisible() ? Color.green : Color.red;
		Gizmos.DrawCube(selfpos, targetPos);

	}

	#endregion

	#region Debug

	// ���E����̌��ʂ�GUI�o��
	private void OnGUI()
	{
		// ���E����
		var isVisible = IsVisible();

		// ���ʕ\��
		GUI.Box(new Rect(20, 20, 150, 23), $"�����蔻�� = {isVisible}");
		GUI.Box(new Rect(20, 50, 150, 23), $"�n�ʐڐG = {isGrounded}");

	}

	#endregion
}