using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening; 

public class StageMove : MonoBehaviour
{
	[SerializeField]
	private Transform m_startTarget;

	[SerializeField]
	private Transform m_goalTarget;

	[SerializeField]
	private float m_moveTime = 1.0f;

	// DOTweenでの動きを制御するシーケンス
	private Sequence m_sequence;

	// Use this for initialization
	void Start ()
	{
		this.transform.position = m_startTarget.position;
		InitSequence();
	}

	private void InitSequence()
	{
		m_sequence = DOTween.Sequence();
		m_sequence.Append(this.transform.DOMove(m_goalTarget.position, m_moveTime));
		m_sequence.Append(this.transform.DOMove(m_startTarget.position, m_moveTime));
		m_sequence.SetLoops(-1);
	}
}
