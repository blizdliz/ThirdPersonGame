using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageRotate : MonoBehaviour
{
	[SerializeField]
	private float m_rotateSpeed = 0.5f;

	// Update is called once per frame
	void Update ()
	{
		transform.Rotate(new Vector3(0, 0, m_rotateSpeed));
	}
}
