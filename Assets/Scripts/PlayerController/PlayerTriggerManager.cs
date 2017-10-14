using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTriggerManager : MonoBehaviour
{

	void OnTriggerEnter (Collider col)
	{
		switch (col.tag) {
		case  "TileBranch":
			GameMaster.Instance.PrepareRotate();
			break;
		case  "Enemy":
			GameMaster.Instance.EndGame ();
			break;
		case  "Barrier":
			GameMaster.Instance.EndGame ();
			break;
		case  "Coin":
			GameMaster.Instance.IncrementScores();
			//PoolManager.Return (col.gameObject, "coin");
			Destroy(col.gameObject);
			break;
		}
	}
	void OnTriggerExit (Collider col)
	{
		if (col.tag == "TileBranch") 
		{
			Destroy (col);
			GameMaster.Instance.WantRotate ();
		}
	}
}
