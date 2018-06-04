using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using common.unity.Singleton;

public class Scene_Title_Manager : SingletonMonoBehaviour<Scene_Title_Manager>
{
	// シーン遷移管理用クラス
	private AllSceneManager m_allSceneManager;

	// Use this for initialization
	void Start () 
	{
		GameObject allSceneManagerObj = GameObject.FindGameObjectWithTag ("AllSceneManager");
		if (allSceneManagerObj == null)
		{
			// AllSceneManagerオブジェクトを生成
			allSceneManagerObj = Instantiate (Resources.Load ("Prefabs/AllSceneManager")) as GameObject;
		}
		// AllSceneManagerクラスを取得
		m_allSceneManager = allSceneManagerObj.GetComponent<AllSceneManager>();
		
		// GUIクラスの初期化
		Scene_Title_GUIManager.Instance.Init();
	}
	
	/// <summary>
	/// メインシーンへ遷移
	/// </summary>
	public void SceneChangeToMain()
	{
		m_allSceneManager.SceneChange(m_allSceneManager.m_Scene_main_name);
	}
}
