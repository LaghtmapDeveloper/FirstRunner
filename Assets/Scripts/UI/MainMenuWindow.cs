using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuWindow : UIWindow
{
	public void StartGame()
	{
		GameMaster.Instance.StartGame ();
	}
}