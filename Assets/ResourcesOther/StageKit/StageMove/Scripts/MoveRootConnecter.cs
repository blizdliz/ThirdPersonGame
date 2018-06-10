using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class MoveRootConnecter : MonoBehaviour
{
	private LineRenderer m_lineRernderer;

	private Vector3[] m_positions;

	/// <summary>
	/// 初期化処理
	/// </summary>
	public void Init ()
	{
		m_lineRernderer = this.GetComponent<LineRenderer>();
		m_positions = new Vector3[this.transform.childCount];
		
		int count = 0;
		// 子供のトランスフォームを取得
		foreach (Transform child in transform)
		{
			m_positions[count] = child.position;
			count++;
		}
		m_lineRernderer.positionCount = count;

		// ラインレンダラーにポジションをセット
		m_lineRernderer.SetPositions(m_positions);
	}

	/// <summary>
	/// ポジションの配列を返す
	/// </summary>
	public Vector3[] GetPositions()
	{
		return m_positions;
	}
}
