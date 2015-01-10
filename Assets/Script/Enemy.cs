using UnityEngine;
using System.Collections;

public class Enemy : Character {

	protected Player m_target; 
	protected CircleCollider2D[] m_targetCols;
	protected Vector2 blow_impact =  new Vector2(200.0f, 100.0f);
	protected bool m_awaking = false;
	
	protected override void Update ()
	{
		if (!GameManager.GameOver() && m_target == null){
			m_target = GameObject.FindWithTag ("Player").GetComponent<Player> ();
			m_targetCols = m_target.GetComponents<CircleCollider2D>(); 
		}
		
		base.Update ();
	}

	protected override void ApplyHealthDamage(int val){
		if(current_status == STATUS.GONE){
			return;
		}
		
		base.ApplyHealthDamage(val);
		if (current_health <= 0) {
			if(!m_visible){
				Destroy(this.gameObject);
			}else{
				Instantiate (effectPoint_destroy, transform.position, transform.rotation);
				rigidbody2D.Sleep();
				current_status = STATUS.GONE;
				StartCoroutine(WaitAndExecute(0.9f, true));
				m_collider.isTrigger = true;
			}
		} else {
			StartCoroutine(WaitAndExecute(0.7f, false));
		}
	}
	
	protected virtual void Crash(GameObject target){
		
		//Debug.Log("HIT" + Time.realtimeSinceStartup.ToString());
		
		if (m_target == null){
			m_target = target.GetComponent<Player> ();
		}
		
		if (m_target.GetStatus() != STATUS.DYING) {
			m_target.SendMessage ("ApplySpiritDamage", attack_power);
			
			float dir =  target.transform.position.x > transform.position.x ? 1.0f : -1.0f;
			
			if (m_target.GetStatus() != STATUS.GHOST_IDLE) {
				m_target.rigidbody2D.velocity = Vector2.zero;
				m_target.rigidbody2D.AddForce (new Vector2 (blow_impact.x * dir, blow_impact.y));
			}
		}
	}
	
	protected override void OnTriggerEnter2D(Collider2D col){
		if(current_status == STATUS.GONE){
		return;
		}
		if (col.gameObject.tag == "Player") {
			Crash(col.gameObject);
		}
	}
	
	protected override void OnCollisionEnter2D(Collision2D col){
		if(current_status == STATUS.GONE){
			return;
		}
		if (col.gameObject.tag == "Player") {
			Crash(col.gameObject);
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
}
