using UnityEngine;
using System.Collections;

public class Bullet : StageObject {

	private Vector3 m_direction;
	private GameObject m_target;
	private float speed;

	protected override void Start ()
	{
		base.Start();
		speed = Random.Range(3.0f, 8.0f);
		m_direction = Vector3.zero;
		if(m_target == null){
			GameObject obj = GameObject.FindWithTag("Player");
			if(obj == null){
				return;
			}
			
			m_target = obj;
			m_direction = (m_target.transform.position - transform.position).normalized;
			
		}		
	}
	
	protected override void Update(){
	
		transform.Rotate(0, 0, 200.0f * Time.deltaTime);
		
		if(m_direction != Vector3.zero){
			Vector3 pos = transform.position;
			transform.position = new Vector3(pos.x + m_direction.x * speed * Time.deltaTime, pos.y + m_direction.y * speed * Time.deltaTime, pos.z);
		}
	}
	
	

	protected override void ApplyHealthDamage(int value){
	
		base.ApplyHealthDamage(value);
		if (current_health <= 0) {
			Destroy (this.gameObject);
		} else {
			StartCoroutine(WaitAndExecute(0.7f, false));
		}
	}
	
	protected virtual IEnumerator WaitAndExecute(float delay, bool dying){
		yield return new WaitForSeconds (delay);
		renderer.material.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
		invincible = false;
	}
}
