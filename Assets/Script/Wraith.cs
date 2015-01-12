using UnityEngine;
using System.Collections;

public class Wraith : Flyer {	


	protected float m_alpha = 0.0f;
	protected const float ALPHA_WAITING = 0.1f;
	private Vector3 m_homePos;
	private bool atHome = true;
	
	
	protected override void Start ()
	{
		base.Start ();
		
		attack_power = 37.0f;
		current_health = 3;
		m_awaking = false;
		flying_move_speed = 4.5f;
		
		AWAKE_RANGE = 15.0f;
		
		Color clr = spriteRenderer.color; 
		Color newColor = new Color (clr.r, clr.g, clr.b, ALPHA_WAITING);
		spriteRenderer.color = newColor;
		
		m_homePos = transform.position;

		}

	
	protected override void Update ()
	{
		base.Update ();

		anim.SetBool("b_awake", m_awaking || !atHome ? true : false);
						
		if(!GameManager.GameOver()){
			if (m_target == null) {
				m_target = GameObject.FindWithTag("Player").GetComponent<Player>();	
			}
			
			if(m_awaking){
			
				if(!GameManager.CheckCurrentPlayerIsGhost()){
					m_awaking = false;
				}
			
				if(current_status !=  STATUS.GONE && !GameManager.Miss()){
				
					if(PlayerIsInRange()){
						
						atHome = false;
						
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
					
						//Chasing Player
						Vector2 dir = (m_target.transform.position - transform.position).normalized ;
						dir = dir * flying_move_speed * Time.deltaTime;
						transform.Translate (new Vector3 (dir.x, dir.y, 0.0f));		
					}
				}
				
				if(m_alpha < 1.0f){
					m_alpha += Time.deltaTime;
					SetAlpha(m_alpha);
				}	
				
			}else{
				if( GameManager.CheckCurrentPlayerIsGhost()){
					m_awaking = true;
				}
				
				if(m_alpha > ALPHA_WAITING){
					m_alpha -= Time.deltaTime;
					SetAlpha(m_alpha);
				}
				
				//Back To Home Pos
				if(Mathf.Abs(m_homePos.x - transform.position.x) < 0.1f && Mathf.Abs(m_homePos.y - transform.position.y) < 0.1f){
					atHome = true;
				}else{
				
					Vector2 dir = (m_homePos - transform.position).normalized ;
					dir = dir * flying_move_speed * Time.deltaTime;
					transform.Translate (new Vector3 (dir.x, dir.y, 0.0f));
					
					if (transform.position.x > m_homePos.x) {
						if (transform.localScale.x < 0) {
							Flip (SIDE.LEFT);
						}
					} else {
						if(transform.localScale.x > 0){
							Flip(SIDE.RIGHT);
						}		
					}	
				}
			}
			
			if(m_alpha < ALPHA_WAITING){
				m_alpha += Time.deltaTime;
				SetAlpha(m_alpha);
			}
		}
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
	
	/*
	protected override void SetAlpha (float val)
	{
		if(spriteRenderer == null){
			spriteRenderer = GetComponent<SpriteRenderer>();
		}
		
		if(!m_awaking){
			if(spriteRenderer.color.a <= ALPHA_WAITING){
				base.SetAlpha(val);
			}
		}else{
			base.SetAlpha (val);
		}
	}
	*/
}
