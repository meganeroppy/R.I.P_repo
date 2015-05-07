using UnityEngine;
using System.Collections;

public class DeadPeace : StageObject {
	
	private bool grounded = false;
	protected override void Start ()
	{
		base.Start ();
		m_alpha = 1.0f;
		
	}

	protected override void Update ()
	{
	
	if(!grounded){
			return;
	
	}
		if(m_alpha <= 0.0f){
			Destroy(this.gameObject);
		}
		m_alpha -= Time.deltaTime * 0.15f;
		SetAlpha(m_alpha);
	
	}
	
	protected override void OnCollisionEnter2D (Collision2D col)
	{
		if(!grounded){
			grounded = true;
				//m_collider.isTrigger = true;
			//rigidbody2D.gravityScale = 0;
		}	
	}
}
