using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDup : MonoBehaviour {

	public Transform mainCamera;
	public GameObject shadowPlayer;
	public bool isRunning = false;
	private Quaternion _facing;

	void Start(){
		_facing = transform.rotation;
	}

	void Update()
	{
		transformPlayer();
		rotatePlayer();
	
		// if (Input.GetButtonDown("Play"))
		// {
		// 	runPlayer();
		// }
	}

	private void rotatePlayer(){
		if(!isRunning){
			Vector3 direction = Vector3.ProjectOnPlane(mainCamera.forward, Vector3.up);
			Quaternion rotation = Quaternion.LookRotation(direction);
			rotation *= _facing;
			transform.rotation = rotation;
		}

	}
	public void transformPlayer(){
		if(isRunning){
			Vector3 shadow = shadowPlayer.transform.position;
			gameObject.transform.position = new Vector3(shadow.x-1000, -shadow.z, shadow.y-1000);
			gameObject.transform.eulerAngles = new Vector3(90, 0, shadowPlayer.transform.eulerAngles.z); 
		}
	}

    private Quaternion GyroToUnity(Quaternion q) {
        return new Quaternion(q.x, q.y, -q.z, -q.w);
    }


	public void runPlayer(){
		Physics2D.gravity = (new Vector2(transform.up.x, transform.up.z).normalized*-3.82f);
		isRunning = true;
	}

	public void reset(){
		isRunning = false;
	}

}
