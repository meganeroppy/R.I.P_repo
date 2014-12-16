using UnityEngine;
using System.Collections;

public class Garbage : Character {

	public GameObject bubble;
	protected float offsetEularZ_bubble = -90.0f;
	protected const float WARMINGUP = 1.5f;
	protected const float THROW_INTERVAL = 0.1f;
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
	}

	protected override void Update (){
		if(current_status == STATUS.GONE){
			return;
		}
	
		base.Update();
		if(m_awaking){
			if(GameManager.CheckCurrentPlayerIsGhost()){
				m_awaking = false;
				anim.SetTrigger("t_hide");
				return;
			}
		
			if(Mathf.Abs( transform.position.x - m_target.transform.position.x) >= AWAKE_RANGE){
				m_awaking = false;
				anim.SetTrigger("t_hide");
			}
			
			if(transform.position.x < m_target.transform.position.x){
				Flip(SIDE.RIGHT);//Means Left
			}else{
				Flip(SIDE.LEFT);//Means Right
			}
			if(rigorTimer <= 0.0f){
				if(Mathf.Floor( Time.frameCount ) % Random.Range(30, 35) == 0){
					ThrowGarbage();
					rigorTimer = THROW_INTERVAL;
				}
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
		invincible = 0.0f;
		if (dying) {
			Destroy (this.gameObject);
		}
	}
	private void ThrowGarbage(){
		anim.SetTrigger("t_attack");
		Vector3 pos = transform.position;
		float offsetY = 2.0f;
		
		GameObject obj =  Instantiate(bubble, new Vector3(pos.x, pos.y + offsetY, pos.z), transform.rotation) as GameObject;
		float speed = 500.0f;
		Vector2 dir = (m_target.transform.position - obj.transform.position).normalized;
		obj.rigidbody2D.AddForce(new Vector2 (dir.x * speed, dir.y * speed));
		
		float radian = Mathf.Atan2(dir.x, dir.y);
		float degree = radian * Mathf.Rad2Deg;
		obj.transform.rotation = Quaternion.Euler(0.0f, 0.0f, -degree + offsetEularZ_bubble);
		
		
		//obj.GetComponent<Bubble>().SendMessage("SetDirectionAndExecute", dir);
	}
	
}
