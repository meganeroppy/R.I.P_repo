using UnityEngine;
using System.Collections;

public class Monument : StageObject {
	
	public GameObject effectPoint_destroy;
	protected Vector3 offset;

	protected override void Start(){
		base.Start();
		if(transform.parent != null){
			offset = transform.position - transform.parent.transform.position;
		}
	}

	protected override void ApplyHealthDamage(int value){
		base.ApplyHealthDamage(value);
		if (current_health <= 0) {
			Instantiate(effectPoint_destroy, transform.position, transform.rotation);
			StartCoroutine(WaitAndExecute(0.9f, true));
			m_collider.enabled = false;
		} else {
			StartCoroutine(WaitAndExecute(0.7f, false));
		}
	}

	protected override void Update(){

	base.Update();
		if(transform.parent != null){
			transform.position = transform.parent.transform.position + offset;
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
