using UnityEngine;
using System.Collections;

public class Junk : Enemy {

	private const float DEFALT_SWITCHINGTIME = 3.0f;
	private float switchingTime = 3.0f;
	private float m_timer = 0.0f;
	
	protected override void Start ()
	{
		base.Start ();
		layer_ground =  1 << 8;
		current_status = STATUS.IDLE;
		Flip(current_side);
		
		attack_power = 45.0f;
	}

	protected override void Update(){
		base.Update();
		
		
		anim.SetBool("b_idle", current_status == STATUS.IDLE ? true : false);
		
		/*
		if(rigidbody2D.velocity.x > 0.0f){
			Flip(SIDE.LEFT);
		}else{
			Flip(SIDE.RIGHT);
		}
		*/
		
		//
		if(m_timer >= switchingTime){
			m_timer = 0.0f;
			switchingTime = current_status == STATUS.IDLE ? DEFALT_SWITCHINGTIME : DEFALT_SWITCHINGTIME * Random.Range(2.5f, 4.0f);
			SwitchStatus();
		}else{
			m_timer += Time.deltaTime;
		}
		
		float speed = move_speed.x;
		
		RaycastHit2D curHit = Physics2D.Raycast(m_collider.transform.position, -Vector2.up, 100, layer_ground );
		
		RaycastHit2D nextHit = Physics2D.Raycast(m_collider.transform.position + new Vector3( speed > 0.0f ? 1.0f : -1.0f , 0,0), -Vector2.up, 100, layer_ground );
		
		
		if( Mathf.Abs (curHit.transform.position.y - nextHit.transform.position.y ) < 5){
			return;
		}else{			          
			move_speed = new Vector3(-speed, move_speed.y);
			Flip(current_side == SIDE.RIGHT ? SIDE.LEFT : SIDE.RIGHT);
		}
	}
	
	protected override void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.tag == "Player") {
			if(col.gameObject.GetComponent<Player>().CheckIsLiving()){
				return;
			}
			Crash(col.gameObject);
		}
	}
	
	
	
	protected override void ApplyHealthDamage(int val){
		base.ApplyHealthDamage(val);
	
		anim.SetTrigger("t_damage");	
	}
	
	private void SwitchStatus(){
		if(current_status == STATUS.WALK){
			current_status = STATUS.IDLE;
			move_speed = new Vector2(0.0f, 0.0f);
		}else{
			current_status = STATUS.WALK;
			int dir = current_side == SIDE.RIGHT ? 1 : -1;
			move_speed = new Vector2(0.5f * dir, 0.0f);
		}
	}
}
