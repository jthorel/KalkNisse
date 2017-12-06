using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour {

	public LineRenderer lineRenderer;
	private EdgeCollider2D edgeCol {
		get { return GetComponent<EdgeCollider2D>();}
	}

	List<Vector3> points;
	List<Vector2> points2D;

	public void UpdateLine (Vector3 mousePos)
	{
		if (points == null)
		{
			points = new List<Vector3>();
			points2D = new List<Vector2>();
			SetPoint(mousePos);
			return;
		}

		if (Vector3.Distance(points.Last(), mousePos) > .001f)
			SetPoint(mousePos);
	}

	void SetPoint(Vector3 point)
	{
		Debug.Log("Added point " + (point.x+1000f).ToString() + " " + point.y.ToString() + " " +(point.z+1000f).ToString());
		points.Add(point);
		points2D.Add(new Vector2(point.x+1000f, point.z+1000f));

		lineRenderer.positionCount = points.Count;
		lineRenderer.SetPosition(points.Count - 1, point);

		if (points2D.Count > 1)
			edgeCol.points = points2D.ToArray();
	}


}
