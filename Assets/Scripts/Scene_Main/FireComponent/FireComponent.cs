using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 可燃物コンポーネント
/// </summary>
[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Rigidbody))]
public class FireComponent : MonoBehaviour
{
	[SerializeField]
	private GameObject m_fireParticleObj;

	[SerializeField]
	// 自分自身が燃えているかフラグ
	private bool m_isFire = false;

	// Use this for initialization
	void Start()
	{
		Init();
	}

	/// <summary>
	/// 初期化処理
	/// </summary>
	private void Init()
	{
		if (m_isFire)
		{
			StartFire();
		}
		else
		{
			EndFire();
		}
	}

	/// <summary>
	/// 燃えているかフラグを返却
	/// </summary>
	/// <returns></returns>
	public bool GetIsFire()
	{
		return m_isFire;
	}

	/// <summary>
	/// 着火する
	/// </summary>
	public void StartFire()
	{
		Debug.Log("StartFire()");
		m_isFire = true;
		m_fireParticleObj.SetActive(true);
	}

	/// <summary>
	///  消火する
	/// </summary>
	public void EndFire()
	{
		Debug.Log("EndFire()");
		m_isFire = false;
		m_fireParticleObj.SetActive(false);
	}

	/// <summary>
	/// 接触判定
	/// </summary>
	/// <param name="other"></param>
	void OnTriggerEnter(Collider other)
	{
		FireComponent fireComponent = other.gameObject.GetComponent<FireComponent>();
		if (fireComponent != null)
		{
			// 可燃オブジェクトに接触した場合
			if (fireComponent.GetIsFire())
			{
				// 相手が燃えていたら自分に着火する
				StartFire();
			}
		}
	}
}
