using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening; 

public class StageMove : MonoBehaviour
{
	[SerializeField]
	// 移動する座標
	private Vector3[] m_targetPositions;

	[SerializeField]
	// 移動にかかる時間
	private float m_moveTime = 1.0f;

	[SerializeField]
	// ループする回数
	private int m_isLoop = -1;

	// DOTweenでの動きを制御するシーケンス
	private Sequence m_sequence;

	/// <summary>
	/// 初期化処理
	/// </summary>
	/// <param name="positions"></param>
	public void Init(Vector3[] positions)
	{
		m_targetPositions = positions;
		// 初期座標を設定
		this.transform.position = m_targetPositions[m_targetPositions.Length - 1];
		// シーケンスを初期化
		InitSequence();
	}

	/// <summary>
	/// シーケンスを初期化する
	/// </summary>
	private void InitSequence()
	{
		m_sequence = DOTween.Sequence();
		// シーケンスに動作を追加
		for (int i = 0; i < m_targetPositions.Length; i++)
		{
			m_sequence.Append(this.transform.DOMove(m_targetPositions[i], m_moveTime));
		}
		// ループする回数を指定
		m_sequence.SetLoops(m_isLoop);
	}
}
