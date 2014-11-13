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
	GHOST,
	GONE
}

public class Character : StageObject {
	
	protected STATUS current_status;

	protected float DEFAULT_GRAVITY_SCALE = 2.0f;

	protected Vector2 JUMP_FORCE_BASE = new Vector2 (0.0f, 600.0f);
	protected Vector2 jump_force;
	protected const float WALK_SPEED_BASE = 8.5f;
	protected float horizontal_move_speed;
	protected float attack_power;
	protected const float ATTACK_DURATION = 0.4f;
	protected const float DAMAGE_DURATION = 1.0f;
	protected float rigorState = 0.0f;
	protected const float DYING_DELAY = 1.0f;
	protected const float DISAPPEARING_DELAY = 2.0f;

	protected Player m_target; 

	[HideInInspector]
	public bool grounded;

	public int layer_ground;

	protected const float MOVE_SPEED_BASE = 8.5f;
	Vector2 move_speed;

	//Animator
	protected Animator anim;
	
	//GameObject
	public GameObject attackZone;
	public GameObject effect_transformation;
	public GameObject effectPoint_destroy;
	
	void Awake(){
		anim = GetComponent<Animator> ();
	}

	// Use this for initialization
	protected override void Start () {
		base.Start ();

		layer_ground =  1 << LayerMask.NameToLayer ("Ground");
		current_side = SIDE.LEFT;
		current_status = STATUS.IDLE;
		//jump_force = JUMP_FORCE_BASE;
		horizontal_move_speed = 0.0f;
		move_speed = new Vector2 (0.0f, 0.0f);
		if (!GameManager.GameOver()){
			m_target = GameObject.FindWithTag ("Player").GetComponent<Player> ();
		}
	}

	// Update is called once per frame
	protected override void Update () {
		Vector3 pos = transform.position;
		//grounded = Physics2D.Linecast (transform.position + transform.up * 1, transform.position - transform.up * 0.1f);

		//
		grounded = Physics2D.Linecast(pos, new Vector3(pos.x, pos.y - 0.01f, pos.z));
		//
		
		if(gameObject.tag == "Player"){
			//Debug.Log(grounded);
		}

		//RaycastHit2D hit = Physics2D.Raycast (new Vector2 (pos.x, pos.y), -Vector2.up, 0.01f);
		// if(hit.collider != null){
		//grounded =  hit.transform.gameObject.layer == 8 ;
		//grounded = hit.transform.gameObject.tag.Equals ("Ground") ? true : false;
		//}
		//Debug.Log (hit.transform.gameObject.layer);
		anim.SetBool("b_jump_down", current_status == STATUS.JUMP_DOWN ? true : false);
 		anim.SetBool("b_jump_up", current_status == STATUS.JUMP_UP ? true : false);
		anim.SetBool("b_run", current_status == STATUS.WALK ? true : false);
		anim.SetBool("b_idle", current_status == STATUS.IDLE ? true : false);
		anim.SetBool("b_ghost", current_status == STATUS.GHOST ? true : false);
		anim.SetBool("b_damaged", current_status == STATUS.DAMAGE ? true : false);
		anim.SetBool("b_dying", current_status == STATUS.DYING ? true : false);
		anim.SetBool("b_grounded", grounded);
		anim.SetBool("b_input", Input.GetAxis("Horizontal") != 0);

		//Debug.Log ("cur_st = " + current_status.ToString() + " / speed = " + horizontal_move_speed.ToString());
		//Debug.Log ("grounded = " + grounded.ToString());

		switch (current_status) {
		case STATUS.IDLE:
			renderer.material.color = new Color (1.0f, 1.0f, 1.0f, 1.0f);
			if(Mathf.Abs(horizontal_move_speed) > 0.05f){
				current_status = STATUS.WALK;
			}else{
				if(!grounded){
					current_status = STATUS.JUMP_DOWN;
					transform.parent = null;
				}
			}
			break;
		case STATUS.JUMP_UP:
			if(grounded){
				if(Mathf.Abs( horizontal_move_speed ) < 0.05f){
					horizontal_move_speed = 0.0f;
					current_status = STATUS.IDLE;
				}else{
					current_status = STATUS.WALK;
				}
			}else if(rigidbody2D.velocity.y <= 0.0f){
				current_status = STATUS.JUMP_DOWN;
			}
			transform.position += new Vector3(horizontal_move_speed * WALK_SPEED_BASE * Time.deltaTime, 0.0f, 0.0f);
			
			break;
		case STATUS.JUMP_DOWN:
			if(grounded){
				if(Mathf.Abs( horizontal_move_speed ) < 0.05f){
					horizontal_move_speed = 0.0f;
					current_status = STATUS.IDLE;
				}else{
					current_status = STATUS.WALK;
				}
			}
			
			transform.position += new Vector3(horizontal_move_speed * WALK_SPEED_BASE * Time.deltaTime, 0.0f, 0.0f);
			
			break;
		case STATUS.WALK:
			if(Mathf.Abs( horizontal_move_speed ) < 0.05f){
				horizontal_move_speed = 0.0f;
				current_status = STATUS.IDLE;
			}else{
				if(!grounded){
					current_status = STATUS.JUMP_DOWN;
					transform.parent = null;

				}
				transform.position += new Vector3(horizontal_move_speed * WALK_SPEED_BASE * Time.deltaTime, 0.0f, 0.0f);
			}
			break;
		case STATUS.ATTACK:
			rigorState -= 1.0f * Time.deltaTime;
			if(rigorState <= 0){
				current_status = STATUS.IDLE;
			}
			if(!grounded){
			transform.position += new Vector3(horizontal_move_speed * WALK_SPEED_BASE * Time.deltaTime, 0.0f, 0.0f);
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
				}
			}
		break;	
		case STATUS.DYING:
			/*
			rigorState -= 1.0f * Time.deltaTime;
			if(rigorState <= 0.0f && grounded){
				StartCoroutine (Die ());
			}
			*/
			break;
		case STATUS.GHOST:
			transform.position += new Vector3(move_speed.x * MOVE_SPEED_BASE * Time.deltaTime * 0.5f, move_speed.y * MOVE_SPEED_BASE * Time.deltaTime * 0.5f, 0.0f);
			break;
		case STATUS.GONE:
			break;
		default:
			break;	
		}
	}
	
	//Added 20141101
	/*
	bool isGrounded = false;
	LayerMask layer;
	
	protected override void LateUpdate(){
		Vector2 pos = transform.position;
		Vector2 groundCheck = new Vector2(pos.x, pos.y - 1.5f);
		Vector2 groundArea = new Vector2(0.5f, 0.5f);
		
		isGrounded = Physics2D.OverlapArea(groundCheck - groundArea, groundCheck + groundArea);// OverLapAerea(Start Pos, End Pos);
	
	} 
*/
	protected bool CheckIsJumpable(){
		if (current_status == STATUS.IDLE || current_status == STATUS.WALK) {
			return true;
		} else {
			return false;
		}
	}

	protected void Attack(){
		if (/*grounded && */ current_status != STATUS.GHOST && current_status != STATUS.DAMAGE && current_health >= 1 ) {
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
	/*
	protected void OnCollisionEnter2D(Collision2D col){
		if (current_status == STATUS.JUMP) {
			if(Mathf.Abs(horizontal_move_speed) < 0.5f){
				current_status = STATUS.IDLE;
			}else{
				current_status = STATUS.WALK;
			}
		}
	}
*/

	public void UpdateWalkSpeed(float speed){

		horizontal_move_speed = speed;
		if (horizontal_move_speed > 0.0f) {
			Flip (SIDE.RIGHT);
			current_side = SIDE.RIGHT;
		} else if (horizontal_move_speed < 0.0f) {
			Flip(SIDE.LEFT);
			current_side = SIDE.LEFT;
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
		if (current_status != STATUS.DAMAGE && current_status != STATUS.DYING  && current_status != STATUS.GHOST) {
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
		renderer.material.color = new Color (1.0f, 1.0f, 1.0f, 1.0f);

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

	public STATUS GetStatus(){
		return current_status;
	}

	public void SetStatus(STATUS status){
		this.current_status = status;
	}
}
