using System.Collections;
using UnityEngine;
using System;

public class MyButton : MonoBehaviour
{
	public Action OnClick;
	public int Index;

	void OnPress(bool pressed)
	{
		if (!pressed && OnClick != null)
			OnClick.Invoke ();
	}
}