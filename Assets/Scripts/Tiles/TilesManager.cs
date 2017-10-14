using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using Random = UnityEngine.Random;

public class TilesManager : MonoBehaviour {

	public static TilesManager instance;

	public Transform parentForTiles,player;
	[SerializeField]
	private TilesMove TileMove;

	private List<Tile> stackTiles = new List<Tile>();

	public GameObject coins;
	public GameObject[] _barriers;
	public Tile[] tiles;
	[SerializeField]
	Vector3 defaultLocPos = new Vector3(0,-.8f,-24f);

	public int curRotateTilesOnScene,cntTilesForBarriers;

	public bool stopMoving,startGame;

	//вынести в конфиг-класс мб с цифрами мб 
	private const float tileLength = 16f;
	private const int maxCountPlatform = 10;
	private const int minimalCountForRotate = 3;
	private const int maxCountForRotate = 8;

	public int cntTilesOnScene;
	public int CashedChoise;

	public Action<Tile> DestroyAction;

	//TODO ГЕЙМ МЕНЕДЖЕР ИНИЦИАЛИЗИРУЕТ
	public void Initialize (PlayerController pplayer) 
	{
		TileMove = gameObject.GetComponent<TilesMove> ();
		TileMove.Initialize (pplayer);
		player = pplayer.transform;

		DestroyAction += (tile) => 
		{
			
			//TODO в пул
			var temp = stackTiles.FirstOrDefault();

			if (temp.Type == TileType.Forward) 
				curRotateTilesOnScene--;
			//возможно есть смысл спавнить после декремента curRotateTilesOnScene
			CreateTile();
			//PoolManager.Return (temp.gameObject, temp.side);
			stackTiles.Remove (temp);
			Destroy(temp.gameObject);
		};
	}

	public void StartGame()
	{
		player.position = Vector3.zero;
		parentForTiles.localPosition = defaultLocPos;
		TileMove.Reset ();
		stackTiles.ForEach (t => Destroy(t.gameObject));
		stackTiles.Clear ();
		curRotateTilesOnScene = minimalCountForRotate;
		cntTilesForBarriers = 0;
		startGame = true;
		CreateStartTiles ();
		//все в пул
		//stackTiles.ForEach (t => GameManager.Instance.PoolManager.ToPool (t));
		TileMove.StartGame = true;
		startGame = false;
	}

	public void EndGame ()
	{
		TileMove.Rotation = false;
		TileMove.StartGame = false;
	}

	public void RealizeRotate (int choosed)
	{
		var lastTile = stackTiles.LastOrDefault();
		TileMove.Rotate (lastTile.Rotations[choosed]);
		SimpleSpawn (lastTile.endPos [choosed].transform, CalculateSpawn ());
		CashedChoise = choosed;
		FreezeSpawn (false);
	}

	private void CreateStartTiles ()
	{
		//небольший костылек
		SimpleSpawn (parentForTiles);
		//parentForTiles.localPosition = Vector3.zero;

		for (int i = 0; i <	maxCountPlatform; i++)
			CreateTile ();
	}
		
	public void CreateTile ()
	{
		var tile = stackTiles.LastOrDefault ();
		var count = tile.endPos.Length;
		// если якорей больше чем 1 ( ветка) - 2> варианта события 
		if (count > 1) 
		{
			FreezeSpawn (true);
			((GamePlayeWindow)UIMaster.Instance.GetWindow (TypeWindow.GamePlay))
			.UploadPoses (tile.endPos);
		}
		else
		{
			SimpleSpawn (tile.endPos [tile.Type == TileType.Forward ? 0 : CashedChoise].transform, CalculateSpawn ());
			CashedChoise = 0;
		}
	}
	int counter = 0;
	private void SimpleSpawn (Transform transformq, int index = 0)
	{
		var tile = Instantiate (tiles[index]);
		if (tile.Type != TileType.Forward) 
			curRotateTilesOnScene = maxCountForRotate;
		counter++;
		tile.gameObject.name = counter + "tile";
		tile.transform.SetParent (parentForTiles);
		tile.Init (transformq,DestroyAction);
		stackTiles.Add (tile);


		if (!startGame && tile.Type == TileType.Forward) {
			//барьеры
			SpawnBarriers(tile, Random.Range (0,2));
			//монетки
			SpawnCoins (tile, Random.Range (0,2));
		}

		cntTilesForBarriers++;
		cntTilesOnScene++;
	}

	void FreezeSpawn (bool state)
	{
		foreach (var t in stackTiles) 
		{
			Debug.LogWarningFormat ("{0} {1}",t.name , state ?  "freezed" : "unfrizzed");
			t.Paused = state;
		}
	}
	public bool spawnRotation = true;
	private int CalculateSpawn ()
	{
		return (curRotateTilesOnScene < 1 && spawnRotation ? Random.Range(1,4) : 0);
	}

	// пять утра ниже, не хочу.

	private TileCell CalculateFreeCell (Tile tile)
	{
		List<TileCell> cells = new List<TileCell>();

		foreach (TileCell cell in tile.elements) {
			if (cell.isFree)
				cells.Add (cell);
		}
		return cells[Random.Range (0, cells.Count)];
	}

	void SpawnBarriers (Tile tile, int i)
	{
		if (i == 1) {
			var barrier = Instantiate (_barriers [Random.Range (0, _barriers.Length)]);
			barrier.transform.SetParent (tile.transform);
			barrier.transform.localEulerAngles = Vector3.zero;
			var freeCell = CalculateFreeCell (tile);
			barrier.transform.localPosition = freeCell.LocalCellPosition;
			freeCell.isFree = false;
			cntTilesForBarriers = 0;
		}
	}

	void SpawnCoins (Tile tile, int i)
	{
		if (i == 1) {
			var coin = Instantiate (coins);
			coin.transform.SetParent (tile.transform);
			coin.transform.localEulerAngles = Vector3.zero;
			var freeCellForCoin = CalculateFreeCell (tile);
			coin.transform.localPosition = freeCellForCoin.LocalCellPosition;
			coin.transform.Translate (Random.Range (-3, 3), 0, 0);
			freeCellForCoin.isFree = false;
		}
	}
}