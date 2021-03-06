﻿using UnityEngine;
using System.Collections;

public class Label_CheckPoint : Monument {
	
	// Use this for initialization
	protected override void Start () {
		
		builtOnGround = true;
		base.Start();
		
		
		m_alpha = 1.0f;
	}
	
	// Update is called once per frame
	protected override void Update () {
	
		if(GameManager.Pause()){
			return;
		}
	
		Vector3 pos = transform.position;
		//Vector3 newPos = new Vector3( pos.x, pos.y  + ( Mathf.PingPong(Time.time * 2.0f, 0.2f) - 0.1f), pos.z);
		Vector3 newPos = new Vector3( pos.x, pos.y  + Time.deltaTime , pos.z);
		transform.position = newPos;
	
		if(m_alpha <= 0.0f){
			Destroy(this.gameObject);
		}else{
			m_alpha -= Time.deltaTime * 0.125f;
			Color clr = new Color(1,1,1,m_alpha);
			GetComponent<Renderer>().material.color = clr;
		}
	}
}
