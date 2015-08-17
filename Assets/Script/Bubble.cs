using UnityEngine;
using System.Collections;

public class Bubble : Bullet {
	

	protected float offsetEularZ = -90.0f;
	public Sprite[] bubble_pic;
	protected int current_frame = 0;
	private float counter = 0.0f;
	
	protected override void Start ()
	{
		base.Start ();
		speed = 7.0f;
		spriteRenderer = GetComponent<SpriteRenderer>();
	}
	
	protected override void Update ()
	{
		base.Update ();
		if(!dying){
			if(counter > 0.2f){
				counter = 0.0f;
				current_frame = current_frame == 0 ? 1 : 0;
				spriteRenderer.sprite = bubble_pic[current_frame];
			}else{
				counter += Time.deltaTime;
			}
		}else{// dying
			if(counter > 0.25f){
				//spriteRenderer.sprite = bubble_pic[2];
				Destroy(this.gameObject);
			}else{
				counter += Time.deltaTime;
			}	
		}
	}

	
	public override void ApplyHealthDamage(float value){
	/*	if(m_collider == null){
			m_collider = GetComponent<Collider2D>();
		}
	*/	m_collider.enabled = false;
		Die();
	}
	
	public override void Die(){
	Debug.Log("Bubble::Die()");
		dying = true;
		
		GetComponent<Rigidbody2D>().velocity = Vector2.zero;
		spriteRenderer.sprite = bubble_pic[2];
		counter = 0;
		//Destroy(this.gameObject, 0.25f);
	}

}
