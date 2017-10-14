using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAgent : MonoBehaviour
{
	[SerializeField]Text textScore;
	[SerializeField]Text finalScore;
	[SerializeField]GameObject gameOverPanel;
	[SerializeField]GameObject left;
	[SerializeField]GameObject right;

	public void RepaintScore (int scores)
	{
		textScore.text = scores.ToString();
//		PlayerController.instance.ChangeSpeed (scores);
	}

	public void ScreenRotate (string arrow)
	{
		switch (arrow) {
		case "TilesLeft":
			left.SetActive (true);
			break;
		case "TilesRight":
			right.SetActive (true);
			break;
		}
	}

	public void ShowGameOver (int scores)
	{
		finalScore.text = scores.ToString ();
		gameOverPanel.SetActive (true);
	}
}
