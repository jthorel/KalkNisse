using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour {

	public LineRenderer lineRenderer;
	public EdgeCollider2D edgeCol;

	List<Vector3> points;

	public void UpdateLine (Vector3 mousePos)
	{
		if (points == null)
		{
			points = new List<Vector3>();
			SetPoint(mousePos);
			return;
		}

		if (Vector3.Distance(points.Last(), mousePos) > .1f)
			SetPoint(mousePos);
	}

	void SetPoint (Vector3 point)
	{
		Debug.Log("Added point " + point.x.ToString() + " " + point.y.ToString() + " " +point.z.ToString());
		points.Add(point);

		lineRenderer.positionCount = points.Count;
		lineRenderer.SetPosition(points.Count - 1, point);

		// if (points.Count > 1)
		// 	edgeCol.points = points.ToArray();
	}

}
