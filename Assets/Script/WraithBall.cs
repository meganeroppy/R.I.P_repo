using UnityEngine;
using System.Collections;

public class WraithBall : Bullet {

	protected override void Start ()
	{
		base.Start ();
		speed = 7.0f;
		spriteRenderer = GetComponent<SpriteRenderer>();
		lifeTime = 4.0f;
		transform.localScale = new Vector3(0.7f,0.7f,0.7f);
		m_alpha = 1;
	}
	
	protected override void Update ()
	{
		transform.Rotate(0, 0, 200.0f * Time.deltaTime);
		base.Update ();
		
		if(!GameManager.GetPlayerIsGhost() || GameManager.CheckCurrentPlayerIsHidden()){
			Die();
		}
		
		if(dying){
			if(m_alpha <= 0.0f){
				Destroy(this.gameObject);
			}else{
				m_alpha -= Time.deltaTime;
				SetAlpha(m_alpha);
			}
		}
	
	}
	
	protected override void OnTriggerEnter2D(Collider2D col){
		return;
		
		/*
		if(dying){
			return;
		}
		
		if (col.gameObject.tag == "Player") {
			Crash(col.gameObject);
		}else if(col.gameObject.tag == "Ground"){
			Die();
		}
		
		*/
	}
	
	protected override void OnCollisionEnter2D(Collision2D col){
		if(col.gameObject.tag.Equals("Player")){
			Crash(col.gameObject.GetComponent<Player>());
			Die();
		}
	}	
	
	public override void ApplyHealthDamage(float value){
		//	Die();
	}
	
	public override void Die(){
		if(dying){
			return;
		}
		
		dying = true;
		
		GetComponent<Rigidbody2D>().velocity = Vector2.zero;
		m_collider.enabled = false;
	}
	
	protected override void Crash(StageObject other){
		
		//Debug.Log("HIT" + Time.realtimeSinceStartup.ToString());
		
		Player  m_target = other.GetComponent<Player> ();
		
		if (m_target.GetStatus() != STATUS.DYING) {
			m_target.ApplyHealthDamage(attack_power);
			float dir = 1.0f;
			if (this.gameObject.transform.position.x > m_target.transform.position.x) {
				dir *= -1.0f;
			}
			m_target.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
			m_target.GetComponent<Rigidbody2D>().AddForce (new Vector2 (blow_impact.x * dir, blow_impact.y));
		}
	}
}

