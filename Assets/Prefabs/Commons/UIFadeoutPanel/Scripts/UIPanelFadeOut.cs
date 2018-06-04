//-----------------------------------------------------------------------
// Unity Default Set
// Copyright 2017 M.Soft Co.,Ltd. All Rights Reserved.
// Copyright 2017 The Sixwish project All Rights Reserved.
// This software is released under the MIT License.
//  http://opensource.org/licenses/mit-license.php
//-----------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace unityDefaultSet.ui
{
	/// <summary>
	/// UI.Image の透過度を操作してフェードアウトさせるためのスクリプト
	/// シーン名をセットした場合は、フェードアウト後にシーン移動を行う。
	/// なお、フェードアウトと並行して LoadSceneAsync を呼ぶようなものではありません。
	/// </summary>
	public class UIPanelFadeOut : MonoBehaviour
	{
		/// <summary>
		/// The speed.
		/// </summary>
		[SerializeField]
		private float speed = 0.01f;

		/// <summary>
		/// The fadeout panel image.
		/// </summary>
		private Image fadeoutPanelImage;

		/// <summary>
		/// The alpha.
		/// </summary>
		private float colorAlpha = 0f;

		/// <summary>
		/// The name of the next scene.
		/// </summary>
		[SerializeField]
		private string nextSceneName = "Scene01";

		/// <summary>
		/// The is load scene.
		/// </summary>
		private bool isLoadScene = false;

		// Use this for initialization
		void Start () {
			fadeoutPanelImage = GetComponent<Image> ();
		}
		
		// Update is called once per frame
		void Update () {
			if (fadeoutPanelImage && colorAlpha<1) {
				// alphaを加算して黒に変えていく
				fadeoutPanelImage.color = new Color (0f, 0f, 0f, colorAlpha);
				colorAlpha += speed;
			}

			// シーン名が指定されていればシーン移動
			if (colorAlpha >= 1 && !isLoadScene) {
				if (nextSceneName!="") {
					SimpleAsyncSceneLoad ();
				}
				isLoadScene = true;
			}
		}

		/// <summary>
		/// Simples the async scene load.
		/// </summary>
		public void SimpleAsyncSceneLoad()
		{
			StartCoroutine (_asyncLoadScene(nextSceneName));
		}

		/// <summary>
		/// Asyncs the load scene.
		/// </summary>
		/// <returns>The load scene.</returns>
		/// <param name="nextSceneName">Next scene name.</param>
		private IEnumerator _asyncLoadScene(string nextSceneName)
		{
			yield return null;
			AsyncOperation ope = SceneManager.LoadSceneAsync(nextSceneName, LoadSceneMode.Single);

			yield return null;
		}
	}
}