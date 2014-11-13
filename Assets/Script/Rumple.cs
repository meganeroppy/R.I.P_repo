using UnityEngine;
using System.Collections;

public class Rumple : Flyer {

	//private const float MAX_FLYING_SPEED = 1.5f;
	//private const float MIN_FLYING_SPEED = 0.3f;

	enum ACTION_PETTERN{
		A,
		B
	}
	private ACTION_PETTERN cur_action_pettern = ACTION_PETTERN.A;

	private float flying_move_speed = 0.3f;
	private float returning_speed = 5.0f;
	private bool m_isReturning = false;

	

	private float m_timer = 0.0f;
	private const float LIMIT_TIME = 12.0f;

	private float m_alpha = 1.0f;
	private SpriteRenderer spriteRenderer;

	private float m_randomNum = 0.0f;

	protected override void Start(){
		spriteRenderer = GetComponent<SpriteRenderer> ();
		base.Start ();
		current_health = 3;
		current_status = STATUS.IDLE;
		while (m_randomNum == 0.0f) {
			m_randomNum = Random.Range (-1.0f, 1.0f);
		}
	}

	protected override void Update(){

		if (GameManager.GameOver() || current_health <= 0) {
			return;		
		}
		switch (current_status) {
				
		case STATUS.IDLE:

			m_timer += 1.0f * Time.deltaTime;

			if (CheckPlayerIsGhost()) {
				current_status = STATUS.ATTACK;
				break;
			}

			if(cur_action_pettern == ACTION_PETTERN.B){
				float val = (Mathf.Cos((Mathf.PI * 2) * (m_timer * 0.5f)) ) * 0.15f;
				val *= m_randomNum;

				Vector3 pos = transform.position;
				transform.position = new Vector3(pos.x + val, pos.y + returning_speed * Time.deltaTime, pos.z);

			}

			//Look at Player
			if(!GameManager.GameOver()){
				if (m_target == null) {
					m_target = GameObject.Find("Player").GetComponent<Player>();	
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

			break;//End of STATUS.IDLE

		case STATUS.ATTACK:
			Vector2 dir = m_target.transform.position - transform.position;
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
				current_status = STATUS.ATTACK;
			}else{
				current_status = STATUS.IDLE;
			}
			break;
		}

	}

	protected override void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.tag == "Player") {
			//if(m_target.GetStatus() == STATUS.GHOST){
			//	m_target.SendMessage("Disappear");
			//}
			Crash(col.gameObject);
		}
	}

	protected override void ApplyHealthDamage(int value){
		base.ApplyHealthDamage(value);
		if (current_health <= 0) {
			StartCoroutine(WaitAndExecute(0.9f, true));
			m_collider.enabled = false;
		} else {
			StartCoroutine(WaitAndExecute(0.7f, false));
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

	private void GoToHome(){
		m_collider.enabled = false;
		m_isReturning = true;
	}

	protected override void Hit(int value){
		if (current_status != STATUS.DYING) {
			ApplyHealthDamage(value);		
		}
	}

	private bool CheckPlayerIsGhost(){
		if (m_target == null) {
			m_target = GameObject.Find("Player").GetComponent<Player>();	
		}

		if (m_target.GetStatus() == STATUS.GHOST) {
			//current_status = STATUS.ATTACK;
			return true;
		}else{
			//current_status = STATUS.IDLE;
			return false;
		}
	}

	public void SwitchPettern(){
		if (cur_action_pettern == ACTION_PETTERN.A) {
			cur_action_pettern = ACTION_PETTERN.B;
		} else {
			cur_action_pettern = ACTION_PETTERN.A;		
		}
	}
}
