using UnityEngine;
using System.Collections;

public abstract class AttackZone : StageObject {

	protected enum DIR
		{
		LEFT = -1,
		RIGHT = 1
	};

	//System
	protected float t_time;

	//Status
	public bool IS_INVISIBLE = false;
	protected const float DELAY = 0.15f;
	protected const float DURATION = 0.32f;
	protected bool hittable = false;

	protected float attack_power = 1.0f;

	protected override void Start () {
		base.Start ();
		t_time = 0.0f;
		if(IS_INVISIBLE){
			transform.renderer.enabled = false;
		}


	}

	protected override void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.tag != "Player" && col.gameObject.tag != "AttackZone") {
			//Debug.Log(col.gameObject.name);
			if(col.gameObject.tag == "Monument" || col.gameObject.tag == "Enemy"){
				Crash(col.gameObject);
			}
			if(col.gameObject.tag == "Item"){
				return;
			}
			Destroy(this.gameObject);
		}
	}

	protected virtual void Crash(GameObject other){
		other.gameObject.SendMessage("ApplyHealthDamage", attack_power);
	}

	protected override void Update () {
		t_time += 1.0f * Time.deltaTime;

		if(t_time >= DELAY && t_time < DELAY + DURATION){
			if(transform.renderer.enabled == true){
				this.renderer.material.color = new Color(0xFF,0x00,0x00);
			}
			if(!hittable){
				hittable = true;
			}
		}else if(t_time >= DELAY + DURATION){
			Destroy(this.gameObject);
		}
	}
}
