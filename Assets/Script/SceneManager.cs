using UnityEngine;
using System.Collections;
using System;

public class SceneManager : MonoBehaviour {

	public static string currentLevelName = "";

	// Use this for initialization
	void Start () {
		currentLevelName = "Title";
		LoadLevelAdditive(currentLevelName);		
	}

	public static void LoadLevelAdditive(string levelName){
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
