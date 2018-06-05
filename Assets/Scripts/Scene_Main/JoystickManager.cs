using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickManager : MonoBehaviour
{
	// Use this for initialization
	void Start ()
	{
#if UNITY_EDITOR || UNITY_STANDALONE
		this.gameObject.SetActive(false);
#endif
	}
}
