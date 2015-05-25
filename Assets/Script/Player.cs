using UnityEngine;
using System.Collections;

public class Player : Walker {

	private float losing_rate = 15.0f;
	private float gaining_rate = 0.5f;
	private float gainingFlug = 0.0f;//Check if is in the range of a soulful object
	private float losingFlug = 0.0f;

	private float default_spirit;

	protected Collider2D[] m_colliders;
	protected float m_colTimer = 0.0f;
	
	public GameObject attackZone;
	public GameObject exorcised_soul;
	public GameObject deadPeace;
	public GameObject chargingEffect;
	
	private float m_attackTimer = 0.0f;
	private const float ATTACK_INTERVAL = 0.3f;
	private float m_savedSpiritVal = 0.0f;
	private Vector2[] colPos_living = new Vector2[2];
	private float m_nonGhostTimer = 0.0f;
	
	//Script
	GameManager gameManager;
	
	// Use this for initialization
	protected override void Awake () {
		base.Awake();
		MAX_HEALTH = 1;
		current_health = MAX_HEALTH;
	}
	
	protected override void Start(){
		m_savedSpiritVal = current_spirit;

		GameObject obj = Instantiate(chargingEffect, transform.position + new Vector3(0,0,1), transform.rotation) as GameObject;
		obj.transform.parent = transform;
	}
	
	public override bool init(GameObject caller){
		gameManager = caller.GetComponent<GameManager>();
		return init();
	}
	
	public override bool init(){
		layer_ground = 1 << 8;
		
		if(transform.parent != null){
			transform.parent = null;
		}
		current_health = MAX_HEALTH;
		
		default_spirit = MAX_SPIRIT;
		current_spirit = default_spirit;
		
		current_status = STATUS.IDLE;
		gameObject.layer = LayerMask.NameToLayer("Player");
		
		Flip(SIDE.RIGHT);
		jump_force = JUMP_FORCE_BASE;
		move_speed.x = 0.0f;
		if(rigidbody2D.IsSleeping()){
			rigidbody2D.Sleep();
		}
		rigidbody2D.velocity = Vector2.zero;
		
		if(gameManager == null){
			gameManager = GameObject.Find ("GameManager").GetComponent<GameManager>();
		}
		sound = gameManager.GetComponent<SoundManager>();
		living = true;
		m_colliders = GetComponents<Collider2D> ();
		
		colPos_living[0] = new Vector2(-0.2f, 1.5f);
		colPos_living[1] = new Vector2(-0.2f, 0.5f);
		for (int i = 0 ; i < m_colliders.Length ; i++) {
			//col.isTrigger = false;
			(m_colliders[i] as CircleCollider2D).center = colPos_living[i];
		}
		
		gameManager.EnableUI();
		
		gameManager.SetRespawnPoint(transform.position + new Vector3(0.0f, -3.0f, 0.0f));
	
		GameManager.InformBecomeGhost(false);
		invincible = false;
		
		return true;
	}

	protected override void Update(){
		
	//print(m_savedSpiritVal);
		base.Update ();
		if (current_status == STATUS.GHOST_IDLE || current_status == STATUS.GHOST_DAMAGE) {
			if(current_status == STATUS.GHOST_DAMAGE){
				rigorState -= Time.deltaTime;
				if(rigorState <= 0.0f){
					current_status = STATUS.GHOST_IDLE;
				}
			}
			
			UpdateSpirit(-(losing_rate * Time.deltaTime));
			Color color = new Color(1.0f, 1.0f, 1.0f, current_spirit / MAX_SPIRIT );
			renderer.material.color = color;
			
			losingFlug = 0.1f;
			
			if (current_spirit <= 0.0f) {
				GetExorcised();
			}
		}else{
			if(current_spirit < MAX_SPIRIT && current_status != STATUS.DYING && current_status != STATUS.DAMAGE ){
				UpdateSpirit( gaining_rate * Time.deltaTime );
			}
		}

		if(gainingFlug > 0.0f){
			gainingFlug -= Time.deltaTime;
			
			//Inform is's hidden
			if(!GameManager.CheckCurrentPlayerIsHidden()){
				GameManager.InformBecomeHidden(true);
			}
		}else{
			//Inform is's NOT hidden
			if(GameManager.CheckCurrentPlayerIsHidden()){
				GameManager.InformBecomeHidden(false);
			}	
		}		
		
		if(losingFlug > 0.0f){
			losingFlug -= Time.deltaTime;
		}
		
		if(m_attackTimer > 0.0f){
			m_attackTimer -= Time.deltaTime;
		}
		
		if(m_nonGhostTimer > 0.0f){
			m_nonGhostTimer -= Time.deltaTime;
		}
	}

	protected void OnEnter2D(GameObject col){
		if (col.tag == "Item" || col.tag == "Treasure") {
			Item item = col.GetComponent<Item> ();
			switch (item.GetItemType ()) {
			case "DYING":
				DieAndBecomeGhost ();
				break;
			case "REVIVAL":
				//UpdateSpirit(12.5f);
				if(!living){
					Revive ();
				}
				break;
			case "GAIN_SPIRIT":
				UpdateSpirit(12.5f);
				break;
			case "TREASURE":
				if(!living){
					return;
				}
				break;
			default:
				break;
			}
			sound.PlaySE ("GetItem", 1.0f);
			item.Remove();
		}
	}
	
	protected override void OnCollisionEnter2D(Collision2D col){
		OnEnter2D(col.gameObject);
	}
		
	protected override void OnTriggerEnter2D(Collider2D col){
		OnEnter2D(col.gameObject);
	}

	public void Jump(){
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
		if(current_spirit <= 0.0f){
			GetExorcised();
		}else{
			if(current_status != STATUS.GHOST_IDLE && m_nonGhostTimer <= 0.0f ){
				DieAndBecomeGhost ();
			}
			
		}
	}
	
	public void CancelMotion(){
		if(current_spirit <= 0.0f){
			return;
		}
		
		if(current_health <= 0 && current_status != STATUS.GHOST_IDLE && current_status != STATUS.GHOST_DAMAGE){
			DieAndBecomeGhost ();
		}
	}
	
	protected override void Flip (SIDE side)
	{
		if(current_status == STATUS.DAMAGE || current_status == STATUS.DYING)
			return;
			
		base.Flip (side);
	}

	public void Attack(bool chargedAttack){
		if(m_attackTimer > 0.0f){
			return;
		}
		
		if (/*grounded && */ current_status != STATUS.GHOST_IDLE && current_status != STATUS.DAMAGE && current_health >= 1 ) {
			current_status = STATUS.ATTACK;
			
			Vector3 pos = transform.position;
			Vector3 offset = new Vector3(current_side == SIDE.RIGHT ? 1.3f : -1.3f, 1.5f, -1.0f);
						
			GameObject obj = Instantiate (attackZone, new Vector3 (pos.x + offset.x, pos.y + offset.y, pos.z + offset.z), transform.rotation) as GameObject;
			if(chargedAttack){
				obj.GetComponent<SanmaBlade>().SetAsCharged();
				sound.PlaySE("Attack2", 1.0f);
			}else{
				sound.PlaySE("Attack", 1.0f);
			}
			obj.GetComponent<SanmaBlade>().SetParentAndExecute(this);
			
			rigorState = ATTACK_DURATION;
			anim.SetTrigger("t_attack");
			
			m_attackTimer = ATTACK_INTERVAL;
		}
	}
	
	
	
	protected void DieAndBecomeGhost(){
		
		living = false;
		rigidbody2D.gravityScale = 0.0f;
		rigidbody2D.velocity = Vector2.zero;
		
		m_nonGhostTimer = DISAPPEARING_DELAY;	
		
		foreach (CircleCollider2D col in m_colliders) {
			//col.isTrigger = true;
			
			col.center = new Vector2(col.center.x, 1.0f);
		}
		
		current_status = STATUS.GHOST_IDLE;
		Instantiate (effect_transformation, transform.position - new Vector3(0,0,1), transform.rotation);
		StartCoroutine (ReattachColliders ());
		
		GameObject obj = Instantiate (deadPeace) as GameObject;
		obj.transform.position = transform.position + new Vector3 (0, 0, 0.5f);		obj.SendMessage("Flip", current_side);
		
		m_savedSpiritVal = current_spirit;
		
		gameObject.layer = LayerMask.NameToLayer("Ghost");
		
		if(transform.parent != null){
			transform.parent = null;
		}
		
		GameManager.InformBecomeGhost(true);
	}
	
	
	protected IEnumerator ReattachColliders(){
		foreach (CircleCollider2D col in m_colliders) {
			col.enabled = false;
		}
		yield return new WaitForSeconds(0.1f);
		foreach (CircleCollider2D col in m_colliders) {
			col.enabled = true;
		}
	}
	/// <param name="value">Value.</param>

	protected override void ApplyHealthDamage(int value){
		if(invincible){
			return;
		}
		base.ApplyHealthDamage (value);
	}
	
	protected void HitNeedle(int value){
		invincible = false;
		ApplyHealthDamage(value);
	}
	
	protected override void ApplySpiritDamage(float val){
		if(invincible){
			return;
		}
		
		base.ApplySpiritDamage(val);
		if(current_spirit <= 0.0f){
			current_health = 0;
		}
	}
	
	protected void Revive(){
		living = true;
		rigidbody2D.gravityScale = DEFAULT_GRAVITY_SCALE;
		for (int i = 0 ; i < m_colliders.Length ; i++) {
			//col.isTrigger = false;
			(m_colliders[i] as CircleCollider2D).center = colPos_living[i];
		}
		//renderer.material.color = new Color (1.0f, 1.0f, 1.0f, 1.0f);
		current_status = STATUS.IDLE;
		gameObject.layer = LayerMask.NameToLayer("Player");
		
		
		
		timer_invincible = INVINCIBLE_DURATION;
		invincible = true;
		
		Instantiate (effect_transformation, transform.position, transform.rotation);

		current_health = MAX_HEALTH;
		current_spirit = m_savedSpiritVal;
		
		GameManager.InformBecomeGhost(false);
	}

	public int[] GetLifeInfo(){
		int[] life = {0, 0};
		life[0] = MAX_HEALTH;
		life[1] = current_health;
		return life;
	}

	public float[] GetSpiritInfo(){

		float[] spirit = {0.0f, 0.0f, 0.0f};
		spirit[0] = MAX_SPIRIT;
		spirit[1] = current_spirit;
		spirit[2] = gainingFlug > 0.0f ? 1 : losingFlug > 0.0f ? -1 : 0 ;
		return spirit;
	}
	

	//From soulful object
	protected override void GainSpirit(float val){
		if(current_status != STATUS.DYING && current_status != STATUS.DAMAGE){
			if(living){
				val *= 0.5f;
			}
			base.GainSpirit(val);
			if(m_savedSpiritVal < current_spirit){
				m_savedSpiritVal = current_spirit;
			}
		}
		gainingFlug = 0.1f;
	}
	
	private void UpdateSpirit(float val){
		current_spirit = current_spirit + val < MAX_SPIRIT ? current_spirit + val : MAX_SPIRIT;
	}
	
	private void GetExorcised(){
		
		if(current_status == STATUS.GONE){
			return;
		}
		
		current_status = STATUS.GONE;
	
		renderer.enabled = false;
		gameManager.Miss(true);
		
		move_speed = Vector2.zero;
		rigidbody2D.velocity = Vector2.zero;
		rigidbody2D.Sleep();
		
		foreach (Collider2D col in m_colliders) {
			col.enabled = false;
		}
		
		current_health = 0;
		current_spirit = 0.0f;
		
		Instantiate(effect_transformation, transform.position + new Vector3(0.0f, 0.0f, -1.0f), transform.rotation);
		Instantiate( exorcised_soul, transform.position, transform.rotation);
	}
	
	public void Restart(Vector3 respawnPos){
		float offsetY = 3.0f;
		transform.position = new Vector3(respawnPos.x, respawnPos.y + offsetY, transform.position.z);
		renderer.enabled = true;
		rigidbody2D.gravityScale = DEFAULT_GRAVITY_SCALE;
		for (int i = 0 ; i < m_colliders.Length ; i++) {
			//col.isTrigger = false;
			(m_colliders[i] as CircleCollider2D).center = colPos_living[i];
			m_colliders[i].enabled = true;
			
		}
		anim.SetTrigger("t_init");
		init ();
	}
	
	/*
	protected void OnCollisionStay2D(Collision2D col){
		if(col.gameObject.tag.Equals("Ground")){
			//Vector2 vel = rigidbody2D.velocity;
			//rigidbody2D.AddForce(new Vector2(-vel.x * 100, 0.0f));
			
		}
	}
	*/
	
}
