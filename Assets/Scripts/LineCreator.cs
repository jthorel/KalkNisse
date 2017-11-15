using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineCreator : MonoBehaviour {

	public GameObject linePrefab;

	Line activeLine;

	void Update ()
	{
		if (Input.GetMouseButtonDown(0) || (Input.GetTouch(0).phase == TouchPhase.Began))
		{
			GameObject lineGO = Instantiate(linePrefab);
			activeLine = lineGO.GetComponent<Line>();
		}

		if (Input.GetMouseButtonUp(0) || (Input.GetTouch(0).phase == TouchPhase.Ended ))
		{
			activeLine = null;
		}

		if (activeLine != null)
		{
			Vector2 pos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
			activeLine.UpdateLine(pos);
		} 

	}

}
