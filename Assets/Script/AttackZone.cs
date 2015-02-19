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
	protected StageObject master = null;
	
	//Status
	public bool IS_INVISIBLE = false;
	protected const float DELAY = 0.1f;
	protected float DURATION = 0.4f;
	protected bool hittable = false;

	protected float attack_power = 1.0f;

	public GameObject effect_slush;
	
	protected override void Start () {
		base.Start ();
		t_time = 0.0f;
		if(IS_INVISIBLE){
			transform.renderer.enabled = false;
		}
	}
	
	protected override void Update () {
		t_time += Time.deltaTime;
		
		if(t_time >= DELAY && t_time <= DELAY + DURATION){
			if(transform.renderer.enabled == true){
				this.renderer.material.color = new Color(0xFF,0x00,0x00);
			}
			if(!hittable){
				hittable = true;
			}
		}else if(t_time > DELAY + DURATION){
			Destroy(this.gameObject);
		}
	}
   
	protected override void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.tag != "Player" && col.gameObject.tag != "AttackZone") {
			if(col.gameObject.tag == "Enemy"){
				
				if(col.gameObject.name.Contains("WraithGunner")){
					if(!col.gameObject.GetComponent<WraithGunner>().CheckIsLiving()){
						return;
					}
				}else if(col.gameObject.name.Contains("WraithChaser")){
					if(!col.gameObject.GetComponent<WraithChaser>().CheckIsLiving()){
						return;
					}
				}else if (col.gameObject.name.Contains("Wraith")){
					if(!col.gameObject.GetComponent<Wraith>().CheckIsLiving()){
						return;
						
					}				}
				
				Crash(col.gameObject);
			//	Destroy(this.gameObject);
			}else if(col.gameObject.tag == "Bullet"){
				col.gameObject.SendMessage("Die");
			}
			/*
			if(col.gameObject.tag != "Ground"){
				return;
			}else{
				Destroy(this.gameObject);
			}
			*/
		}
	}

	protected virtual void Crash(GameObject other){
		other.gameObject.SendMessage("ApplyHealthDamage", attack_power);
		Vector3 pos = transform.position;
		Instantiate(effect_slush, new Vector3(pos.x, pos.y, pos.z), transform.rotation);
		
	}


}
