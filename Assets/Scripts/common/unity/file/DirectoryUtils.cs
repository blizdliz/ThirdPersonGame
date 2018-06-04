// Copyright (c) komiya_ktk All rights reserved.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using common.unity.Singleton;

namespace common.unity.file
{
	public class DirectoryUtils : SingletonMonoBehaviour<DirectoryUtils>
	{
		/// <summary>
		/// 指定パスにディレクトリを生成する
		/// </summary>
		/// <returns><c>true</c>, if directory was created, <c>false</c> otherwise.</returns>
		/// <param name="path">Path.</param>
		public DirectoryInfo CreateDirectory(string path)
		{
			Debug.Log("Create" + path);
			if (Directory.Exists(path)) 
			{
				Debug.Log("Directory exists.");
				// フォルダが存在する場合
				return null;
			}
			// ディレクトリを生成する
			return Directory.CreateDirectory( path );
		}

		/// <summary>
		/// 指定パスのディレクトリを削除する
		/// </summary>
		/// <param name="path">Path.</param>
		public void DeleteDirectory(string path)
		{
			Debug.Log("Delete" + path);
			if (Directory.Exists(path)) 
			{
				Debug.Log("Directory exists.");
				// フォルダが存在する場合
				Directory.Delete(path);
			}
		}
	}
}