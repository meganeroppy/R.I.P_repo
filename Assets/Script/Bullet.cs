using UnityEngine;
using System.Collections;

public class Bullet : StageObject {
	
	protected float speed = 15.0f;
	protected Vector3 m_direction = Vector3.zero;
	protected Vector2 blow_impact =  new Vector2(100.0f, 50.0f);
	protected float attack_power = 50.0f;
	protected float lifeTime = 2.0f;
	protected bool executed = false;
	protected bool dying = false;
	
	protected override void Start ()
	{
//		base.Start();
	}
	
	protected override void Update(){
	
		if(!executed){
			return;
		}
		
		if(!dying){
			Vector3 pos = transform.position;
			transform.position = new Vector3(pos.x + m_direction.x * speed * Time.deltaTime, pos.y + m_direction.y * speed * Time.deltaTime, pos.z);
		}
		lifeTime -= Time.deltaTime;
		if(lifeTime < 0.0f){
			Die ();
		}
	}
	
	protected virtual void SetDirectionAndExecute(Vector3 dir){
		m_direction = dir;
		executed = true;
	}
	
	protected virtual void SetDirectionAndExecute(Vector2 dir){
		SetDirectionAndExecute(new Vector3 (dir.x, dir.y, 0.0f));
	}
	
	protected override void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.tag == "Player") {
			Crash(col.gameObject);
		}else if(col.gameObject.tag == "Ground"){
			Die();
		}
	}
	
	protected virtual void OnCollisionEnter2D(Collision2D col){
		if(col.gameObject.tag == "Ground"){
			Die();
		}
	}
	
	protected virtual void Die(){
		Destroy(this.gameObject);
	}
	
	protected void Crash(GameObject other){
		
		//Debug.Log("HIT" + Time.realtimeSinceStartup.ToString());
		
		Player  m_target = other.GetComponent<Player> ();
		
		
		if (m_target.GetStatus() != STATUS.DYING && m_target.GetStatus() != STATUS.GHOST_IDLE) {
			m_target.SendMessage ("ApplySpiritDamage", attack_power);
			float dir = 1.0f;
			if (this.gameObject.transform.position.x > m_target.transform.position.x) {
				dir *= -1.0f;
			}
			m_target.rigidbody2D.AddForce (new Vector2 (blow_impact.x * dir, blow_impact.y));
		}
	}
}
