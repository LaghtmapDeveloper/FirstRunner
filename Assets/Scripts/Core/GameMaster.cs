using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour {

	public static GameMaster Instance;

	public TilesManager TileManager;

	public static PlayerData Player;
	[SerializeField]
	private PlayerController player;
	[SerializeField]
	private TilesManager tm;

	private UIMaster UI;
	public GameObject tileT;
	// Use this for initialization
	#if UNITY_EDITOR
	public 
	#endif
	void Awake () 
	{
		Instance = this;
		DontDestroyOnLoad (this.gameObject);	
		Player = new PlayerData ();
		StartCoroutine (LoadMainScene ());
	}

	private IEnumerator LoadMainScene ()
	{
		var load = SceneManager.LoadSceneAsync ("1");
		while (!load.isDone) 
		{
			yield return null;
		}
		UI = UIMaster.Instance;
		TileManager = Instantiate (tm);
		TileManager.Initialize (Instantiate (player));
		UI.ShowWindow(TypeWindow.MainMenu);
	}

	public void StartGame()
	{
		UI.ShowWindow(TypeWindow.GamePlay);
		TileManager.StartGame ();
	}

	public void EndGame()
	{
		Debug.Log ("HUIPIZDA");
		UI.ShowWindow(TypeWindow.Loser);
		TileManager.EndGame ();
		Player.score = 0;
	}

	public void PrepareRotate ()
	{
		//сделать генерик метод T : UIWindow
		((GamePlayeWindow)UI.GetWindow(TypeWindow.GamePlay)).ShowButtons();
	}

	public void WantRotate ()
	{
		if (Player.ChoosedSide != 0) {
			TileManager.RealizeRotate (Player.ChoosedSide);
			((GamePlayeWindow)UI.GetWindow (TypeWindow.GamePlay)).HideButtons ();
		}

		Player.ChoosedSide = 0;
	}

	public void IncrementScores (int getScore = 1)//default point
	{
		Player.score += getScore;
		//сделать генерик метод T : UIWindow
		((GamePlayeWindow)UI.GetWindow(TypeWindow.GamePlay)).ShowCoins(Player.score);
		Debug.Log (Player.score.ToString ());
	}
}
