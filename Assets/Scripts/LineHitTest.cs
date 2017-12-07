using System;
using System.Collections.Generic;

namespace UnityEngine.XR.iOS
{
	public class LineHitTest : MonoBehaviour
	{
        public GameObject linePrefab;
        Line activeLine;
        
        // draw line with touch on screen
        bool HitTestWithResultTypeLine(ARPoint point, ARHitTestResultType resultTypes)
        {
            List<ARHitTestResult> hitResults = UnityARSessionNativeInterface.GetARSessionNativeInterface().HitTest (point, resultTypes);
            if (hitResults.Count > 0) {
                foreach (var hitResult in hitResults) {
                    Debug.Log ("Got hit!");
                    if (activeLine != null)
                    {
                        activeLine.UpdateLine(UnityARMatrixOps.GetPosition(hitResult.worldTransform));
                    } 
                    return true;
                }
            }
            return false;
        }
		
		// Update is called once per frame
		void Update () {

            // prioritize reults types
            ARHitTestResultType[] resultTypes = {
                ARHitTestResultType.ARHitTestResultTypeExistingPlaneUsingExtent, 
                // if you want to use infinite planes use this:
                //ARHitTestResultType.ARHitTestResultTypeExistingPlane,
                ARHitTestResultType.ARHitTestResultTypeHorizontalPlane,
                //ARHitTestResultType.ARHitTestResultTypeVerticalPlane, 
                ARHitTestResultType.ARHitTestResultTypeFeaturePoint
            };

            // check for touch on screen
			if (Input.touchCount > 0)
			{
				var touch = Input.GetTouch(0);

                // run if started touching or moving touch
				if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved)
				{

                    if(touch.phase == TouchPhase.Began){
                        Debug.Log("Started touching, created new line");
                        // create a new line
                        GameObject lineGO = Instantiate(linePrefab);
			            activeLine = lineGO.GetComponent<Line>();
                    }
                    
					var screenPosition = Camera.main.ScreenToViewportPoint(touch.position);
					ARPoint point = new ARPoint {
						x = screenPosition.x,
						y = screenPosition.y
					};

                    foreach (ARHitTestResultType resultType in resultTypes)
                    {
                        if (HitTestWithResultTypeLine(point, resultType))
                        {
                            return;
                        }
                    }
				}
                if (touch.phase == TouchPhase.Ended )
                {
                    // stop adding to the activeline (disable it)
                    Debug.Log("Touch ended");
                    activeLine = null;
                }

			}
            
			if (Input.GetMouseButtonDown(0))
			{

                // run if started touching or moving touch
				if (Input.GetMouseButtonDown(0))
				{

                    if(Input.GetMouseButtonDown(0)){
                        Debug.Log("Started touching, created new line");
                        // create a new line
                        GameObject lineGO = Instantiate(linePrefab);
			            activeLine = lineGO.GetComponent<Line>();
                    }

                    Vector3 screenPosition; 
                    screenPosition = Camera.main.ScreenToViewportPoint(Input.mousePosition);

					ARPoint point = new ARPoint {
						x = screenPosition.x,
						y = screenPosition.y
					};

                    foreach (ARHitTestResultType resultType in resultTypes)
                    {
                        if (HitTestWithResultTypeLine(point, resultType))
                        {
                            return;
                        }
                    }
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                // stop adding to the activeline (disable it)
                Debug.Log("Touch ended");
                activeLine = null;
            }
		}

	
	}
}

