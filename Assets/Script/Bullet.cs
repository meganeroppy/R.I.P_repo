using UnityEngine;
using System.Collections;

public class Bullet : StageObject {
	
	protected float speed = 15.0f;
	protected Vector3 m_direction;
	protected Vector2 blow_impact =  new Vector2(100.0f, 50.0f);
	protected float attack_power = 1.0f;
	protected float lifeTime = 2.0f;
	
	protected override void Start ()
	{
//		base.Start();
	
		m_direction = Vector3.zero;
	}
	
	protected override void Update(){		
		if(m_direction != Vector3.zero){
			Vector3 pos = transform.position;
			transform.position = new Vector3(pos.x + m_direction.x * speed * Time.deltaTime, pos.y + m_direction.y * speed * Time.deltaTime, pos.z);
		}
		lifeTime -= Time.deltaTime;
		if(lifeTime < 0.0f){
			Die ();
		}
	}
	
	protected void SetDirectionAndExecute(Vector3 dir){
		m_direction = dir;
	}
	
	protected void SetDirectionAndExecute(Vector2 dir){
		m_direction = new Vector3 (dir.x, dir.y, 0.0f);
	}
	
	protected override void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.tag == "Player") {
			Crash(col.gameObject);
		}else if(col.gameObject.tag == "Ground"){
			Die();
		}
	}
	
	protected virtual void Die(){
		Destroy(this.gameObject);
	}
	
	protected void Crash(GameObject other){
		
		//Debug.Log("HIT" + Time.realtimeSinceStartup.ToString());
		
		Player  m_target = other.GetComponent<Player> ();
		
		
		if (m_target.GetStatus() != STATUS.DYING && m_target.GetStatus() != STATUS.GHOST) {
			m_target.SendMessage ("Hit", attack_power);
			float dir = 1.0f;
			if (this.gameObject.transform.position.x > m_target.transform.position.x) {
				dir *= -1.0f;
			}
			m_target.rigidbody2D.AddForce (new Vector2 (blow_impact.x * dir, blow_impact.y));
		}
	}
}
