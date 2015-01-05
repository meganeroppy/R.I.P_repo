using UnityEngine;
using System.Collections;

public class Bubble : Bullet {
	

	protected float offsetEularZ = -90.0f;
	protected SpriteRenderer spriteRenderer;
	public Sprite[] bubble_pic;
	protected int current_frame = 0;
	private float counter = 0.0f;
	
	protected override void Start ()
	{
		base.Start ();
		attack_power = 45.0f;
		speed = 7.0f;
		spriteRenderer = GetComponent<SpriteRenderer>();
	}
	
	protected override void Update ()
	{
		base.Update ();
		
		if(counter > 0.2f && !dying){
			counter = 0.0f;
			current_frame = current_frame == 0 ? 1 : 0;
			spriteRenderer.sprite = bubble_pic[current_frame];
		}else{
			counter += Time.deltaTime;
		}
	}
	
	protected override void ApplyHealthDamage(int value){
		Die();
	}
	
	protected override void Die(){
		dying = true;
		collider.enabled = false;
		rigidbody2D.velocity = Vector2.zero;
		spriteRenderer.sprite = bubble_pic[2];
		Destroy(this.gameObject, 0.25f);
	}

}
