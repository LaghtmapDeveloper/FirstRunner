using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

[Serializable]
public class TileCell
{
	public Vector3 LocalCellPosition;

	public TileCell (Vector3 vector3)
	{
		LocalCellPosition = vector3;
	}

	[SerializeField]public bool isFree = true;
}







