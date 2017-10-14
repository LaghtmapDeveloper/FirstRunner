using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class UIMaster : MonoBehaviour
{
	public static UIMaster Instance;

	[SerializeField]
	private UIWindow[] windows;

	// Use this for initialization
	void Awake () 
	{
		Instance = this;
		DontDestroyOnLoad (this.gameObject);
	}

	public UIWindow ShowWindow (TypeWindow type)
	{
		foreach (var w in windows) 
		{
			w.Close();
		}
		return windows.FirstOrDefault (w=> w.Type == type).Open();
	}

	public UIWindow GetWindow (TypeWindow type)
	{
		return windows.FirstOrDefault (w=> w.Type == type);
	}
}
