using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cheats : MonoBehaviour 
{
	void Update () 
	{
		if (Input.GetKeyDown (KeyCode.V))
			Time.timeScale = Time.timeScale == 1 ? 7 : 1;

		if (Input.GetKeyDown (KeyCode.B))
			GameMaster.Instance.Awake ();
	}
}
