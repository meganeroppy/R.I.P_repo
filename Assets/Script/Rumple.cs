using UnityEngine;
using System.Collections;

public class Rumple : Flyer {

	protected const float ALPHA_WAITING = 0.1f;

	enum ACTION_PETTERN{
		A,
		B
	}
	private ACTION_PETTERN cur_action_pettern = ACTION_PETTERN.A;

	private float returning_speed = 5.0f;
	//private bool m_isReturning = false;
	//private Vector3 m_basePos;
	

	private float m_timer = 0.0f;
	private const float LIMIT_TIME = 12.0f;

	public Sprite[] pic =  new Sprite[2];

	private float m_randomNum = 0.0f;
	
	private EffectPoint_EvilSpirit effect;

	protected override void Start(){
		spriteRenderer = GetComponent<SpriteRenderer> ();
		spriteRenderer.enabled = true;

		effect = GetComponent<EffectPoint_EvilSpirit>();
		effect.enabled = true;
		m_visible = true;
		
		base.Start ();
		current_health = 2;
		current_status = STATUS.IDLE;
		
		attack_power = 12.0f;
		
		while (Mathf.Abs( m_randomNum) <= 0.2f) {
			m_randomNum = Random.Range (-1.0f, 1.0f);
		}
		
		m_alpha = 0.0f;
		
		SetAlpha(m_alpha);
		
		
	//	m_basePos = transform.position;
	}

	protected override void Update(){
		
		
		
		if(m_awaking){
			if (GameManager.GameOver() || current_health <= 0) {
				m_awaking = false;
				effect.enabled = false;
			}
			
			if(GameManager.GetPlayerIsGhost()){
				m_awaking = false;		
				effect.enabled = false;
				
				}
			/*
			if(!GameManager.CheckCurrentPlayerIsGhost()){
				if(m_visible){
					if(cur_action_pettern == ACTION_PETTERN.B){
						m_visible = false;
						spriteRenderer.enabled = false;
						effect.enabled = false;
					}
				}
			}else{
				if(!m_visible){
					GameObject obj = Instantiate( effect_transformation ) as GameObject;
					obj.transform.position = transform.position + new Vector3(0,0,-1);
					obj.transform.localScale = new Vector3(1.5f, 1.5f, 1.0f);
				m_visible = true;
					spriteRenderer.enabled = true;
					effect.enabled = true;			
					
				}
			}
			*/
			
			switch (current_status) {
				
			case STATUS.IDLE:
				
				m_timer += Time.deltaTime;
			/*
			if (CheckPlayerIsGhost()) {
				current_status = STATUS.ATTACK;
				break;
			}
*/
				
				if(cur_action_pettern == ACTION_PETTERN.A){
					Vector3 pos = transform.position;
					Vector3 newPos = new Vector3( pos.x, pos.y  +  ( (Mathf.Sin (180 * (m_timer) * Mathf.Deg2Rad) * 0.01f) ), pos.z);
					transform.position = newPos;
				}else if(cur_action_pettern == ACTION_PETTERN.B){
					float val = (Mathf.Cos((Mathf.PI * 2) * (m_timer * 0.5f)) ) * 10.0f;
					val *= m_randomNum;
					
					Vector3 pos = transform.position;
					transform.position = new Vector3(pos.x + ( val * Time.deltaTime) , pos.y + returning_speed * Time.deltaTime, pos.z);
					
				}
				
			//Look at Player
				if(!GameManager.GameOver()){
					if (m_target == null) {
						m_target = GameObject.FindWithTag("Player").GetComponent<Player>();	
					}
					if (transform.position.x > m_target.transform.position.x) {
						if (transform.localScale.x < 0) {
							Flip (SIDE.LEFT);
					}
					} else {
						if(transform.localScale.x > 0){
							Flip(SIDE.RIGHT);
						}		
					}
				}
				
				//Flying away
				/*
			if (m_isReturning) {
				float val = (Mathf.Cos((Mathf.PI * 2) * (m_timer * 0.5f)) ) * 0.15f;
				val *= m_randomNum;
				Vector3 pos = transform.position;
				
				transform.position = new Vector3(pos.x + val, pos.y + returning_speed * Time.deltaTime, pos.z);
				//			transform.Translate(new Vector3(Mathf.Cos( (Mathf.Cos(Mathf.PI * 2 * m_timer))), returning_speed * Time.deltaTime, 0.0f));
				
				m_alpha -= 1.0f * Time.deltaTime;
				spriteRenderer.color = new Color(1.0f, 1.0f, 1.0f, m_alpha);
				if(m_alpha <= 0.0f ){
					Destroy(this.gameObject);
				}
			}else{
				if(m_timer > LIMIT_TIME){
					GoToHome();
				}
			}
			*/
				
				break;//End of STATUS.IDLE
				
			case STATUS.ATTACK:
				if(GameManager.Miss()){
					GoToHome();
				return;
				}
				
				Vector2 dir = (m_target.transform.position - transform.position).normalized ;
				dir = dir * flying_move_speed * Time.deltaTime;
			
				//Debug.Log(dir);
				
				/*
			while(Mathf.Abs(dir.x) < 0.3f || Mathf.Abs(dir.y) < 0.3f){
			}
			*/
				transform.Translate (new Vector3 (dir.x, dir.y, 0.0f));
				
				if(!CheckPlayerIsGhost()){
					current_status = STATUS.IDLE;
					GoToHome();
				}
				break;//End of STATUS.ATTACK
				
			default:
				if(CheckPlayerIsGhost()){
					//	current_status = STATUS.ATTACK;
				}else{
					current_status = STATUS.IDLE;
				}
				break;
			}
			
			if(m_alpha < 1.0f){
				m_alpha += Time.deltaTime;
				SetAlpha(m_alpha);
			}	
			
		}else{//Not Awaking
			
			if(!GameManager.GetPlayerIsGhost()){
				m_awaking = true;
				effect.enabled = true;
				
			}
			if(m_alpha > ALPHA_WAITING){
					m_alpha -= Time.deltaTime;
					SetAlpha(m_alpha);
				}
				
		}
	}

	protected override void OnCollisionEnter2D(Collision2D col){
		if(current_status == STATUS.GONE){
			return;
		}
	
		if (col.gameObject.tag == "Player") {
			if( cur_action_pettern == ACTION_PETTERN.B && col.gameObject.GetComponent<Player>().GetIsLiving()){
				return;
			}
			OnEnter (col.gameObject);
		}
	}

	private void GoToHome(){
		m_collider.enabled = false;
//		m_isReturning = true;
	}

	public override void ApplyHealthDamage (int value){
		if (current_status == STATUS.DYING) {
			return;
		}
		base.ApplyHealthDamage(value);
		if(current_health <= 0){
			spriteRenderer.sprite = pic[1];
			m_collider.enabled = false;
		}
	}

	private bool CheckPlayerIsGhost(){
		if (m_target == null) {
			m_target = GameObject.FindWithTag("Player").GetComponent<Player>();	
		}

		if (m_target.GetStatus() == STATUS.GHOST_IDLE) {
			//current_status = STATUS.ATTACK;
			return true;
		}else{
			//current_status = STATUS.IDLE;
			return false;
		}
	}

	protected void SwitchPettern(){
		if (cur_action_pettern == ACTION_PETTERN.A) {
			cur_action_pettern = ACTION_PETTERN.B;
		} else {
			cur_action_pettern = ACTION_PETTERN.A;		
		}
	}
	
	protected void SetPettern(string cmd){
		if (cmd == "A") {
			cur_action_pettern = ACTION_PETTERN.A;
		} else if(cmd == "B"){
			cur_action_pettern = ACTION_PETTERN.B;		
		}
	}
}
