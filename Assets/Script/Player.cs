﻿using UnityEngine;
using System.Collections;


public class Player : Walker {
	private bool living;

	//private Vector3 default_pos;
	private float losing_rate = 20.0f;
	private float gaining_rate = 0.4f;

	private float default_spirit;

	Collider2D[] m_colliders;
	bool debugedDamage = false;
	//Script
	GameManager gameManager;
	
	// Use this for initialization
	protected override void Start () {
	}
	
	protected override bool init(GameObject caller){
		gameManager = caller.GetComponent<GameManager>();
		return init();
	}
	
	protected override bool init(){
		//layer_ground = 1 << LayerMask.NameToLayer ("Ground");
		layer_ground = 1 << 8;
		//		Debug.Log(layer_ground);
		
		if(transform.parent != null){
			transform.parent = null;
		}
		current_health = MAX_HEALTH;
		
		default_spirit = MAX_SPIRIT;
		current_spirit = default_spirit;
		
		current_status = STATUS.IDLE;
		Flip(SIDE.RIGHT);
		jump_force = JUMP_FORCE_BASE;
		move_speed.x = 0.0f;
		if(rigidbody2D.IsSleeping()){
			rigidbody2D.Sleep();
		}
		rigidbody2D.velocity = Vector2.zero;
		
		GameObject manager = GameObject.Find ("GameManager");
		sound = manager.GetComponent<SoundManager>();
		living = true;
		m_colliders = GetComponents<Collider2D> ();
		
		manager.SendMessage("EnableUI");
		GameObject.FindWithTag("MainCamera").GetComponent<MainCamera>().enabled = true;
		
		//Set Position relate to the game advances
		//transform.position = GameManager.GetCurrentRespawnPosition();
		
		manager.SendMessage("ApplyRespawnPoint", transform.position + new Vector3(0.0f, -3.0f, 0.0f));
	
		GameManager.InformBecomeGhost(false);
		
	
		return true;
	}

	protected override void Update(){
								
		base.Update ();
		if (current_status == STATUS.GHOST || debugedDamage) {
			current_spirit -= losing_rate * Time.deltaTime ;	
			Color color = new Color(1.0f, 1.0f, 1.0f, current_spirit / MAX_SPIRIT );
			renderer.material.color = color;
			if (current_spirit <= 0.0f) {
					Disappear();
				}
		}else{
			if(current_spirit < MAX_SPIRIT){
				current_spirit += gaining_rate * Time.deltaTime;
			}
		}

		//current_status = Input.GetKey(KeyCode.O) ? STATUS.GHOST : STATUS.IDLE;

	}

	protected override void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.tag == "Item") {
			Item item = col.gameObject.GetComponent<Item> ();
			switch (item.GetItemType ()) {
			case "DYING":
					DieAndBecomeGhost ();
					break;
			case "REVIVAL":
					GainSpirit(25.0f);
					Revive ();
					break;
			default:
					break;
			}
			col.gameObject.SendMessage("Remove");
		}
	}

	/*
	protected void OnCollisionExit2D(Collision2D col){
		if (col.gameObject.tag == "MovingFloor") {
			transform.parent = null;		
		}
	}
*/
	protected void Jump(){
		if ( grounded && (current_status == STATUS.WALK || current_status == STATUS.IDLE )) {
			rigidbody2D.AddForce (JUMP_FORCE_BASE);
			anim.SetTrigger("t_jump_start");
			sound.PlaySE("Jump", 0.5f);
			current_status = STATUS.JUMP_UP;
			transform.parent = null;
		}
	}


	protected override IEnumerator Die(){
		current_status = STATUS.DAMAGE;
		anim.SetTrigger("t_die");
		renderer.material.color = new Color (1.0f, 1.0f, 1.0f, 1.0f);
		
		yield return new  WaitForSeconds(DISAPPEARING_DELAY);
		DieAndBecomeGhost ();
	}
	
	protected override void Flip (SIDE side)
	{
		if(current_status == STATUS.DAMAGE || current_status == STATUS.DYING)
			return;
			
		base.Flip (side);
	}

	
	protected void DieAndBecomeGhost(){
		living = false;
		rigidbody2D.gravityScale = 0.0f;
		
		foreach (Collider2D col in m_colliders) {
			col.isTrigger = true;
		}
		
		rigidbody2D.velocity = new Vector2 (0.0f, 0.0f);
		current_status = STATUS.GHOST;
		Instantiate (effect_transformation, transform.position, transform.rotation);
		
		if(transform.parent != null){
			transform.parent = null;
		}
		
		GameManager.InformBecomeGhost(true);
	}

	protected override void ApplyHealthDamage(int value){
		if(invincible >= 0.0f){
			return;
		}
		base.ApplyHealthDamage (value);
	}
	
	protected override void Hit(int value){
		base.Hit(value);
		if(current_status == STATUS.GHOST){
			ApplySpiritDamage(value * 25.0f);
		}
	}
	
	protected override void Disappear(){
		Instantiate(effect_transformation, transform.position, transform.rotation);
		
		Miss();
	}

	protected void Revive(){
		living = true;
		rigidbody2D.gravityScale = DEFAULT_GRAVITY_SCALE;
		foreach (Collider2D col in m_colliders) {
			col.isTrigger = false;
		}
		
		//renderer.material.color = new Color (1.0f, 1.0f, 1.0f, 1.0f);
		current_status = STATUS.IDLE;
		invincible = INVINCIBLE_DURATION;
		
		Instantiate (effect_transformation, transform.position, transform.rotation);

		current_health = MAX_HEALTH;
		
		GameManager.InformBecomeGhost(false);
	}

	public int[] GetLifeInfo(){
		int[] life = {0, 0};
		life[0] = MAX_HEALTH;
		life[1] = current_health;
		return life;
	}

	public float[] GetSpiritInfo(){

		float[] spirit = {0.0f, 0.0f};
		spirit[0] = MAX_SPIRIT;
		spirit[1] = current_spirit;
		return spirit;
	}
	
	public bool CheckIsLiving(){
		return living;
	}

	protected override void GainSpirit(float val){
		base.GainSpirit(val);
	}
	
	public void Miss(){
		renderer.enabled = false;
		gameManager.SendMessage("Miss", true);
		this.enabled = false;
		rigidbody2D.Sleep();
	}
	
	public void Restart(Vector3 respawnPos){
		float offsetY = 3.0f;
		transform.position = new Vector3(respawnPos.x, respawnPos.y + offsetY, transform.position.z);
		renderer.enabled = true;
		rigidbody2D.gravityScale = DEFAULT_GRAVITY_SCALE;
		foreach (Collider2D col in m_colliders) {
			col.isTrigger = false;
		}
		
		init ();
	}
	
}
