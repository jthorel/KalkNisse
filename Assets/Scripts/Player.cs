using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	public Rigidbody2D rb {
		get { return GetComponent<Rigidbody2D>();}
	}
	public bool isRunning = false;
	public PlayerDup playerDup;
	private GameObject shadowPlayer;

	void Start() {
		shadowPlayer = playerDup.gameObject;
	}
	void Update()
	{
		// if (Input.GetButtonDown("Play"))
		// {
		// 	runPlayer();
		// }
	}

	private void transformPlayer(){
		Vector3 shadow = shadowPlayer.transform.position;
		gameObject.transform.position = new Vector3(shadow.x+1000, shadow.z+1000, -shadow.y);
		gameObject.transform.eulerAngles = new Vector3(0, 0, -shadowPlayer.transform.eulerAngles.y); 
	}

	public void runPlayer(){
		transformPlayer();
		Debug.Log("runPlayer");
		rb.bodyType = RigidbodyType2D.Dynamic;
		isRunning = true;
		playerDup.runPlayer();
	}

	public void reset(){
		isRunning = false;
		playerDup.reset();
		rb.bodyType = RigidbodyType2D.Static;
	}

}
