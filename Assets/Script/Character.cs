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
	
	protected bool living = true;
	
	protected STATUS current_status;

	protected float DEFAULT_GRAVITY_SCALE = 2.0f;

	protected Vector2 JUMP_FORCE_BASE = new Vector2 (0.0f, 600.0f);
	protected Vector2 jump_force;
	protected const float WALK_SPEED_BASE = 8.5f;
//	protected const float WALK_SPEED_BASE = 2500.0f;
//	protected const float WALK_SPEED_MAX = 10.0f;
	
	protected float attack_power;
	protected const float ATTACK_DURATION = 0.4f;
	protected const float DAMAGE_DURATION = 1.0f;
	protected float rigorState = 0.0f;
	protected const float DYING_DELAY = 1.0f;
	protected const float DISAPPEARING_DELAY = 2.0f;

	protected float rayRange = 0.01f;

	[HideInInspector]
	public bool grounded;
	
	protected LayerMask layer_ground;
	protected LayerMask layer_spikyWire;
	
	protected const float MOVE_SPEED_BASE = 8.5f;
//	protected const float MOVE_SPEED_BASE = 500.0f;
//	protected const float MOVE_SPEED_MAX = 30.0f;
	protected Vector2 move_speed;

	//Animator
	protected Animator anim;
			
	protected CircleCollider2D[] m_cols;
	
	protected override void Awake(){
		base.Awake();
		anim = GetComponent<Animator> ();
	}

	// Use this for initialization
	protected override void Start () {
		base.Start ();

		layer_ground =  1 << 8;
		//layer_ground = LayerMask.NameToLayer("Ground");
		layer_spikyWire = LayerMask.NameToLayer("SpikyWire");
		current_side = SIDE.LEFT;
		current_status = STATUS.IDLE;
		move_speed.x = 0.0f;
		move_speed = new Vector2 (0.0f, 0.0f);
		
		m_cols = GetComponents<CircleCollider2D>();

	}

	// Update is called once per frame
	protected override void Update () {
	
		Vector3 pos = transform.position;
		/*
		grounded = Physics2D.Raycast(pos, -Vector2.up, rayRange, layer_ground) || Physics2D.Raycast(pos, -Vector2.up, rayRange, layer_spikyWire) 
			?  true : Physics2D.Raycast(pos + new Vector3(0.5f,0.0f,0.0f), -Vector2.up, rayRange, layer_ground) || Physics2D.Raycast(pos + new Vector3(0.5f,0.0f,0.0f), -Vector2.up, rayRange, layer_spikyWire)
				? true : Physics2D.Raycast(pos + new Vector3(-0.5f,0.0f,0.0f), -Vector2.up, rayRange, layer_ground) || Physics2D.Raycast(pos + new Vector3(-0.5f,0.0f,0.0f), -Vector2.up, rayRange, layer_spikyWire);
		*/
		grounded = Physics2D.Raycast(pos, -Vector2.up, rayRange, layer_ground)  
			?  true : Physics2D.Raycast(pos + new Vector3(0.5f,0.0f,0.0f), -Vector2.up, rayRange, layer_ground) 
				? true : Physics2D.Raycast(pos + new Vector3(-0.5f,0.0f,0.0f), -Vector2.up, rayRange, layer_ground) ;
		
		if(!invincible){
			if(renderer.material.color != Color.white){
				renderer.material.color = Color.white;
			}
		}else{
			if(current_status != STATUS.DAMAGE){
				timer_invincible -= Time.deltaTime;
				if(timer_invincible <= 0.0f ){
					invincible = false;
				}
			}
		}
		
		if(GameManager.GameClear()){
			move_speed = Vector2.zero;
		}

		switch (current_status) {
		case STATUS.IDLE:
			if(Mathf.Abs(move_speed.x) > 0.05f){
				current_status = STATUS.WALK;
			}else{
				if(!grounded){
					//transform.parent = null;
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
				if(current_health <= 0 || current_spirit <= 0.0f){
					if(grounded){
						anim.SetTrigger("t_die");
					}
					StartCoroutine (Die ());
					current_status = STATUS.DYING;
					
				}else{
					current_status = STATUS.IDLE;
					//invincible = true;
					//timer_invincible = INVINCIBLE_DURATION;
				}
			}
		break;

		case STATUS.DYING:
			break;
		case STATUS.GHOST_IDLE:
		
			if(rigidbody2D.velocity != Vector2.zero){
				rigidbody2D.velocity = Vector2.zero;
			}
			//Ghost move
			transform.position += new Vector3(move_speed.x * MOVE_SPEED_BASE * Time.deltaTime * 0.7f, move_speed.y * MOVE_SPEED_BASE * Time.deltaTime * 0.7f, 0.0f);
			break;
			
		case STATUS.GONE:
		
			move_speed = Vector2.zero;
			
			if(rigidbody2D)
				rigidbody2D.velocity = Vector2.zero;
			
			break;
		default:
			break;	
		}
	}
/*
	protected virtual void FixedUpdate(){
		float velocityY = rigidbody2D.velocity.y;
		
			
		switch (current_status) {
		case STATUS.IDLE:
			break;
		case STATUS.JUMP_UP:
		case STATUS.JUMP_DOWN:
		case STATUS.WALK:
			if(Mathf.Abs( rigidbody2D.velocity.x) < WALK_SPEED_MAX ){
				rigidbody2D.AddForce(new Vector2(move_speed.x * WALK_SPEED_BASE * Time.deltaTime , velocityY));
			}
//			transform.position += new Vector3(move_speed.x * WALK_SPEED_BASE * Time.deltaTime, 0.0f, 0.0f);
			break;
		case STATUS.ATTACK:
			
			if(!grounded){
				if(Mathf.Abs( rigidbody2D.velocity.x) < WALK_SPEED_MAX ){
					rigidbody2D.AddForce(new Vector2(move_speed.x * WALK_SPEED_BASE * Time.deltaTime , velocityY));
				}				//transform.position += new Vector3(move_speed.x * WALK_SPEED_BASE * Time.deltaTime, 0.0f, 0.0f);
			}
			break;
		case STATUS.DAMAGE:

			break;
			
		case STATUS.DYING:
			break;
		case STATUS.GHOST_IDLE:
			if(rigidbody2D.velocity != Vector2.zero){
				rigidbody2D.velocity = Vector2.zero;
			}
			//Ghost move
			rigidbody2D.velocity = new Vector2(move_speed.x * MOVE_SPEED_BASE * Time.deltaTime * 0.7f, move_speed.y * MOVE_SPEED_BASE * Time.deltaTime * 0.7f);
			
			//transform.position += new Vector3(move_speed.x * MOVE_SPEED_BASE * Time.deltaTime * 0.7f, move_speed.y * MOVE_SPEED_BASE * Time.deltaTime * 0.7f, 0.0f);
			break;
		case STATUS.GONE:
			
			if(rigidbody2D){
				rigidbody2D.velocity = Vector2.zero;
			}
			break;
		default:
			break;	
		}
	}
*/

	protected bool CheckIsJumpable(){
		if (current_status == STATUS.IDLE || current_status == STATUS.WALK) {
			return true;
		} else {
			return false;
		}
	}

	protected virtual void Attack(){
		if (/*grounded && */ current_status != STATUS.GHOST_IDLE && current_status != STATUS.DAMAGE && current_health >= 1 ) {
			current_status = STATUS.ATTACK;
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

	protected override void ApplySpiritDamage(float value){
		if (current_status == STATUS.DAMAGE || current_status == STATUS.DYING ) {
			return;
		}
		
		base.ApplySpiritDamage (value);
		if (current_spirit <= 0) {
			if(current_status == STATUS.GHOST_IDLE){
				current_status = STATUS.GHOST_DAMAGE;
			}else{
				current_status = STATUS.DAMAGE;
			}
			rigorState = DYING_DELAY;
		} else {
			if(current_status == STATUS.GHOST_IDLE){
				current_status = STATUS.GHOST_DAMAGE;
			}else{
				current_status = STATUS.DAMAGE;
			}
			rigorState = DAMAGE_DURATION;
		}
	}

	protected virtual IEnumerator Die(){
		current_status = STATUS.GONE;
		//renderer.material.color = new Color (1.0f, 1.0f, 1.0f, 1.0f);
		renderer.material.color = Color.white;
		
		yield return new  WaitForSeconds(DISAPPEARING_DELAY);
		Instantiate(effectPoint_smoke, transform.position, transform.rotation);
		
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
		
		base.Flip();
	}

	public virtual bool CheckIsLiving(){
		return living;
	}
	
	

	public STATUS GetStatus(){
		return current_status;
	}

	public void SetStatus(STATUS status){
		this.current_status = status;
	}
}
