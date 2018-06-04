// Copyright (c) komiya_ktk All rights reserved.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using common.unity.Singleton;

namespace common.unity.util 
{
	/// <summary>
	/// 画面キャプチャ関連処理を扱うクラス
	/// </summary>
	public class Screenshot : SingletonMonoBehaviour<Screenshot>
	{
		/// <summary>
		/// スクリーンショットを撮影しTexture2dとして返す
		/// </summary>
		/// <returns>The screenshot texture2d.</returns>
		/// <param name="camera">Camera.</param>
		public Texture2D GetScreenshotTexture2d(Camera camera)
		{
			Texture2D screenShot = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
			RenderTexture rt = new RenderTexture(screenShot.width, screenShot.height, 24);
			RenderTexture prev = camera.targetTexture;
			camera.targetTexture = rt;
			camera.Render();
			camera.targetTexture = prev;
			RenderTexture.active = rt;
			screenShot.ReadPixels(new Rect(0, 0, screenShot.width, screenShot.height), 0, 0);
			screenShot.Apply();

			return screenShot;
		}

		/// <summary>
		/// スクリーンショットを撮影して指定パスに保存する
		/// </summary>
		/// <returns>The screenshot.</returns>
		/// <param name="path">Path.</param>
		/// <param name="camera">Camera.</param>
		public string TakeScreenshot (string path, Camera camera)
		{
			Texture2D screenShot = GetScreenshotTexture2d (camera);

			byte[] bytes = screenShot.EncodeToPNG();
			UnityEngine.Object.Destroy(screenShot);

			string fileName = "cap_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".png";

			string contentPath = path + "/" + fileName;
			File.WriteAllBytes(contentPath, bytes);

			// メディアスキャン
			_ScanMedia (contentPath);

			return contentPath;
		}

		/// <summary>
		/// 出力ファイルがエクスプローラー上で見えない問題の対策
		/// </summary>
		/// <param name="fileName"></param>
		private void _ScanMedia(string fileName)
		{
			if (Application.platform != RuntimePlatform.Android)
			{
				return;
			}

			#if UNITY_ANDROID
			using (AndroidJavaClass jcUnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			using (AndroidJavaObject joActivity = jcUnityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
			using (AndroidJavaObject joContext = joActivity.Call<AndroidJavaObject>("getApplicationContext"))
			using (AndroidJavaClass jcMediaScannerConnection = new AndroidJavaClass("android.media.MediaScannerConnection"))
			using (AndroidJavaClass jcEnvironment = new AndroidJavaClass("android.os.Environment"))
			using (AndroidJavaObject joExDir = jcEnvironment.CallStatic<AndroidJavaObject>("getExternalStorageDirectory"))
			{
				jcMediaScannerConnection.CallStatic("scanFile", joContext, new string[] { fileName }, new string[] { "image/png" }, null);
			}
			Handheld.StopActivityIndicator();
			#endif
		}
	}
}
