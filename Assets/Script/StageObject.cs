﻿using UnityEngine;
using System.Collections;

public class StageObject : MonoBehaviour {

	protected enum ATK_VAL{
		HEALTH = 0,
		SPIRIT,
	};

	protected float INVINCIBLE_DURATION = 1.5f;
	protected float timer_invincible = 0.0f;
	protected bool invincible;
	

	protected Collider2D m_collider;
	
	protected int current_health = 1;
	protected float current_spirit = 1.0f;
	protected int MAX_HEALTH = 3;
	protected float MAX_SPIRIT = 100.0f;
	
	[HideInInspector]
	public enum TYPE{
		NORMAL,
		GIMIC
	}

	[HideInInspector]
	public enum SIDE
	{
		RIGHT,
		LEFT
	}
	protected SIDE current_side;

	//Script
	protected SoundManager sound;
	
	protected virtual void Awake(){
		sound = GameObject.Find ("GameManager").GetComponent<SoundManager>();
		
	}
	
	// Use this for initialization
	protected virtual void Start () {
	
		invincible = false;
		m_collider = GetComponent<Collider2D> ();
		
	}
	
	protected virtual bool init(GameObject caller){
		transform.parent = caller.transform;
		return init();
	}
	
	protected virtual bool init(){
		return true;
	}

	protected virtual void Update(){}

	protected virtual void Flip(SIDE side){
		if (side == SIDE.RIGHT) {
			transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
			current_side = SIDE.RIGHT;
		} else {
			transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
			current_side = SIDE.LEFT;
		}
	}
	
	protected virtual void LateUpdate(){
	}
	
	//Face to the oppsite side
	protected virtual void Flip(){
		SIDE side = this.current_side;
		if (side == SIDE.RIGHT) {
			transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
			current_side = SIDE.LEFT;
		} else {
			transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
			current_side = SIDE.RIGHT;
		}
	}

	protected virtual void ApplyHealthDamage(int value){
		sound.PlaySE ("Damage", 1.0f);
		current_health -= value;
		invincible = true;
		timer_invincible = INVINCIBLE_DURATION;
		renderer.material.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
	}
	
	protected virtual void ApplySpiritDamage(float value){
		sound.PlaySE ("Damage", 1.0f);
		current_spirit = value >= current_spirit ? 0.0f : current_spirit -= value;
		invincible = true;
		timer_invincible = INVINCIBLE_DURATION;	
		renderer.material.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
	}
	
	protected virtual void GainSpirit(float val){
		if(current_spirit < MAX_SPIRIT){
			if(current_spirit + val > MAX_SPIRIT){
				current_spirit = MAX_SPIRIT;
			}else{
				current_spirit += val;
			}
		}
	}
	
	protected virtual void OnTriggerEnter2D(Collider2D col){
	}
	
}
