using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	public Rigidbody rb {
		get { return GetComponent<Rigidbody>();}
	}
	public bool isRunning = false;

	void Update ()
	{
		// if (Input.GetButtonDown("Play"))
		// {
		// 	runPlayer();
		// }
	}

	public void runPlayer(){
		Debug.Log("runPlayer");
		rb.isKinematic = false;
		isRunning = true;
	}

	public void reset(){
		isRunning = false;
		rb.isKinematic = true;
	}

}
