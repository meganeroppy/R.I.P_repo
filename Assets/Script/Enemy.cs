﻿using UnityEngine;
using System.Collections;

public class Enemy : Character {

	protected Player m_target = null; 
	protected CircleCollider2D[] m_targetCols = null;
	protected bool m_awaking = false;
	protected float AWAKE_RANGE = 10.24f;
	protected const float WARMINGUP = 1.0f;
	protected Vector2 blow_impact =  new Vector2(200.0f, 100.0f);
	protected float rigorTimer = 0.0f;
	
	protected override void Start ()
	{
		base.Start ();
		attack_power = 5.0f;
	}
	
	protected override void Update ()
	{
		if (!GameManager.GameOver()){
			if(m_target == null){
			m_target = GameObject.FindWithTag ("Player").GetComponent<Player> ();
			}else if(m_targetCols == null){
				m_targetCols = m_target.GetComponents<CircleCollider2D>(); 
			}
		}
		
		base.Update ();
	}

	protected override void ApplyHealthDamage(int val){
		if(current_status == STATUS.GONE){
			return;
		}
		
		base.ApplyHealthDamage(val);
		if (current_health <= 0) {
			if(!m_visible){
				Destroy(this.gameObject);
			}else{
				Instantiate (effectPoint_destroy, transform.position, transform.rotation);
				if(rigidbody2D)
					rigidbody2D.Sleep();
				current_status = STATUS.GONE;
				StartCoroutine(WaitAndExecute(0.9f, true));
				m_collider.isTrigger = true;
			}
		} else {
			StartCoroutine(WaitAndExecute(0.7f, false));
		}
	}
	
	protected virtual void OnEnter(GameObject target){
		
		//Debug.Log("HIT" + Time.realtimeSinceStartup.ToString());
		
		
		if (m_target == null){
			m_target = target.GetComponent<Player> ();
		}
		
		STATUS status = (m_target.GetStatus());
		
		if(status == STATUS.GONE || status == STATUS.DYING || status == STATUS.GHOST_IDLE || status == STATUS.GHOST_DAMAGE){
			return;
		}
		
		m_target.SendMessage ("ApplySpiritDamage", attack_power);
				
		float dir =  target.transform.position.x > transform.position.x ? 1.0f : -1.0f;
		
		if (status != STATUS.GHOST_IDLE && status != STATUS.GHOST_DAMAGE) {
			m_target.rigidbody2D.velocity = Vector2.zero;
			m_target.rigidbody2D.AddForce (new Vector2 (blow_impact.x * dir, blow_impact.y));
		}
	}
	
	protected override void OnTriggerEnter2D(Collider2D col){
		if(current_status == STATUS.GONE){
		return;
		}
		if (col.gameObject.tag == "Player") {
			OnEnter(col.gameObject);
		}
	}
	
	protected override void OnCollisionEnter2D(Collision2D col){
		if(current_status == STATUS.GONE){
			return;
		}
		if (col.gameObject.tag == "Player") {
			OnEnter(col.gameObject);
		}
	}
	
	protected virtual bool PlayerIsInRange(){
		if(Mathf.Abs( transform.position.x - m_target.transform.position.x) < AWAKE_RANGE
		   && Mathf.Abs( transform.position.y - m_target.transform.position.y) < AWAKE_RANGE
		){
			return true;
		}else{
			return false;
		}
	}
	
	
	protected virtual IEnumerator WaitAndExecute(float delay, bool dying){
		yield return new WaitForSeconds (delay);
		renderer.material.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
		invincible = false;
		if (dying) {
			Destroy (this.gameObject);
		}
	}
}