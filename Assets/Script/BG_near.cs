﻿using UnityEngine;
using System.Collections;

public class BG_near : MonoBehaviour {

	public float moveRate = 0.25f;
	
	private Vector3 bornPos;
	private Vector3 distanceFromOrigin;
	private GameObject target;
	private float OffsetY = 5.0f;
	//private float OffsetY = 0.0f;
	
	void Start(){
		target = GameObject.FindWithTag("Player");
		//target = GameObject.FindWithTag("MainCamera");
		bornPos = target.transform.position; 
	}
	
	// Update is called once per frame
	void Update () {
		if(target == null || GameManager.Miss()){
			return;
		}
		
		distanceFromOrigin = target.transform.position - bornPos;
		Vector3 newPos = new Vector3(-distanceFromOrigin.x * moveRate, target.transform.position.y + OffsetY, 0.0f);
		transform.position = newPos;
	}
}
