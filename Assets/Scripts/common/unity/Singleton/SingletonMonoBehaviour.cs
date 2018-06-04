// Copyright (c) komiya_ktk All rights reserved.

using UnityEngine;

namespace common.unity.Singleton
{	
	/// <summary>
	/// 汎用シングルトンクラス
	/// </summary>
	public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
	{
		private static T instance;
		public static T Instance {
			get {
				if (instance == null) {
					instance = (T)FindObjectOfType(typeof(T));

					if (instance == null) {
						Debug.LogError (typeof(T) + "is nothing");
					}
				}

				return instance;
			}
		}
	}
}