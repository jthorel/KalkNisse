using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinchToZoom : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		if (Input.touchCount == 2)
		{
			// Store both touches.
			Touch touchZero = Input.GetTouch(0);
			Touch touchOne = Input.GetTouch(1);

			// Find the position in the previous frame of each touch.
			Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
			Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

			// Find the magnitude of the vector (the distance) between the touches in each frame.
			float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
			float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

			// Find the difference in the distances between each frame.
			float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

			GameObject realPlayer = GameObject.FindGameObjectWithTag("realPlayer");
			GameObject duplicatePlayer = GameObject.FindGameObjectWithTag("duplicatePlayer");

			Vector3 scaleVectorReal = realPlayer.transform.localScale + new Vector3(1,1,1) * deltaMagnitudeDiff;
			Vector3 scaleVector = new Vector3(1,1,1) * -deltaMagnitudeDiff * 0.0001f;

			realPlayer.transform.localScale += scaleVector;
			duplicatePlayer.transform.localScale += scaleVector;

			if(realPlayer.transform.localScale.x < 0) {
				realPlayer.transform.localScale = new Vector3(0, 0, 0);
				duplicatePlayer.transform.localScale = new Vector3(0, 0, 0);
			}

		}
	}
}
