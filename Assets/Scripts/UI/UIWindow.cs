using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TypeWindow
{
	Splash, Loser, MainMenu, GamePlay
}

public class UIWindow : MonoBehaviour
{
	public TypeWindow Type;
	[SerializeField]
	private UIPanel _panel;

	public virtual UIWindow Open()
	{
		this.gameObject.SetActive (true);
		return this;
	}

	public virtual UIWindow Close()
	{
		this.gameObject.SetActive (false);
		return this;
	}
}