using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageMoveManager : MonoBehaviour
{
	[SerializeField]
	private MoveRootConnecter m_moveRootConnecter;

	[SerializeField]
	private StageMove m_stageMove;

	// Use this for initialization
	void Start ()
	{
		// 道順を決める
		m_moveRootConnecter.Init();
		// 移動させる
		m_stageMove.Init(m_moveRootConnecter.GetPositions());
	}
}
