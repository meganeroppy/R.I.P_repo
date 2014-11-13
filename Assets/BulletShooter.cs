using UnityEngine;
using System.Collections;

public class BulletShooter : Generator {

	public float SHOOT_INTERVAL = 0.1f;
	

	protected override void Start(){
		base.Start ();
		m_isWorking = false;
	}
	
	// Update is called once per frame
	protected override void Update () {
		generate_timer += 1.0f * Time.deltaTime;
		if (generate_timer >= SHOOT_INTERVAL) {
			if(m_isWorking){
				Generate();
				generate_timer = 0.0f;
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
		
		Vector2 offset;
		offset.x = Random.Range (-offset_range, offset_range);
		offset.y = Random.Range (-offset_range, offset_range);
		
		Vector3 pos = transform.position;
		Instantiate (child, new Vector3(pos.x + offset.x, pos.y + offset.y, pos.z), transform.rotation);
	} 
	
	private void OnTriggerEnter2D(Collider2D col){
		if(col.gameObject.tag == "Player"){
			Debug.Log("TARGET FIND!");
		}
	}
	
	private void OnTriggerExit2D(Collider2D col){
		if(col.gameObject.tag == "Player"){
			Debug.Log("TARGET LOST!");
		}
	}
	
}
