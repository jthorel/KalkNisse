using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public static GameManager instance = null;
	public GameObject player;
	public GameObject canvas;
	public bool running = false;
	public Button runButton;

	void Awake()
	{
		//Check if instance already exists
		if (instance == null)
			
			//if not, set instance to this
			instance = this;
		
		//If instance already exists and it's not this:
		else if (instance != this)
			
			//Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
			Destroy(gameObject);    
		
		//Sets this to not be destroyed when reloading scene
		DontDestroyOnLoad(gameObject);
	}


	public void startGame(){
		running = true;
		canvas.SetActive(true);
		player.SetActive(true);
	}

	public void hideGame(){
		running = false;
		canvas.SetActive(false);
		player.SetActive(false);
	}

	public void removeLines(){
		GameObject[] lines = GameObject.FindGameObjectsWithTag("lineRenderer");
		foreach(GameObject line in lines){
			Destroy(line);
		}
	}

	public void changeRunButton(){
		ColorBlock cb = runButton.colors;
        cb.normalColor = Color.green;
		runButton.colors = cb;
	}
}
