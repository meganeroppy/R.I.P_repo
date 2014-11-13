using UnityEngine;
using System.Collections;

public class Needle : Monument {

	private Player m_target;
	private float attack_power;

	private Vector2 blow_impact =  new Vector2(200.0f, 100.0f);
	
	protected override void Start(){
		base.Start();
		attack_power = 10.0f;
	}

	protected override void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.tag == "Player") {
			Crash(col.gameObject);
		}
	}


	protected void Crash(GameObject other){
		
		//Debug.Log("HIT" + Time.realtimeSinceStartup.ToString());
		
		if (m_target == null){
			m_target = other.GetComponent<Player> ();
		}
		
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
