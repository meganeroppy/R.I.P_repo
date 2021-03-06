﻿using UnityEngine;
using System.Collections;

public class Exorcised_soul : MonoBehaviour {

	private float counter = 0.0f;
	public Sprite[] pic = new Sprite[2];
	private SpriteRenderer spriteRenderer;
	private int current_frame = 0;
	
	private float speed = 7.5f;
	
	private const float LIFE_TIME = 5.0f;
	private float timer = 0.0f;
	
	void Start(){
		spriteRenderer = GetComponent<SpriteRenderer>();
	
	}
	// Update is called once per frame
	void Update () {
	
		transform.Translate(0.0f, speed * Time.deltaTime, 0.0f);
	
		if (counter >= 0.1f) {
			counter = 0.0f;
			current_frame = current_frame == 0 ? 1 : 0;
			spriteRenderer.sprite = pic [current_frame];
		} else {
			counter += Time.deltaTime;

		}
		
		
		if(timer > LIFE_TIME){
			Destroy(this.gameObject);
		}else{
			timer += Time.deltaTime;
		}
	}
}
