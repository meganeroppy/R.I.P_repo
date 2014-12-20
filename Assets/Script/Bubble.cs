using UnityEngine;
using System.Collections;

public class Bubble : Bullet {
	

	protected float offsetEularZ = -90.0f;
	protected SpriteRenderer spriteRenderer;
	public Sprite[] bubble_pic;
	protected int current_frame = 0;
	protected bool dying = false;
	
	protected override void Start ()
	{
		base.Start ();
		spriteRenderer = GetComponent<SpriteRenderer>();
	}
	
	protected override void Update ()
	{
		base.Update ();
		
		if(Mathf.Floor( Time.frameCount % 10 ) == 0 && !dying){
		current_frame = current_frame == 0 ? 1 : 0;
		spriteRenderer.sprite = bubble_pic[current_frame];
		}
	}
	
	protected override void ApplyHealthDamage(int value){
		Die();
	}
	
	protected override void Die(){
		dying = true;
		rigidbody2D.velocity = Vector2.zero;
		spriteRenderer.sprite = bubble_pic[2];
		Destroy(this.gameObject, 0.25f);
	}

}
