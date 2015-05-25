using UnityEngine;
using System.Collections;

public class BulletShooter : Generator {
	private Vector3 m_direction;
	
	protected float SHOOT_INTERVAL = 0.7f;
	protected float theta = 0.0f;
	private GameObject m_target;
	protected float speed = 12.0f;
	
	protected override void Start(){
		base.Start ();
		m_isWorking = true;
		if(m_target == null){
			GameObject obj = GameObject.FindWithTag("Player");
			if(obj == null){
				return;
			}
			
			m_target = obj;
			
		}		
	}
	
	// Update is called once per frame
	protected override void Update () {
		generate_timer -= Time.deltaTime;
		if (generate_timer <= 0.0f) {
			if(m_isWorking){
				//theta = theta >= 360 ? 12.0f : theta + 12.0f; 
			
				Generate();
				generate_timer = SHOOT_INTERVAL;
			}
			if(GameManager.GameOver()){
				m_isWorking = false;
			}
		}
	}
	
	protected override void ApplyHealthDamage (int value)
	{
		base.ApplyHealthDamage (value);
		
		//Generator is undead
		current_health += value;
	}
	
	protected override void Generate(){
		print("Generate()");
		Vector2 offset;
		//offset.x = Random.Range (-offset_range, offset_range);
		//offset.y = Random.Range (-offset_range, offset_range);
		
		offset = Vector2.zero;
		
		m_direction = (m_target.transform.position - transform.position).normalized;

		float rad = Mathf.Atan2(m_direction.y, m_direction.x);		
		theta = rad * Mathf.Rad2Deg;
		
		float offsetTheta =  Random.Range(-15.0f, 15.0f);
				
		float vx = Mathf.Cos(Mathf.PI / 180 * (theta + offsetTheta )) * speed * Time.deltaTime;
		float vy = Mathf.Sin(Mathf.PI / 180 * (theta + offsetTheta )) * speed * Time.deltaTime;
		
		Vector3 pos = transform.position;
		GameObject obj= Instantiate (child, new Vector3(pos.x + offset.x, pos.y + offset.y, pos.z), transform.rotation) as GameObject;
		obj.SendMessage("SetDirectionAndExecute", new Vector2(vx, vy));
	} 
	
	protected override void OnTriggerEnter2D(Collider2D col){
		if(col.gameObject.tag == "Player"){
			Debug.Log("TARGET FIND!");
		}
	}
	
	protected void OnTriggerExit2D(Collider2D col){
		if(col.gameObject.tag == "Player"){
			Debug.Log("TARGET LOST!");
		}
	}
	
}
