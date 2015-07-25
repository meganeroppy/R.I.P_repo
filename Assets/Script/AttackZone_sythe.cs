using UnityEngine;
using System.Collections;

public class AttackZone_sythe : AttackZone {

	protected override void Start ()
	{
		base.Start ();
		DURATION = 0.1f;
		attack_power = 33.0f;
	}
	
	protected override void OnTriggerEnter2D(Collider2D col){
		
		if (col.gameObject.tag == "Player" && col.gameObject.tag != "AttackZone") {
			Crash(col.gameObject.GetComponent<StageObject>());
		}
	}
	
	public void ApplyParentAndExecute(StageObject master){
		//this.master = master;
		//transform.SetParent(master.transform);
		
		//Flip(master.current_side);
		
		//Judge whether player faces to right side or left side
		if (transform.position.x >= master.transform.position.x) {
			Flip(SIDE.RIGHT);
		} else {
			Flip(SIDE.LEFT);
		}
	}
	
	protected override void Crash(StageObject other){
		other.ApplySpiritDamage(attack_power);
		Vector3 pos = transform.position;
		Instantiate(effect_slush, new Vector3(pos.x, pos.y, pos.z), transform.rotation);
		
	}
	
}
