using UnityEngine;
using UnityEditor;
using System.Collections;
using System;

public class SceneManager : MonoBehaviour {

	public static string currentLevelName = "";
	private static bool initialized = false;
	
	// Use this for initialization
	void Awake () {
		Init("Title");
	}
	
	public void Init(string sceneName){
		if(initialized){
			return;
		}
		currentLevelName = sceneName;
		if(!GameObject.Find("GameManager")){
			LoadLevelAdditive(currentLevelName);
		}	
	}

	public static void LoadLevelAdditive(string levelName){
		initialized = true;
		RemoveAll();
		currentLevelName = levelName;
		Application.LoadLevelAdditive(currentLevelName);		
	}
	
	private static void RemoveAll(){
		// get all objects by type
		foreach (GameObject obj in UnityEngine.Object.FindObjectsOfType(typeof(GameObject)))
		{
			// process it if it's on current scene
			if (obj.activeInHierarchy && !obj.name.Contains("SceneManager"))
			{
				Destroy(obj);
			}
		}
	
	}

}
