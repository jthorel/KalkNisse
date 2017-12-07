using System;
using System.Collections.Generic;

namespace UnityEngine.XR.iOS
{
	public class HitTest : MonoBehaviour
	{
        public GameObject playerObject;
        Transform playerTransform;
        PlayerDup playerScript;

        void Start(){
            playerTransform = playerObject.GetComponent<Transform>();
            playerScript = playerObject.GetComponent<PlayerDup>();
        }


        // place player sprite where camera is pointing (kind of)
        void HitTestWithResultTypePlayer(ARPoint point, ARHitTestResultType resultTypes){
            List<ARHitTestResult> hitResults = UnityARSessionNativeInterface.GetARSessionNativeInterface().HitTest(point, resultTypes);
            if (hitResults.Count > 0) {
                foreach (var hitResult in hitResults) {
                    playerTransform.position = UnityARMatrixOps.GetPosition(hitResult.worldTransform);
                    //playerTransform.rotation = UnityARMatrixOps.GetRotation(hitResult.worldTransform);
                    
                    // TRYING TO ROTATE IT TO THE PLANE HIT
                    // DOESNT WORK?
                    // if(hitResult.type == ARHitTestResultType.ARHitTestResultTypeVerticalPlane){
                    //     playerTransform.eulerAngles = new Vector3(0,0,0);
                    // }
                    // if(hitResult.type == ARHitTestResultType.ARHitTestResultTypeHorizontalPlane){
                    //     playerTransform.eulerAngles = new Vector3(90,0,0);
                    // }
                }
            }
        }
        		
		// Update is called once per frame
		void Update () {

            // prioritize reults types
            ARHitTestResultType[] resultTypes = {
                //ARHitTestResultType.ARHitTestResultTypeExistingPlaneUsingExtent, 
                // if you want to use infinite planes use this:
                //ARHitTestResultType.ARHitTestResultTypeExistingPlane,
                ARHitTestResultType.ARHitTestResultTypeHorizontalPlane,
                //ARHitTestResultType.ARHitTestResultTypeVerticalPlane, 
                //ARHitTestResultType.ARHitTestResultTypeFeaturePoint
            };

            // center of screen, "cast ray" and place player at first hit
            // if player is not running
            if(!playerScript.isRunning){
                
                Vector3 centerPosition = Camera.main.ScreenToViewportPoint(new Vector2(Screen.width/2f,Screen.height/2f));
                ARPoint cPoint = new ARPoint {
                    x = centerPosition.x,
                    y = centerPosition.y
                };

                foreach (ARHitTestResultType resultType in resultTypes)
                {
                    HitTestWithResultTypePlayer(cPoint, resultType);
                }
            }
        }
	}
}

