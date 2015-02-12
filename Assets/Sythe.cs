using UnityEngine;
using System.Collections;

public class Sythe : SonicBoom {

protected override void Start ()
	{
		base.Start ();
		attack_power = 33.0f;
		m_base_speed = 8.0f;
		
	}

	protected override void OnTriggerEnter2D(Collider2D col){
		
		if (col.gameObject.tag == "Player") {
				Crash(col.gameObject);
		}
	}
	
	protected override void Crash (GameObject other)
	{
		other.gameObject.SendMessage("ApplySpiritDamage", attack_power);
	}
}
