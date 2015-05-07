using UnityEngine;
using System.Collections;

public class Flyer : Enemy {
	
	protected float flying_move_speed = 2.5f;
	
	// Update is called once per frame
	protected override void Update () {
				switch (current_status) {
				case STATUS.IDLE:
						break;

				case STATUS.ATTACK:
						rigorState -= 1.0f * Time.deltaTime;
						if (rigorState <= 0) {
								current_status = STATUS.IDLE;
						}
						transform.position += new Vector3 (move_speed.x * WALK_SPEED_BASE * Time.deltaTime, 0.0f, 0.0f);
						break;
				case STATUS.DAMAGE:
						rigorState -= 1.0f * Time.deltaTime;
						if (rigorState <= 0.0f) {
								current_status = STATUS.IDLE;
						}
						break;	
				case STATUS.DYING:
						rigorState -= 1.0f * Time.deltaTime;
						if (rigorState <= 0.0f && grounded) {
								StartCoroutine (Die ());
								current_status = STATUS.GONE;
						}
						break;

				case STATUS.GONE:
						break;
				default:
						break;	
				}
		}
	protected override void OnEnter(GameObject target){
		
		if (m_target == null){
			m_target = target.GetComponent<Player> ();
		}
		
		STATUS status = (m_target.GetStatus());
		
		if(status == STATUS.GONE || status == STATUS.DYING){
			return;
		}
		
		m_target.SendMessage ("ApplySpiritDamage", attack_power);
		
		float dir =  target.transform.position.x > transform.position.x ? 1.0f : -1.0f;
		
		m_target.rigidbody2D.velocity = Vector2.zero;
		m_target.rigidbody2D.AddForce (new Vector2 (blow_impact.x * dir, blow_impact.y));
	}
	
	protected virtual void OnTriggerStay2D(Collider2D col){
		if(col.tag == "Player" && GameManager.CheckCurrentPlayerIsGhost()){
		
			if (m_target == null){
				m_target = col.GetComponent<Player> ();
			}
			
			STATUS status = (m_target.GetStatus());
			
			if(status == STATUS.GONE || status == STATUS.DYING){
				return;
			}
			
			m_target.SendMessage ("ApplySpiritDamage", attack_power);
			
			float dir =  col.transform.position.x > transform.position.x ? 1.0f : -1.0f;
			
			m_target.rigidbody2D.velocity = Vector2.zero;
			m_target.rigidbody2D.AddForce (new Vector2 (blow_impact.x * dir, blow_impact.y));
			
		}
	}
	
}
