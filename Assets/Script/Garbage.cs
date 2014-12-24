using UnityEngine;
using System.Collections;

public class Garbage : Character {

	public GameObject bubble;
	protected float eulerZ_bubbleTexture = -90.0f;
	protected const float WARMINGUP = 1.0f;
	protected const float THROW_INTERVAL = 0.6f;
	protected int throwCount = 0;
	protected const float DAMAGED_RIGOR = 0.1f;
	protected float rigorTimer = 0.0f;
	private bool m_awaking = false;
	private const float AWAKE_RANGE = 10.24f;
	
	
	protected override void Awake(){
		base.Awake();
	}
	
	protected override void Start ()
	{
		base.Start ();
		current_health = 4;
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
//				if(Mathf.Floor( Time.frameCount ) % Random.Range(20, 25) == 0){
					ThrowGarbage();
					throwCount++;
				rigorTimer = throwCount % 9 == 0 ? THROW_INTERVAL * 6.0f : throwCount % 3 == 0 ? THROW_INTERVAL * 3.0f : THROW_INTERVAL;
//				}
			}else{
				rigorTimer -= Time.deltaTime;
			}
		
		
		}else{//Waiting
			if(GameManager.CheckCurrentPlayerIsGhost()){
				return;
			}
			
			if(Mathf.Abs( transform.position.x - m_target.transform.position.x) < AWAKE_RANGE){
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
		if (current_health <= 0) {
			Instantiate (effectPoint_destroy, transform.position, transform.rotation);
			current_status = STATUS.GONE;
			StartCoroutine(WaitAndExecute(0.9f, true));
			m_collider.enabled = false;
		} else {
			StartCoroutine(WaitAndExecute(0.7f, false));
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
	private void ThrowGarbage(){
		anim.SetTrigger("t_attack");
		Vector3 pos = transform.position;
		float offsetY = 2.0f;
		
		GameObject obj =  Instantiate(bubble, new Vector3(pos.x, pos.y + offsetY, pos.z), transform.rotation) as GameObject;
		
		//Move Direction
		Vector3 baseDir = (m_target.transform.position - obj.transform.position).normalized;
		float radian = Mathf.Atan2(baseDir.y, baseDir.x);
		float baseAngle = radian * Mathf.Rad2Deg;
		float offsetAngle = Random.Range(-15.0f, 15.0f);
		float fixedAngle = baseAngle + offsetAngle;
		
		Vector3 fixedDir = new Vector3(Mathf.Cos(Mathf.PI / 180 * fixedAngle), Mathf.Sin(Mathf.PI / 180 * fixedAngle), 0.0f).normalized; 
		
		obj.SendMessage("SetDirectionAndExecute", fixedDir);
		//obj.rigidbody2D.AddForce(new Vector2 (baseDir.x * speed, baseDir.y * speed));
		
		//Rotation
		radian = Mathf.Atan2(fixedDir.x, fixedDir.y);
		float degree = radian * Mathf.Rad2Deg;
		obj.transform.rotation = Quaternion.Euler(0.0f, 0.0f, -degree + eulerZ_bubbleTexture);
	}
	
}
