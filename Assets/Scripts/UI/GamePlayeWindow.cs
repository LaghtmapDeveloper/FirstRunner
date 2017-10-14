using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GamePlayeWindow : UIWindow
{
	[SerializeField]
	private UILabel _pointsLabel;
	[SerializeField]
	private MyButton prefab;
	private List<MyButton> _buttons = new List<MyButton> ();
	private List<MyButton> _pool = new List<MyButton> ();

	private List<Vector3> _pos = new List<Vector3> ();

	[SerializeField]
	private int _maxHalfWidth = 960;
	[SerializeField]
	private Transform _container;

	public override UIWindow Open ()
	{
		_pointsLabel.text = "0";
		return base.Open ();
	}

	public void UploadPoses(Transform[] pos)
	{
		_pos.Clear ();
		for (int p = 1; p < pos.Length; p++) 
		{
			var m = Mathf.Clamp(
				transform.InverseTransformPoint( 
					UICamera.currentCamera.ViewportToWorldPoint(
						Camera.main.WorldToViewportPoint(pos[p].position))).x
				,-_maxHalfWidth,_maxHalfWidth);
			
			_pos.Add (new Vector3(m, 0, 0));
		}
	}

	public void ShowButtons ()
	{
		CreateButtons ();
	}

	private void CreateButtons ()
	{
		int index = 1;
		foreach (var p in _pos) 
		{
			MyButton b;
			if (_pool.Count > 0) 
			{
				b = _pool.FirstOrDefault();
				_pool.Remove (b);
				b.gameObject.SetActive (true);
			
				b.transform.localPosition = p;
			} 
			else 
			{
				b = Instantiate (prefab) as MyButton;
				b.transform.parent = _container.transform;
				b.transform.localScale = Vector3.one;
				b.transform.localPosition = p;
				b.OnClick += () => 
				{
					ChooseCompleted(b.Index);
				};
			}

			b.Index = index;
			_buttons.Add (b);
			index++;
		}
	}

	void ChooseCompleted (int index)
	{
		Debug.Log ("called");
		GameMaster.Player.ChoosedSide = index;
		HideButtons ();
	}

	public void HideButtons()
	{
		foreach (var b in _buttons) 
		{
			_pool.Add (b);
			b.gameObject.SetActive (false);
		}
		_buttons.Clear ();
	}
		
	public override UIWindow Close ()
	{
		HideButtons ();
		return base.Close ();
	}

	public void ShowCoins (int score)
	{
		_pointsLabel.text = score.ToString ();
	}
}

