using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoseWindow : UIWindow 
{
	[SerializeField]
	private UILabel _pointsLabel;

	public override UIWindow Open ()
	{
		_pointsLabel.text = GameMaster.Player.score.ToString();
		return base.Open ();
	}

	public void Restart()
	{
		GameMaster.Instance.StartGame ();
	}

	public void ShowPoints(int points)
	{
		_pointsLabel.text = points.ToString ();
	}
}
