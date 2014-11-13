using UnityEngine;
using System.Collections;

public class Sakara : Flyer {
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

		protected override void Update(){
			base.Update();
			
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
		}
}
