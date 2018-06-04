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

namespace unityDefaultSet.ui
{
	/// <summary>
	/// UI.Image の透過度を操作してフェードインさせるためのスクリプト
	/// フェードイン後、自分自身を無効化する。
	/// </summary>
	public class UIFadeinPanel : MonoBehaviour
	{
		/// <summary>
		/// The speed.
		/// </summary>
		[SerializeField]
		private float speed = 0.01f;

		/// <summary>
		/// The fadeout panel image.
		/// </summary>
		private Image fadeinPanelImage;

		/// <summary>
		/// The alpha.
		/// </summary>
		private float colorAlpha = 1f;


		// Use this for initialization
		void Start () {
			fadeinPanelImage = GetComponent<Image> ();
		}
		
		// Update is called once per frame
		void Update () {
			if (fadeinPanelImage && colorAlpha>0) {
				// alphaを減算して透過に変えていく
				fadeinPanelImage.color = new Color (0f, 0f, 0f, colorAlpha);
				colorAlpha -= speed;
			}

			// 完全に透過されたら自分自身を disable にする
			if (colorAlpha <= 0) {
				transform.gameObject.SetActive (false);
			}
		}
	}
}