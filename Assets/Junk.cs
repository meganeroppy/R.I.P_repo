using UnityEngine;
using System.Collections;

public class Junk : Enemy {

	private const float DEFALT_SWITCHINGTIME = 3.0f;
	private float switchingTime = 3.0f;
	private float m_timer = 0.0f;
	private float m_effectTimer = 0.0f;
	
	public GameObject bubble;
	protected float eulerZ_bubbleTexture = -90.0f;
	
	
	protected override void Start ()
	{
		base.Start ();
		layer_ground =  1 << 8;
		current_status = STATUS.IDLE;
		Flip(current_side);
		
		attack_power = 22.0f;
		
		AWAKE_RANGE = 15.36f;
		m_awaking = false;
		rigidbody2D.isKinematic = true;
		
	}

	protected override void Update(){
	
		anim.SetBool("b_idle", current_status == STATUS.IDLE ? true : false);
		base.Update();
		
		if(m_awaking){
		
		
			if(move_speed.x > 0.0f && current_side == SIDE.LEFT){
				Flip(SIDE.RIGHT);
			}else if(move_speed.x < 0.0f && current_side == SIDE.RIGHT){
				Flip(SIDE.LEFT);		
			}
		
		
			if(Mathf.Abs( transform.position.x - m_target.transform.position.x) >= AWAKE_RANGE){
				m_awaking = false;
				current_status = STATUS.IDLE;
				move_speed = Vector2.zero;
				m_timer = 0.0f;
			}
			
			if(m_timer >= switchingTime){
				m_timer = 0.0f;
				switchingTime = current_status == STATUS.WALK ? DEFALT_SWITCHINGTIME * Random.Range(0.7f, 1.3f): DEFALT_SWITCHINGTIME * Random.Range(1.5f, 2.1f);
				
				
				SwitchStatus();
			}else{
			
				if(current_status == STATUS.WALK && m_effectTimer > 0.4f){
					m_effectTimer = 0.0f;
					GameObject obj = Instantiate(effect_transformation, transform.position, transform.rotation) as GameObject;
					obj.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
					obj.renderer.material.color = new Color(1,1,1,0.5f);
					obj.transform.parent = transform.parent.transform;
				}else{
					m_effectTimer += Time.deltaTime;
				}
			
				m_timer += Time.deltaTime;
				
			}
			
			float speed = move_speed.x;
			
			RaycastHit2D curHit = Physics2D.Raycast(m_collider.transform.position + new Vector3(0, 3, 0), -Vector2.up, 100, layer_ground );
			RaycastHit2D nextHit = Physics2D.Raycast(m_collider.transform.position + new Vector3( speed > 0.0f ? 1.5f : -1.5f , 2,0), -Vector2.up, 100, layer_ground );
			
			
			if( Mathf.Abs (curHit.point.y - nextHit.point.y ) > 1.0f){
				move_speed = new Vector3(-speed, move_speed.y);
			//	Flip(current_side == SIDE.RIGHT ? SIDE.LEFT : SIDE.RIGHT);
			}
			
		}else{
			if(PlayerIsInRange()){
				m_awaking = true;
				rigidbody2D.isKinematic = false;
			}
		}
	}
	
	protected override void OnTriggerEnter2D(Collider2D col){
		OnEnter(col.gameObject);
	}
	
	protected override void OnCollisionEnter2D (Collision2D col)
	{
		OnEnter(col.gameObject);
	}
	
	protected override void OnEnter(GameObject col){
		if (col.tag == "Player" && current_status != STATUS.GONE) {
			if(!col.GetComponent<Player>().CheckIsLiving()){
				return;
			}
			base.OnEnter(col);
		}else if(col.tag == "Enemy" && current_status == STATUS.WALK){
			float speed = move_speed.x;
			move_speed = new Vector3(-speed, move_speed.y);
		}
	}
	
	protected void SwitchDir ()
	{
		float speed = move_speed.x;
		move_speed = new Vector3(-speed, move_speed.y);
		
	}
	
	protected override void ApplyHealthDamage(int val){
		base.ApplyHealthDamage(val);
	
		anim.SetTrigger("t_damage");
		if(current_health <= 0.0f){
			Explode();
		}
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
	
	private void Explode(){
		Vector3 pos = transform.position;
		//Vector3 targetPos = m_target.transform.position;
		//Vector3 targetColsCenter = new Vector3( targetPos.x + (m_targetCols[0].center.x + m_targetCols[1].center.x)/2, targetPos.y + (m_targetCols[0].center.y + m_targetCols[1].center.y)/2, m_target.transform.position.z);
		
		float offsetY = 1.0f;
		
		int numOfBullet = 8;
		float offsetAngle = Random.Range(-4.0f, 4.0f);
		
		for(int i = 0 ; i < numOfBullet ; i++){
		
			GameObject obj =  Instantiate(bubble, new Vector3(pos.x, pos.y + offsetY, pos.z), transform.rotation) as GameObject;
			
			//Move Direction
			//Vector3 baseDir = (targetColsCenter - obj.transform.position).normalized;
			//float radian = Mathf.Atan2(baseDir.y, baseDir.x);
			//float baseAngle = radian * Mathf.Rad2Deg;
			float baseAngle = 90;
			float fixedAngle = baseAngle + ( (360 / numOfBullet) * i ) + offsetAngle;
			
			Vector3 fixedDir = new Vector3(Mathf.Cos(Mathf.PI / 180 * fixedAngle), Mathf.Sin(Mathf.PI / 180 * fixedAngle), 0.0f).normalized; 
			obj.SendMessage("SetAttackPower", attack_power);
			
			obj.SendMessage("SetDirectionAndExecute", fixedDir);
			//obj.rigidbody2D.AddForce(new Vector2 (baseDir.x * speed, baseDir.y * speed));
			
			//Rotation
			float radian = Mathf.Atan2(fixedDir.x, fixedDir.y);
			float degree = radian * Mathf.Rad2Deg;
			obj.transform.rotation = Quaternion.Euler(0.0f, 0.0f, -degree + eulerZ_bubbleTexture);
		}
	}
}
