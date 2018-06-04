// Copyright (c) komiya_ktk All rights reserved.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AllSceneManager : MonoBehaviour
{
	[SerializeField]
	private GameObject m_bgMask;

	public string m_Scene_title_name = "";
	public string m_Scene_main_name = "";

	// Use this for initialization
	void Start ()
	{
		// 破棄しないように設定する
		DontDestroyOnLoad(this);
		// マスクを非表示にする
		m_bgMask.SetActive (false);
	}

	/// <summary>
	/// シーン変更処理
	/// </summary>
	/// <param name="scaneName">Scane name.</param>
	public void SceneChange(string scaneName)
	{
		StartCoroutine(_SceneChangeSequence(scaneName));
	}

	/// <summary>
	/// 順を追ってシーン遷移処理を実行する
	/// </summary>
	/// <returns></returns>
	private IEnumerator _SceneChangeSequence(string sceneName)
	{
		// マスクを表示する
		m_bgMask.SetActive (true);

		// 非同期でシーン遷移を行う
		// シーンの読み込みが終わるまで
		yield return SceneManager.LoadSceneAsync(sceneName);

		// マスクを非表示にする
		m_bgMask.SetActive (false);
	}

	/// <summary>
	/// 現在のシーン名を取得する
	/// </summary>
	/// <returns>The current scene name.</returns>
	public string GetCurrentSceneName()
	{
		return SceneManager.GetActiveScene ().name;
	}
}
