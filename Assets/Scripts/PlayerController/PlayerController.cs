using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerStates {
	Sit,
	Stay,
	Jump
};

public enum Difficult {
	Easy,
	Medium,
	Hard
};

public class PlayerController : MonoBehaviour 
{
	public float speedStrafe = 6f, curSpeed = 8f, sensitivity = 2f;

	public PlayerStates _curPlayerState = PlayerStates.Stay;
	public Difficult _curDifficult = Difficult.Easy;

	Rigidbody _rigidbody;
	Animator _anim;

	void Awake ()
	{
		_anim = GetComponentInChildren<Animator> ();
		_rigidbody = GetComponent<Rigidbody> ();
	}

	void Update () 
	{
		Strafe();
		Jump ();
		Sit ();
	}

	void Strafe()
	{
		var calcMove = transform.position + (transform.right * Input.GetAxisRaw("Horizontal") * speedStrafe * Time.fixedDeltaTime);
		var clampMove = new Vector3 ((Mathf.Clamp (calcMove.x, -3.5f, 3.5f)), transform.position.y, 0);
		_rigidbody.MovePosition (clampMove);
	}

	void Jump ()
	{
		if (Input.GetKeyDown (KeyCode.W)) {
			_anim.SetBool ("jump", true);
		} else {
			_anim.SetBool ("jump", false);
		}
	}

	void Sit ()
	{
		if (Input.GetKeyDown (KeyCode.S)) {
			_anim.SetBool ("sit", true);
		} else {
			_anim.SetBool ("sit", false);
		}
	}

	public void ChangeSpeed (int scores)
	{
		switch (scores) {
		case 5: 
			speedStrafe = 7f; 
			curSpeed = 9f;
			break;
		case 10: 
			speedStrafe = 8f; 
			curSpeed = 10f;
			_curDifficult = Difficult.Medium;
			break;
		case 20: 
			speedStrafe = 9f; 
			curSpeed = 11f;
			break;
		case 30: 
			speedStrafe = 10f; 
			curSpeed = 12f;
			_curDifficult = Difficult.Hard;
			break;
		case 50: 
			speedStrafe = 12f; 
			curSpeed = 15f;
			break;
		}
	}
}
