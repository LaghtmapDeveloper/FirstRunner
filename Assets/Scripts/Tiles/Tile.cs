using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public enum TileType
{
	Forward,T
}

public delegate void TileDestroyed();

public interface ITile
{
	List<TileCell> elements  { get; }
	TileCell GetFreeRandomCell (float procentStartLimitation);
}

public class Tile : MonoBehaviour, ITile
{
	private Action<Tile> _destroyAction;

	public List<TileCell> elements { get {return _cells; } }

	[SerializeField]private List <TileCell> _cells = new List<TileCell>();

	[SerializeField]private float _sizeX, _sizeY;
	[SerializeField]private Vector2 _cellSize;
	[SerializeField]private Transform plane;

	public TileType Type = TileType.Forward;
	public bool Paused;

	[SerializeField]public Transform[] endPos;

	public Vector3[] Rotations = new Vector3[]{ };

	public TileCell GetFreeRandomCell (float procentStartLimitation)
	{
		throw new System.NotImplementedException ();
	}

	public void Init (Transform transformq, Action<Tile> destroyAction)
	{
		transform.position = transformq.position;
		transform.rotation = transformq.rotation;
		_destroyAction = destroyAction;
	}

	void FixedUpdate ()
	{
		if (Paused)
			return;
		if (transform.position.z < -60f) 
			_destroyAction.Invoke (this);
	}

	#if UNITY_EDITOR
	[ContextMenu("Calculate")]
	public void CalculateCells ()
	{
		var beginPos = plane.transform.localPosition - new Vector3 ( _sizeX/2-_cellSize.x/2, 0,_sizeY/2-_cellSize.y/2);

		_cells.Clear ();

		for (int x = 0; x < _sizeX/_cellSize.x; x++)
			for (int y = 0;y < _sizeY/_cellSize.y; y++)
				_cells.Add (new TileCell (beginPos + new Vector3 (_cellSize.x * x, 0, _cellSize.y * y)));
	}

	[SerializeField]
	private Color colorGizmos = new Color(1,1,0,1);

	void OnDrawGizmos ()
	{
		var vectorSize = new Vector3 (_cellSize.x,0,_cellSize.y);
		Gizmos.color = colorGizmos;
		foreach(var c in _cells)
			Gizmos.DrawWireCube (plane.transform.position + c.LocalCellPosition, vectorSize);
	}
	#endif
}