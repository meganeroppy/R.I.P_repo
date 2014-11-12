using UnityEngine;
using System.Collections;

public class Bullet : Flyer {
	protected override void ApplyHealthDamage(int value){
		base.ApplyHealthDamage(value);
		if (current_health <= 0) {
			Destroy (this.gameObject);
		} else {
			StartCoroutine(WaitAndExecute(0.7f, false));
		}
	}
	
	protected virtual IEnumerator WaitAndExecute(float delay, bool dying){
		yield return new WaitForSeconds (delay);
		renderer.material.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
		invincible = false;
	}
}
