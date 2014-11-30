using UnityEngine;
using System.Collections;

public class Bullet : StageObject {
	
	private float speed = 15.0f;
	private Vector3 m_direction;
	
	protected override void Start ()
	{
//		base.Start();
	
		m_direction = Vector3.zero;
	}
	
	protected override void Update(){
		transform.Rotate(0, 0, 200.0f * Time.deltaTime);
		
		if(m_direction != Vector3.zero){
			Vector3 pos = transform.position;
			transform.position = new Vector3(pos.x + m_direction.x * speed * Time.deltaTime, pos.y + m_direction.y * speed * Time.deltaTime, pos.z);
		}
	}
	
	private void SetDirectionAndExecute(Vector3 dir){
		m_direction = dir;
	}
}
