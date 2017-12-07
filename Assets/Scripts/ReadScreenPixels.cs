using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Linq;
using System;
using UnityEngine.SceneManagement;

namespace UnityEngine.XR.iOS
{
	public class ReadScreenPixels : MonoBehaviour {

		private List<Vector3> positions = new List<Vector3>();
		private List<Vector3> positionsTemp = new List<Vector3>();
		private List<Vector3> positionsTotal = new List<Vector3>();
		private List<Vector2> edgePositions = new List<Vector2>();
		private Texture2D screenImage;
		private List<Vector3> screenPositions;

		public GameObject linePrefab;
		Line activeLine;

		public float maxDifference = 100;
		public float threshold = 1;
		public int neighborOffset = 10;
		public int pixelOffset = 100;

		private bool enabledLines = false;

		public Sprite spriteDefault;
		public Sprite spriteChanged;

		public bool linesWereHidden = false;

		// Use this for initialization
		void Start () {

		}

		public void hideLines(bool changeSprite) {

			enabledLines = !enabledLines;
			GameObject[] lines = GameObject.FindGameObjectsWithTag("lineRenderer");

			if(changeSprite) {
				GameObject sprite = GameObject.Find("SaveLine");
				Image spriteImage = sprite.GetComponent<Image>();

				if(enabledLines) {
					spriteImage.sprite = spriteChanged;
				} else {
					spriteImage.sprite = spriteDefault;
				}
			}

			foreach (GameObject line in lines)
			{
				LineRenderer lr = line.GetComponent<LineRenderer>();
				lr.enabled = enabledLines;
			}

		}


		public void saveLine(){
			hideLines(false);
			GameManager.instance.hideGame();
			StartCoroutine(CaptureScreenshot("screenshot", ScreenshotFormat.PNG));
		}
		// Update is called once per frame
		void Update () {
			// if(Input.GetKeyDown("space") || Input.touchCount > 0) {
			// 	saveLine();
			// }
		}

		// draw line where hit in world space
        // bool HitTestWithResultTypeLine(ARPoint point, ARHitTestResultType resultTypes)
        // {
		// 	Debug.Log("hit test");
        //     List<ARHitTestResult> hitResults = UnityARSessionNativeInterface.GetARSessionNativeInterface().HitTest(point, resultTypes);
        //     if (hitResults.Count > 0) {
        //         foreach (var hitResult in hitResults) {
        //             Debug.Log ("Got hit!");
        //             if (activeLine != null)
        //             {
        //                 activeLine.UpdateLine(UnityARMatrixOps.GetPosition(hitResult.worldTransform));
        //             }
        //             return true;
        //         }
        //     }
        //     return false;
        // }

		IEnumerator CaptureScreenshot(string filename, ScreenshotFormat screenshotFormat)
		{
			//Wait for end of frame
			yield return new WaitForEndOfFrame();

			screenImage = new Texture2D(Screen.width, Screen.height);
			//Get Image from screen
			screenImage.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
			screenImage.Apply();

				// RemoveGameObject("TestCube");
				// RemoveGameObject("Line");
		//
				// positions.Clear();
				// edgePositions.Clear();

				Color32[] pix = screenImage.GetPixels32();

				for(int a = 0; a<pix.Length-1; a+=pixelOffset) {

					int xVal = (int) a % screenImage.width;
					int yVal = (int) a / screenImage.width;

					float neighborColor = 0f;
					Color pixelColor;
					int successCheck = 0;


					for(int i = -1; i<2; i++) {
						for(int j = -1; j<2;j++) {
							if(i==0 && j==0) {
							} else {

								//int neighborIndex = (xVal+i*10) + screenImage.width * (yVal+j*10);
								try {
									int neighborIndex = a - (i * neighborOffset * screenImage.width + j * neighborOffset);
									pixelColor = pix[neighborIndex];
									neighborColor += pixelColor.r + pixelColor.g + pixelColor.b;
									successCheck++;
							}
							catch (Exception e) {
								//print("error");
							}
							}
						}
					}

					neighborColor = neighborColor/successCheck;

					pixelColor = pix[a];

					float sumColor = pixelColor.r + pixelColor.g + pixelColor.b;

					if(sumColor < neighborColor * threshold) {
					//if(sumColor < 1f) {

						Vector3 coordinate = new Vector3(xVal,yVal,0);
						positions.Add(coordinate);
					}

				}

				while(positions.Count>0) {

					Vector3 lastCoord = positions[positions.Count-1];
					positionsTemp.Add(lastCoord);
					positions.Remove(lastCoord);

					int step = 0;

					while(step<positionsTemp.Count) {
						for(int i = positions.Count-1; i>0;i--) {
							Vector3 diff = positionsTemp[step] - positions[i];
							if(diff.magnitude<maxDifference) {

								positionsTemp.Add(positions[i]);
								positions.Remove(positions[i]);
							}
						}
						step+=1;
					}

					// Vector3[] sortedVectors = positionsTemp.OrderBy(v => v.x).ToArray<Vector3>();
					// positionsTemp = sortedVectors.ToList();

					DrawLine();
					positionsTemp.Clear();

				}

			// string filePath = Path.Combine(Application.persistentDataPath, "images");
			// byte[] imageBytes = null;

			// //Convert to png/jpeg/exr
			// if (screenshotFormat == ScreenshotFormat.PNG)
			// {
			// 	filePath = Path.Combine(filePath, filename + ".png");
			// 	createDir(filePath);
			// 	imageBytes = screenImage.EncodeToPNG();
			// }
			// else if (screenshotFormat == ScreenshotFormat.JPEG)
			// {
			// 	filePath = Path.Combine(filePath, filename + ".jpeg");
			// 	createDir(filePath);
			// 	imageBytes = screenImage.EncodeToJPG();
			// }
			// else if (screenshotFormat == ScreenshotFormat.EXR)
			// {
			// 	filePath = Path.Combine(filePath, filename + ".exr");
			// 	createDir(filePath);
			// 	imageBytes = screenImage.EncodeToEXR();
			// }

			// //Save image to file
			// System.IO.File.WriteAllBytes(filePath, imageBytes);
			// Debug.Log("Saved Data to: " + filePath.Replace("/", "\\"));
			
			hideLines(false);
			GameManager.instance.startGame();
		}

		void DrawLine() {
			//LineRenderer lineR = line.GetComponent<LineRenderer>();
			// Vector3 linePos = new Vector3(0,0,0);
			//Quaternion lineRot = Quaternion.identity;
			//GameObject Line = Instantiate(line, linePos, Quaternion.identity);
			//Line.transform.localPosition = linePos;
			//LineRenderer lineR = Line.GetComponent<LineRenderer>();
			GameObject lineGO = Instantiate(linePrefab);
			activeLine = lineGO.GetComponent<Line>();
			lineGO.GetComponent<LineRenderer>().enabled = enabledLines;

            ARHitTestResultType[] resultTypes = {
                ARHitTestResultType.ARHitTestResultTypeExistingPlaneUsingExtent,
                // if you want to use infinite planes use this:
                ARHitTestResultType.ARHitTestResultTypeExistingPlane,
                ARHitTestResultType.ARHitTestResultTypeHorizontalPlane,
                //ARHitTestResultType.ARHitTestResultTypeVerticalPlane,
                //ARHitTestResultType.ARHitTestResultTypeFeaturePoint
            };

			//activeLine.positionCount = positionsTemp.Count;
			//Debug.Log(positionsTemp.Count);


			Vector3[] sortedVectors = positionsTemp.OrderBy(v => v.x).ToArray<Vector3>();
			positionsTemp = sortedVectors.ToList();
			// float scaleFactorX = 1f;
			// float scaleFactorY = 1f;

			for (int i = 0; i < positionsTemp.Count; i++)
			{
				Vector3 screenPosition = Camera.main.ScreenToViewportPoint(positionsTemp[i]);
				if (screenPositions == null)
				{
					screenPositions = new List<Vector3>();
					screenPositions.Add(screenPosition);
					ARPoint point = new ARPoint {
						x = screenPosition.x,
						y = screenPosition.y
					};

					foreach(ARHitTestResultType resultType in resultTypes)
					{
						List<ARHitTestResult> hitResults = UnityARSessionNativeInterface.GetARSessionNativeInterface().HitTest(point, resultType);
						if (hitResults.Count > 0) {
							foreach (var hitResult in hitResults) {
								activeLine.UpdateLine(UnityARMatrixOps.GetPosition(hitResult.worldTransform));
							}
						}
					}
				} else {
					if(Vector3.Distance(screenPositions.Last(), screenPosition) > .05f){
						screenPositions.Add(screenPosition);
						ARPoint point = new ARPoint {
							x = screenPosition.x,
							y = screenPosition.y
						};

						foreach(ARHitTestResultType resultType in resultTypes)
						{
							List<ARHitTestResult> hitResults = UnityARSessionNativeInterface.GetARSessionNativeInterface().HitTest(point, resultType);
							if (hitResults.Count > 0) {
								foreach (var hitResult in hitResults) {
									activeLine.UpdateLine(UnityARMatrixOps.GetPosition(hitResult.worldTransform));
								}
							}
						}
						//Vector3 pos = new Vector3(positions[i].x, positions[i].y, 0f);
						//lineR.SetPosition(i, pos);
					}
				}



				// float posX = (float)(positionsTemp[i].x-screenImage.width/2) * scaleFactorX;
				// float posY = (float)(positionsTemp[i].y-screenImage.height/2) * scaleFactorY;
				// Vector3 screenPosition = Camera.main.ScreenToViewportPoint(positionsTemp[i]);
			}

		}

		void createDir(string dir)
		{
			//Create Directory if it does not exist
			if (!Directory.Exists(Path.GetDirectoryName(dir)))
			{
				Directory.CreateDirectory(Path.GetDirectoryName(dir));
			}
		}

		public enum ScreenshotFormat
		{
			PNG, JPEG, EXR
		}

	}

}
