using UnityEngine;
using System.Collections;

public class Enemy : Character {

	protected override void ApplyHealthDamage(int val){
		if(current_status == STATUS.GONE){
			return;
		}
		
		base.ApplyHealthDamage(val);
		if (current_health <= 0) {
			Instantiate (effectPoint_destroy, transform.position, transform.rotation);
			current_status = STATUS.GONE;
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
}
