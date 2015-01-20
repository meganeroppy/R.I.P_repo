using UnityEngine;
using System.Collections;

public class Wraith : Flyer {	


	protected float m_alpha = 1.0f;
	protected const float ALPHA_WAITING = 0.1f;
	protected Vector3 m_homePos;
	protected bool atHome = true;
	protected bool m_attacking = false;
	protected bool readyToAct = false;
	protected float m_timer = 0.0f;
	
	protected override void Start ()
	{
		base.Start ();
		
		attack_power = 25.0f;
		current_health = 3;
		m_awaking = false;
		flying_move_speed = 3.0f;
		
		AWAKE_RANGE = 20.48f;
		
		//Color clr = spriteRenderer.color; 
		//Color newColor = new Color (clr.r, clr.g, clr.b, ALPHA_WAITING);
		//spriteRenderer.color = newColor;
		
		m_homePos = transform.position;
		rigorTimer = WARMINGUP;
		
		m_timer = Random.Range(0, 360);
		}

	
	protected override void Update ()
	{
		base.Update ();

		anim.SetBool("b_awake", m_awaking || !atHome ? true : false);
		anim.SetBool("b_attack", m_attacking ? true : false);
		
		if(!GameManager.GameOver()){
			if (m_target == null) {
				m_target = GameObject.FindWithTag("Player").GetComponent<Player>();	
			}
			
			if(m_awaking){
			
				if(!GameManager.CheckCurrentPlayerIsGhost()){
					m_awaking = false;
					readyToAct = false;
					rigorTimer = WARMINGUP;
				}
			
				if(current_status !=  STATUS.GONE && !GameManager.Miss()){
				
					if(PlayerIsInRange()){
												
						//Look at Player
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
					if(!readyToAct){
						if(rigorTimer <= 0.0f ){
							 readyToAct = true;
						}else{
							rigorTimer -= Time.deltaTime;
						}
					}
					
				}
				/*
				if(m_alpha < 1.0f){
					m_alpha += Time.deltaTime;
					SetAlpha(m_alpha);
				}	
				*/
			}else{//Sleeping
			
				if(GameManager.CheckCurrentPlayerIsGhost()){
					if(PlayerIsInRange()){
						m_awaking = true;
						rigorTimer = WARMINGUP;
						Invoke("Hawl", 0.65f);
					}
				}
				/*
				if(m_alpha > ALPHA_WAITING){
					m_alpha -= Time.deltaTime;
					SetAlpha(m_alpha);
				}
				*/
				Vector3 pos = transform.position;
				Vector3 newPos = new Vector3( pos.x, pos.y  +  ( (Mathf.Sin (180 * (m_timer) * Mathf.Deg2Rad) * 0.01f) ), pos.z);
				transform.position = newPos;
				m_timer += Time.deltaTime;
				
			}
			/*
			if(m_alpha < ALPHA_WAITING){
				m_alpha += Time.deltaTime;
				SetAlpha(m_alpha);
			}
			*/
		}
	}
	
	protected void Hawl(){
		sound.PlaySE("Wraith_wakeup");		
	}


	protected override void OnEnter (GameObject target)
	{
		if(target.GetComponent<Player>().CheckIsLiving()){
			return;
		}else{
			base.OnEnter (target);
		}
	}
	
	public override bool CheckIsLiving(){
		return m_awaking;
	}
	
	protected override bool PlayerIsInRange(){
		if(Mathf.Abs( m_homePos.x - m_target.transform.position.x) < AWAKE_RANGE
		   && Mathf.Abs( m_homePos.y - m_target.transform.position.y) < AWAKE_RANGE
		   ){
			return true;
		}else{
			return false;
		}
	}
	
	protected virtual bool IAmInRange(){
		if(Mathf.Abs( m_homePos.x - transform.position.x) < AWAKE_RANGE
		   && Mathf.Abs( m_homePos.y - transform.position.y) < AWAKE_RANGE
		   ){
			return true;
		}else{
			return false;
		}
	}
}
