using UnityEngine;
using System.Collections;

public class Garbage : Enemy {

	public GameObject bubble;
	protected float eulerZ_bubbleTexture = -90.0f;
	private const float THROW_INTERVAL = 0.6f;
	private int throwCount = 0;
	protected const float DAMAGED_RIGOR = 0.1f;

	protected override void Start ()
	{
		base.Start ();
		current_health = 2;
		attack_power = 19.0f;
		invincible = true;

	}

	protected override void Update (){
		if(current_status == STATUS.GONE){
			return;
		}
	
		base.Update();
		if(m_awaking){
			if(GameManager.CheckCurrentPlayerIsGhost()){
				Hide();
				return;
			}
		
			if(Mathf.Abs( transform.position.x - m_target.transform.position.x) >= AWAKE_RANGE){
				Hide();
			}
			
			if(transform.position.x < m_target.transform.position.x){
				Flip(SIDE.RIGHT);//Means Left
			}else{
				Flip(SIDE.LEFT);//Means Right
			}
			
			if(rigorTimer <= 0.0f){
					ThrowGarbage();
					throwCount++;
				rigorTimer = throwCount % 9 == 0 ? THROW_INTERVAL * 6.0f : throwCount % 3 == 0 ? THROW_INTERVAL * 3.0f : THROW_INTERVAL;
			}else{
				rigorTimer -= Time.deltaTime;
			}
		
		
		}else{//Waiting
			if(GameManager.CheckCurrentPlayerIsGhost()){
				return;
			}
			
			if(PlayerIsInRange() && grounded){
				m_awaking = true;
				rigorTimer = WARMINGUP;
				anim.SetTrigger("t_showup");
				
			}
		}
		//anim.SetBool("b_awaking", m_awaking ? true : false);
		
		//anim.SetBool("b_idle", current_status == STATUS.IDLE ? true : false);
		//anim.SetBool("b_damaged", current_status == STATUS.DAMAGE ? true : false);
	}
	
	private void Hide(){
		m_awaking = false;
		anim.SetTrigger("t_hide");
		throwCount = 0;
	}
	
	protected override void ApplyHealthDamage (int value)
	{
		if(!m_awaking){
			return;
		}
		base.ApplyHealthDamage (value);
		anim.SetTrigger("t_damage");
	}

	
	private void ThrowGarbage(){
		anim.SetTrigger("t_attack");
		
		Vector3 pos = transform.position;
		Vector3 myColsCenter = new Vector3( pos.x + (m_cols[0].center.x + m_cols[1].center.x)/2, pos.y + (m_cols[0].center.y + m_cols[1].center.y)/2, transform.position.z);
		Vector3 targetPos = m_target.transform.position;
		Vector3 targetColsCenter = new Vector3( targetPos.x + (m_targetCols[0].center.x + m_targetCols[1].center.x)/2, targetPos.y + (m_targetCols[0].center.y + m_targetCols[1].center.y)/2, m_target.transform.position.z);
		
		float offsetY = 1.0f;
		
		GameObject obj =  Instantiate(bubble, new Vector3(myColsCenter.x, myColsCenter.y + offsetY, myColsCenter.z), transform.rotation) as GameObject;
		
		//Move Direction
		//Vector3 baseDir = (m_target.transform.position - obj.transform.position).normalized;
		Vector3 baseDir = (targetColsCenter - obj.transform.position).normalized;
		float radian = Mathf.Atan2(baseDir.y, baseDir.x);
		float baseAngle = radian * Mathf.Rad2Deg;
		float offsetAngle = Random.Range(-10.0f, 10.0f);
		float fixedAngle = baseAngle + offsetAngle;
		
		Vector3 fixedDir = new Vector3(Mathf.Cos(Mathf.PI / 180 * fixedAngle), Mathf.Sin(Mathf.PI / 180 * fixedAngle), 0.0f).normalized; 
		
		obj.SendMessage("SetAttackPower", attack_power);
		obj.SendMessage("SetDirectionAndExecute", fixedDir);
		//obj.rigidbody2D.AddForce(new Vector2 (baseDir.x * speed, baseDir.y * speed));
		
		//Rotation
		radian = Mathf.Atan2(fixedDir.x, fixedDir.y);
		float degree = radian * Mathf.Rad2Deg;
		obj.transform.rotation = Quaternion.Euler(0.0f, 0.0f, -degree + eulerZ_bubbleTexture);
	}
	
}
