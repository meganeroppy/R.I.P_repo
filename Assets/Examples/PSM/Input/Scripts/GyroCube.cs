using UnityEngine;
using System.Collections;

public class GyroCube : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
        GameObject.Find("Cube").SetActive(false);
	}
	
	// Update is called once per frame
	void Update ()
	{
		transform.rotation = Input.gyro.attitude;
	}
}
