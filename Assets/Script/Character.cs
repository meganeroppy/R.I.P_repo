using UnityEngine;
using System.Collections;

//Status
public enum STATUS{
	IDLE,
	WALK,
	ATTACK,
	JUMP_UP,
	JUMP_DOWN,
	DAMAGE,
	DYING,
	GHOST_IDLE,
	GHOST_DAMAGE,
	GONE
}

public class Character : StageObject {
	
	protected STATUS current_status;

	protected float DEFAULT_GRAVITY_SCALE = 2.0f;

	protected Vector2 JUMP_FORCE_BASE = new Vector2 (0.0f, 600.0f);
	protected Vector2 jump_force;
	protected const float WALK_SPEED_BASE = 8.5f;
	protected float attack_power;
	protected const float ATTACK_DURATION = 0.4f;
	protected const float DAMAGE_DURATION = 1.0f;
	protected float rigorState = 0.0f;
	protected const float DYING_DELAY = 1.0f;
	protected const float DISAPPEARING_DELAY = 2.0f;

	protected float rayRange = 0.01f;
	protected Player m_target; 

	[HideInInspector]
	public bool grounded;
	protected LayerMask layer_ground;
	
	protected const float MOVE_SPEED_BASE = 8.5f;
	protected Vector2 move_speed;

	//Animator
	protected Animator anim;
	
	//GameObject
	public GameObject attackZone;
	public GameObject effect_transformation;
	public GameObject effectPoint_destroy;
	
	protected override void Awake(){
		base.Awake();
		anim = GetComponent<Animator> ();
	}

	// Use this for initialization
	protected override void Start () {
		base.Start ();

		layer_ground =  1 << 8;
		current_side = SIDE.LEFT;
		current_status = STATUS.IDLE;
		move_speed.x = 0.0f;
		move_speed = new Vector2 (0.0f, 0.0f);

	}

	// Update is called once per frame
	protected override void Update () {
	
		if (!GameManager.GameOver() && m_target == null){
			m_target = GameObject.FindWithTag ("Player").GetComponent<Player> ();
		}
	
		Vector3 pos = transform.position;

		grounded = Physics2D.Raycast(pos, -Vector2.up, rayRange, layer_ground) 
			?  true : Physics2D.Raycast(pos + new Vector3(0.5f,0.0f,0.0f), -Vector2.up, rayRange, layer_ground)
				? true : Physics2D.Raycast(pos + new Vector3(-0.5f,0.0f,0.0f), -Vector2.up, rayRange, layer_ground);

		if(invincible < 0.0f){
			renderer.material.color = new Color (1.0f, 1.0f, 1.0f, 1.0f);
		}else{
			invincible -= Time.deltaTime;
		}

		switch (current_status) {
		case STATUS.IDLE:
			if(Mathf.Abs(move_speed.x) > 0.05f){
				current_status = STATUS.WALK;
			}else{
				if(!grounded){
					transform.parent = null;
					current_status = rigidbody2D.velocity.y <= 0.0f ? STATUS.JUMP_DOWN : STATUS.JUMP_UP;
				}
			}
			break;
		case STATUS.JUMP_UP:
			if(grounded){
				if(Mathf.Abs( move_speed.x ) < 0.05f){
					move_speed.x = 0.0f;
					current_status = STATUS.IDLE;
				}else{
					current_status = STATUS.WALK;
				}
			}else if(rigidbody2D.velocity.y <= 0.0f){
				current_status = STATUS.JUMP_DOWN;
			}
			transform.position += new Vector3(move_speed.x * WALK_SPEED_BASE * Time.deltaTime, 0.0f, 0.0f);
			
			break;
		case STATUS.JUMP_DOWN:
			if(grounded){
				if(Mathf.Abs( move_speed.x ) < 0.05f){
					move_speed.x = 0.0f;
					current_status = STATUS.IDLE;
				}else{
					current_status = STATUS.WALK;
				}
			}else if(rigidbody2D.velocity.y > 0.0f){
				current_status = STATUS.JUMP_UP;
			}
			
			transform.position += new Vector3(move_speed.x * WALK_SPEED_BASE * Time.deltaTime, 0.0f, 0.0f);
			
			break;
		case STATUS.WALK:
			if(Mathf.Abs( move_speed.x ) < 0.05f){
				move_speed.x = 0.0f;
				current_status = STATUS.IDLE;
			}else{
				if(!grounded){
					transform.parent = null;
					current_status = rigidbody2D.velocity.y <= 0.0f ? STATUS.JUMP_DOWN : STATUS.JUMP_UP;

				}
				transform.position += new Vector3(move_speed.x * WALK_SPEED_BASE * Time.deltaTime, 0.0f, 0.0f);
			}
			break;
		case STATUS.ATTACK:
			rigorState -= 1.0f * Time.deltaTime;
			if(rigorState <= 0){
				current_status = STATUS.IDLE;
			}
			if(!grounded){
			transform.position += new Vector3(move_speed.x * WALK_SPEED_BASE * Time.deltaTime, 0.0f, 0.0f);
			}
			break;
		case STATUS.DAMAGE:
			rigorState -= 1.0f * Time.deltaTime;
			if(rigorState <= 0.0f){
				if(current_health <= 0){
					if(grounded){
						anim.SetTrigger("t_die");
					}
					StartCoroutine (Die ());
					current_status = STATUS.DYING;
					
				}else{
					current_status = STATUS.IDLE;
					invincible = INVINCIBLE_DURATION;
				}
			}
		break;	
		case STATUS.DYING:
			break;
		case STATUS.GHOST_IDLE:
			if(rigidbody2D.velocity != Vector2.zero){
				rigidbody2D.velocity = Vector2.zero;
			}
			transform.position += new Vector3(move_speed.x * MOVE_SPEED_BASE * Time.deltaTime * 0.5f, move_speed.y * MOVE_SPEED_BASE * Time.deltaTime * 0.5f, 0.0f);
			break;
		case STATUS.GONE:
			break;
		default:
			break;	
		}
	}

	protected bool CheckIsJumpable(){
		if (current_status == STATUS.IDLE || current_status == STATUS.WALK) {
			return true;
		} else {
			return false;
		}
	}

	protected void Attack(){
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


	//For Ghost
	public void UpdateMoveSpeed(Vector2 speed){
		move_speed = speed;
		if (move_speed.x > 0.0f) {
			Flip (SIDE.RIGHT);
			current_side = SIDE.RIGHT;
		} else if (move_speed.x < 0.0f) {
			Flip(SIDE.LEFT);
			current_side = SIDE.LEFT;
		}
	}

	protected void OnCollisionStay2D(Collision2D col){
		if (col.gameObject.tag == "MovingFloor") {
			transform.parent = col.transform;
		} else {
			transform.parent = null;		
		}
	}
	protected void OnCollisionExit2D(Collision2D col){
		if (col.gameObject.tag == "MovingFloor") {
			transform.parent = null;		
		} 
	}

	protected virtual void Hit(int value){
		if (current_status != STATUS.DAMAGE && current_status != STATUS.DYING  && current_status != STATUS.GHOST_IDLE) {
			ApplyHealthDamage(value);		
		}
	}

	protected override void ApplyHealthDamage(int value){
		base.ApplyHealthDamage (value);
		if (current_health <= 0) {
			current_status = STATUS.DAMAGE;
			rigorState = DYING_DELAY;
		} else {
			current_status = STATUS.DAMAGE;
			rigorState = DAMAGE_DURATION;
		}
	}

	protected virtual IEnumerator Die(){
		current_status = STATUS.GONE;
		//renderer.material.color = new Color (1.0f, 1.0f, 1.0f, 1.0f);
		renderer.material.color = Color.white;
		
		yield return new  WaitForSeconds(DISAPPEARING_DELAY);
		Instantiate(effectPoint_destroy, transform.position, transform.rotation);
		
		Disappear ();
	}

	virtual protected void Disappear(){
		renderer.enabled = false;
		Destroy (this.gameObject);
	}

	protected IEnumerator WaitAndSwtichStatus(STATUS status, float delay){
		yield return new WaitForSeconds (delay);
		switch (status) {
		case STATUS.JUMP_UP:
			current_status = STATUS.JUMP_UP;
		//	grounded = false;
			break;
		default:
			break;
		}
	}
	
	//Face to the oppsite side
	protected override void Flip(){
		if(current_status == STATUS.DYING){
			return;
		}
		
		SIDE side = this.current_side;
		if (side == SIDE.RIGHT) {
			transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
			current_side = SIDE.LEFT;
		} else {
			transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
			current_side = SIDE.RIGHT;
		}
	}

	public STATUS GetStatus(){
		return current_status;
	}

	public void SetStatus(STATUS status){
		this.current_status = status;
	}
}
