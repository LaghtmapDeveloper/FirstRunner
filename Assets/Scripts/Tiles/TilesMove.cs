using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilesMove : MonoBehaviour {

	[SerializeField] private Transform target,whoWeRotate;
	[SerializeField] private TilesManager _TManager;
	[SerializeField] private PlayerController _pController;

	public Vector3 targetAngle;
	public bool Rotation = false;

	public bool StartGame {get { return _moving;}set {_moving = value;_TManager.startGame = value; } }

	private float rotationSpeed = 500f, deltaMove;
	private bool _moving = false;

	public void Initialize(PlayerController pController)
	{
		_TManager = gameObject.GetComponent<TilesManager> ();
		_pController = pController;
		whoWeRotate = pController.transform;
	}

	public void Reset ()
	{
		target.localEulerAngles = Vector3.zero;
	}

	void FixedUpdate () 
	{
		if (_moving)
			MoveTiles ();
	}

	void MoveTiles () 
	{
		deltaMove = Time.fixedDeltaTime * _pController.curSpeed;
		_TManager.parentForTiles.position -= whoWeRotate.forward * deltaMove;
	}
		
	public void Rotate(Vector3 angle) {
		Rotation = true;
 		
		targetAngle = angle;
		StartCoroutine (RotateMeNow ());
	}

	private IEnumerator RotateMeNow() 
	{ 
		_moving = false;
		float currentAngle = 0f; 
		float step = rotationSpeed * Time.fixedDeltaTime; 
		while (Rotation) 
		{ 
			if (targetAngle.y > 0) 
			{
				if (currentAngle + step > targetAngle.y ) { 
					step = targetAngle.y  - currentAngle; 
					target.Rotate (Vector3.up, step); 
					Rotation = false;
				} 
				currentAngle += step; 
				target.Rotate(Vector3.up, step); 
			} 
			if (targetAngle.y  < 0) 
			{
				if (currentAngle - step < targetAngle.y ) { 
					step = targetAngle.y + -currentAngle; 
					target.Rotate (Vector3.up, step); 
					Rotation = false;
				} 
				currentAngle -= step; 
				target.Rotate(Vector3.up, -step); 
			} 
			yield return new WaitForFixedUpdate(); 
		}
		_moving = !_moving;
	}
}