using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CharacterController))]
public class ThirdPersonCharacterContoroller : MonoBehaviour
{
	// Animatorコンポーネント
	private Animator m_animator;
	// CharacterControllerコンポーネント
	private CharacterController m_controller;

	public float m_walkSpeed = 2.0f; // 歩行速度
	public float m_runSpeed = 4.0f; // 走行速度
	public float m_jumpSpeed = 5.0f; // ジャンプ速度

	private Vector3 m_movement;   // 移動するベクター

	private float m_gravity = 20.0f; // キャラへの重力
	private float m_speedSmoothing = 10.0f;   // 回頭するときの滑らかさ
	private float m_rotateSpeed = 500.0f; // 回頭の速度
	private float m_runAfterSeconds = 0.1f;   // 走り終わったとき止まるまでの時間(秒)

	private Vector3 m_moveDirection = Vector3.zero;   // カレント移動方向
	private float m_verticalSpeed = 0.0f;    // カレント垂直方向速度
	private float m_moveSpeed = 0.0f;    // カレント水平方向速度

	private CollisionFlags m_collisionFlags;    //  controller.Move が返すコリジョンフラグ：キャラが何かにぶつかったとき使用

	private float m_walkTimeStart = 0.0f;    // 歩き始める速度

	// ユーザーの入力による値
	private float m_userVertical; // 縦方向の入力
	private float m_userHorizontal; // 水平方向の入力
	private bool m_userJump; // ジャンプ入力
	private bool m_userCrouch; // しゃがみ入力

	[SerializeField]
	private Transform m_raycastStartTransform;
	private RaycastHit m_hit;
	private GameObject m_parentObj;

	// Use this for initialization
	void Start()
	{
		m_hit = new RaycastHit();
		// Animator コンポーネントを取得
		m_animator = GetComponent<Animator>();
		// CharacterController コンポーネントを取得
		m_controller = GetComponent<CharacterController>();
		// キャラの移動方向をキャラの向いている方向にセットする
		m_moveDirection = transform.TransformDirection(Vector3.forward);
	}

	/// <summary>
	/// ユーザーの操作をメンバ変数に反映する
	/// </summary>
	public void SetUserControl(float v, float h, bool jump, bool crouch)
	{
		m_userVertical = v;
		m_userHorizontal = h;
		m_userJump = jump;
		m_userCrouch = crouch;
	}

	// Update is called once per frame
	void Update()
	{
		// カメラの向いている方向を取得
		Transform cameraTransform = Camera.main.transform;
		// camera の x-z 平面から forward ベクターを求める 
		Vector3 forward = cameraTransform.TransformDirection(Vector3.forward);
		// Y方向は無視：キャラは水平面しか移動しないため
		forward.y = 0;
		// 方向を正規化
		forward = forward.normalized;
		// 右方向ベクターは常にforwardに直交
		Vector3 right = new Vector3(forward.z, 0, -forward.x);

		float v = m_userVertical;//Input.GetAxisRaw("Vertical"); // マウスもしくはコントローラスティックの垂直方向の値
		float h = m_userHorizontal;//Input.GetAxisRaw("Horizontal");   // マウスもしくはコントローラスティックの水平方向の値

		bool jump = m_userJump;// Input.GetButtonDown("Jump"); // キーボードもしくはコントローラのJumpの値
		bool crouch = m_userCrouch; // キーボードもしくはコントローラのCrouchの値

		// カメラと連動した進行方向を計算：視点の向きが前方方向
		Vector3 targetDirection = h * right + v * forward;

		bool isGrounded = CheckGrounded();

		// キャラは接地しているか？：宙に浮いていないとき
		if (m_controller.isGrounded || isGrounded)
		{
			Debug.Log("接地している");
			if (targetDirection != Vector3.zero) // キャラは順方向を向いていないか？：つまり回頭している場合
			{
				if (m_moveSpeed < m_walkSpeed * 0.9) // ゆっくり移動しているか
				{
					m_moveDirection = targetDirection.normalized; // 止まっているときは即時ターン
				}
				else  // 移動しているときはスムースにターン
				{
					m_moveDirection = Vector3.RotateTowards(m_moveDirection, targetDirection, m_rotateSpeed * Mathf.Deg2Rad * Time.deltaTime, 1000);
					m_moveDirection = m_moveDirection.normalized;
				}
			}

			// 向きをスムースに変更
			float curSmooth = m_speedSmoothing * Time.deltaTime;
			// 最低限のスピードを設定
			float targetSpeed = Mathf.Min(targetDirection.magnitude, 1.0f);

			// 歩く速度と走る速度の切り替え：最初は歩いてで時間がたつと走る
			if (Time.time - m_runAfterSeconds > m_walkTimeStart)
			{
				targetSpeed *= m_runSpeed;
			}
			else
			{
				targetSpeed *= m_walkSpeed;
			}

			// Animatorの現在再生中のステートがTopToGroundか？
			if (m_animator.GetCurrentAnimatorStateInfo(0).IsName("TopToGround")
				|| m_animator.GetCurrentAnimatorStateInfo(0).IsName("Crouch"))
			{
				// 移動速度を0に設定
				m_moveSpeed = 0;
			}
			else
			{
				// 移動速度を設定
				m_moveSpeed = Mathf.Lerp(m_moveSpeed, targetSpeed, curSmooth);
			}

			// Animator に移動速度のパラメータを渡す
			m_animator.SetFloat("speed", m_moveSpeed);
			// Animator に落下フラグのパラメータを渡す：落下していない
			m_animator.SetBool("fall", false);

			// まだ歩きはじめ
			if (m_moveSpeed < m_walkSpeed * 0.3)
			{
				// その時間を保存しておく
				m_walkTimeStart = Time.time;
			}

			// ジャンプボタンが押されたか？
			if (jump
				&& m_animator.GetCurrentAnimatorStateInfo(0).IsName("walkrun")
				&& !m_animator.IsInTransition(0))
			{
				Debug.Log("Jump");
				// AnimatorのjumpのトリガーをONにする
				m_animator.SetTrigger("jump");
				// 垂直方向の速度を設定
				m_verticalSpeed = m_jumpSpeed;
			}
			// しゃがみボタンが押されたか？
			if (crouch 
				&& m_animator.GetCurrentAnimatorStateInfo(0).IsName("walkrun")
				&& !m_animator.IsInTransition(0))
			{
				Debug.Log("Crouch");
				// AnimatorのcrouchのトリガーをONにする
				m_animator.SetTrigger("crouch");
				// 移動速度を0に設定
				m_moveSpeed = 0;
			}
		}
		else
		{
			// 宙に浮いている場合
			Debug.Log("宙に浮いている");

			if (targetDirection != Vector3.zero) // キャラは順方向を向いていないか？：つまり回頭している場合
			{
				if (m_moveSpeed < m_walkSpeed * 0.9) // ゆっくり移動しているか
				{
					m_moveDirection = targetDirection.normalized; // 止まっているときは即時ターン
				}
				else  // 移動しているときはスムースにターン
				{
					m_moveDirection = Vector3.RotateTowards(m_moveDirection, targetDirection, m_rotateSpeed * Mathf.Deg2Rad * Time.deltaTime, 1000);
					m_moveDirection = m_moveDirection.normalized;
				}
			}
			// 重力を適応
			m_verticalSpeed -= m_gravity * Time.deltaTime;
			if (m_verticalSpeed < -1.0)   // 落ちる速度が一定を超えたら
			{
				// Animator に落下フラグのパラメータを渡す：落下している
				m_animator.SetBool("fall", true);
			}
		}
		Debug.Log("m_verticalSpeed:" + m_verticalSpeed);
		// キャラの移動量を計算
		m_movement = m_moveDirection * m_moveSpeed + new Vector3(0, m_verticalSpeed, 0);
		m_movement *= Time.deltaTime;
		// キャラを移動をキャラクターコントローラに伝える
		m_collisionFlags = m_controller.Move(m_movement);

		// 移動方向に回頭：浮いていると回頭しない
		transform.rotation = Quaternion.LookRotation(m_moveDirection);
	}

	/// <summary>
	/// 地面に接地しているかどうかを調べる
	/// </summary>
	public bool CheckGrounded()
	{
		if (m_controller.isGrounded) { return true; }
		//　CharacterControllerのコライダで接地が確認出来ない場合
		if (Physics.Linecast(m_raycastStartTransform.position, (m_raycastStartTransform.position - transform.up * 0.5f), out m_hit))
		{
			m_parentObj = m_hit.collider.gameObject;
			// 親のPlayerオブジェクトごと、接地したオブジェクトの子にする
			transform.parent.gameObject.transform.SetParent(m_parentObj.transform);
			return true;
		}
		else
		{
			return false;
		}
	}

	public void OnCallChangeFace(string str)
	{

	}

	/// <summary>
	/// Characterontrollerの当たり判定用処理
	/// </summary>
	/// <param name="hit"></param>
	void OnControllerColliderHit(ControllerColliderHit hit)
	{
		// hit.gameObjectで衝突したオブジェクト情報が得られる
		if (hit.gameObject.tag == "Stage" || hit.gameObject.tag == "MoveStage")
		{
			// 親のPlayerオブジェクトごと、接地したオブジェクトの子にする
			transform.parent.gameObject.transform.SetParent(hit.transform);
		}
	}
}