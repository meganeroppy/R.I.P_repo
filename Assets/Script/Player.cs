using UnityEngine;
using System.Collections;

public class Player : Walker {
	private bool living;

	private float losing_rate = 15.0f;
	private float gaining_rate = 1.0f;
	private float gainingFlug = 0.0f;
	private float losingFlug = 0.0f;

	private float default_spirit;

	Collider2D[] m_colliders;
	
	public GameObject attackZone;
	public GameObject exorcised_soul;
	
	//Script
	GameManager gameManager;
	
	// Use this for initialization
	protected override void Awake () {
		base.Awake();
		MAX_HEALTH = 1;
		current_health = MAX_HEALTH;
	}
	
	protected override void Start(){
	
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
		
		manager.SendMessage("ApplyRespawnPoint", transform.position + new Vector3(0.0f, -3.0f, 0.0f));
	
		GameManager.InformBecomeGhost(false);
		invincible = false;
		
		return true;
	}

	protected override void Update(){
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
		}
		
		if(losingFlug > 0.0f){
			losingFlug -= Time.deltaTime;
		}
		
	}

	protected override void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.tag == "Item") {
			Item item = col.gameObject.GetComponent<Item> ();
			switch (item.GetItemType ()) {
			case "DYING":
				DieAndBecomeGhost ();
				break;
			case "REVIVAL":
				UpdateSpirit(12.5f);
				if(current_status == STATUS.GHOST_IDLE){
					Revive ();
				}
				break;
			default:
				break;
			}
			col.gameObject.SendMessage("Remove");
		}
	}

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
		if(current_spirit <= 0.0f){
			GetExorcised();
		}else{
			if(current_status != STATUS.GHOST_IDLE){
				DieAndBecomeGhost ();
			}
		}
	}
	
	protected void CancelMotion(){
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

	protected override void Attack(){
		
		if (/*grounded && */ current_status != STATUS.GHOST_IDLE && current_status != STATUS.DAMAGE && current_health >= 1 ) {
			current_status = STATUS.ATTACK;
			
			Vector3 pos = transform.position;
			Vector3 offset = new Vector3(current_side == SIDE.RIGHT ? 1.7f : -1.7f, 1.5f, -1.0f);
			
			GameObject attack = Instantiate (attackZone, new Vector3 (pos.x + offset.x, pos.y + offset.y, pos.z + offset.z), transform.rotation) as GameObject;
			attack.SendMessage("ApplyParentAndExecute", this);
			sound.PlaySE("Attack", 1.0f);
			rigorState = ATTACK_DURATION;
			anim.SetTrigger("t_attack");
		}
	}
	
	protected void DieAndBecomeGhost(){
		
		living = false;
		rigidbody2D.gravityScale = 0.0f;
		rigidbody2D.velocity = Vector2.zero;
		
		foreach (Collider2D col in m_colliders) {
			col.isTrigger = true;
		}
		
		current_status = STATUS.GHOST_IDLE;
		Instantiate (effect_transformation, transform.position, transform.rotation);
		
		if(transform.parent != null){
			transform.parent = null;
		}
		
		GameManager.InformBecomeGhost(true);
	}

	protected override void ApplyHealthDamage(int value){
		if(invincible){
			return;
		}
		base.ApplyHealthDamage (value);
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
		foreach (Collider2D col in m_colliders) {
			col.isTrigger = false;
		}
		
		//renderer.material.color = new Color (1.0f, 1.0f, 1.0f, 1.0f);
		current_status = STATUS.IDLE;
		timer_invincible = INVINCIBLE_DURATION;
		invincible = true;
		
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

		float[] spirit = {0.0f, 0.0f, 0.0f};
		spirit[0] = MAX_SPIRIT;
		spirit[1] = current_spirit;
		spirit[2] = gainingFlug > 0.0f ? 1 : losingFlug > 0.0f ? -1 : 0 ;
		return spirit;
	}
	
	public bool CheckIsLiving(){
		return living;
	}

	protected override void GainSpirit(float val){
		if(current_status != STATUS.DYING && current_status != STATUS.DAMAGE){
			if(living){
				val *= 0.5f;
			}
			base.GainSpirit(val);
		}
		gainingFlug = 0.1f;
	}
	
	private void UpdateSpirit(float val){
		current_spirit += val;
	}
	
	private void GetExorcised(){
		
		if(GameManager.Miss()){
			return;
		}
	
		renderer.enabled = false;
		gameManager.SendMessage("Miss", true);
		
		this.enabled = false;
		rigidbody2D.Sleep();
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
		foreach (Collider2D col in m_colliders) {
			col.isTrigger = false;
		}
		
		anim.SetTrigger("t_init");
		init ();
	}
	
}
