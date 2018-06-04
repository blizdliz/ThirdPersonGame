// Copyright (c) komiya_ktk All rights reserved.

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using common.unity.Singleton;

namespace common.unity.util 
{
	public class UUID : SingletonMonoBehaviour<UUID>
	{
		public string GenerateUUID()
		{
			Guid guid = Guid.NewGuid ();
			return guid.ToString ();
		}
	}
}
