using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using common.unity.Singleton;
using TouchControlsKit;

public class Scene_Main_UserController : SingletonMonoBehaviour<Scene_Main_UserController>
{
	[SerializeField]
	private ThirdPersonCharacterContoroller m_thirdPersonController;

	[SerializeField]
	private Cinemachine.CinemachineFreeLook m_freeLookCam;
	void Start()
	{
#if UNITY_EDITOR || UNITY_STANDALONE
		m_freeLookCam.m_YAxis.m_InputAxisName = "Mouse Y";
		m_freeLookCam.m_XAxis.m_InputAxisName = "Mouse X";
#elif UNITY_ANDROID
		m_freeLookCam.m_YAxis.m_InputAxisName = "Stick Y";
		m_freeLookCam.m_XAxis.m_InputAxisName = "Stick X";
#endif
	}

	// Update is called once per frame
	void Update ()
	{
		float v = 0;
		float h = 0;
		bool jump = false;

#if UNITY_EDITOR || UNITY_STANDALONE
		v = Input.GetAxisRaw("Vertical"); // マウスもしくはコントローラスティックの垂直方向の値
		h = Input.GetAxisRaw("Horizontal");   // マウスもしくはコントローラスティックの水平方向の値

		jump = Input.GetButtonDown("Jump"); // キーボードもしくはコントローラのJumpの値

#elif UNITY_ANDROID
		v = TCKInput.GetAxis("Joystick").y; // Joystickの垂直方向の値
		h = TCKInput.GetAxis("Joystick").x; // Joystickの水平方向の値

		jump = TCKInput.GetAction("JumpButton", EActionEvent.Down); // ジャンプボタンの値

		Vector2 look = TCKInput.GetAxis("Touchpad");
		m_freeLookCam.m_YAxis.Value += look.y * 0.01f;
		m_freeLookCam.m_XAxis.Value += look.x * 10f;
#endif

		m_thirdPersonController.SetUserControl(v, h, jump);
	}
}
