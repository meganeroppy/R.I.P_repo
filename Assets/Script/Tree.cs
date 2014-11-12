using UnityEngine;
using System.Collections;

public class Tree : Monument {

	public GameObject item;
	// Use this for initialization
	protected override void Start () {
		base.Start ();
		current_health = 3;
	}
	/*
	void OnTriggerEnter2D(Collider2D col){
		Debug.Log ("OnTriggerEnter2D");
		if (col.gameObject.tag == "AttackZone") {
			if(!invincible){
				GetDamage(1);
			}

		}
	}
	*/

	protected override void ApplyHealthDamage(int value){
		base.ApplyHealthDamage (value);
	}
		/*
	protected override void Destroy(){
		//Sound;

		Destroy (this.gameObject);
	}
*/
	protected override IEnumerator WaitAndExecute(float delay, bool dying){
		yield return new WaitForSeconds (delay);
		renderer.material.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
		invincible = false;
		if (dying) {
			if (item != null) {
				Instantiate (item, transform.position, transform.rotation);
			}
			Destroy (this.gameObject);
		}
	}
	
}
