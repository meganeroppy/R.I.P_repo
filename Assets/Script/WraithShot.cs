﻿using UnityEngine;
using System.Collections;

public class WraithShot : Bullet {

	protected float offsetEularZ = -90.0f;
	public Sprite[] bubble_pic;
	protected int current_frame = 0;
	private float counter = 0.0f;
	
	protected override void Start ()
	{
		base.Start ();
		speed = 7.0f;
		spriteRenderer = GetComponent<SpriteRenderer>();
	}
	
	protected override void Update ()
	{
		base.Update ();
		
		if(counter > 0.2f && !dying){
			counter = 0.0f;
			current_frame = current_frame == 0 ? 1 : 0;
			spriteRenderer.sprite = bubble_pic[current_frame];
		}else{
			counter += Time.deltaTime;
		}
		
		if(!GameManager.CheckCurrentPlayerIsGhost() || GameManager.CheckCurrentPlayerIsHidden()){
			Die();
		}
	}
	
	protected override void OnTriggerEnter2D(Collider2D col){
	return;
	
	/*
		if(dying){
			return;
		}
		
		if (col.gameObject.tag == "Player") {
			Crash(col.gameObject);
		}else if(col.gameObject.tag == "Ground"){
			Die();
		}
		
		*/
	}
	
	protected override void OnCollisionEnter2D(Collision2D col){
		if(col.gameObject.tag.Equals("Player")){
			Crash(col.gameObject);
			Die();
		}
	}	
	
	protected override void ApplyHealthDamage(int value){
	//	Die();
	}
	
	protected override void Die(){
		if(dying){
			return;
		}
			
		dying = true;
		
		rigidbody2D.velocity = Vector2.zero;
		spriteRenderer.sprite = bubble_pic[2];
		Destroy(this.gameObject, 0.25f);
	}
	
	protected override void Crash(GameObject other){
		
		//Debug.Log("HIT" + Time.realtimeSinceStartup.ToString());
		
		Player  m_target = other.GetComponent<Player> ();
		
		if (m_target.GetStatus() != STATUS.DYING) {
			m_target.SendMessage ("ApplySpiritDamage", attack_power);
			float dir = 1.0f;
			if (this.gameObject.transform.position.x > m_target.transform.position.x) {
				dir *= -1.0f;
			}
			m_target.rigidbody2D.velocity = Vector2.zero;
			m_target.rigidbody2D.AddForce (new Vector2 (blow_impact.x * dir, blow_impact.y));
		}
	}
}
