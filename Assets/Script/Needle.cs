using UnityEngine;
using System.Collections;

public class Needle : Monument {

	private Player m_target;
	private float attack_power;

	private Vector2 blow_impact =  new Vector2(10.0f, 10.0f);
	private float m_timer = 0.0f;
	private const float DELAY = 0.2f;
	
	protected override void Start(){
		builtOnGround = false;
		base.Start();
		attack_power = 10.0f;
	}
	
	protected override void OnCollisionEnter2D(Collision2D col){
		if (col.gameObject.tag == "Player" || col.gameObject.tag == "Enemy") {
			Crash(col.gameObject);
		}
	}
	
	protected override void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.tag == "Player" || col.gameObject.tag == "Enemy") {
			Crash(col.gameObject);
		}
	}
	

	protected override void Update ()
	{
		base.Update ();
		if(m_timer > 0.0f){
			m_timer -= Time.deltaTime;
		}
	}

	protected void Crash(GameObject other){
	
		if(m_timer > 0.0f){
			return;
		}
	
		if (other.tag == "Player"){
			
			if (m_target == null){
				m_target = other.GetComponent<Player> ();
			}
			
			if ( m_target.GetStatus() != STATUS.GHOST_IDLE && m_target.GetStatus() != STATUS.GHOST_DAMAGE){
				
				float dir = 1.0f;
				if (gameObject.transform.position.x > m_target.transform.position.x){
					dir *= -1.0f;
				}
				
				m_target.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
				Vector2 force = new Vector2 (blow_impact.x * dir, blow_impact.y);
				m_target.SendMessage("ApplyForce", force);
				
				m_timer = DELAY;
				
				if(m_target.GetStatus() != STATUS.DYING){
					m_target.SendMessage ("HitNeedle", attack_power);
				}
			}
		}else if(other.tag.Equals("Enemy")){//For Enemies
			other.SendMessage ("InstantDeath");
		}
	}

}
