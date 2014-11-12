using UnityEngine;
using System.Collections;

public class Effect_aura : Effect {
	private float opacity;
	private SpriteRenderer spriteRenderer;

	protected override void Start ()
	{
		spriteRenderer = GetComponent<SpriteRenderer> ();
		opacity = 0.9f;
		transform.Rotate(0.0f, 0.0f, 180.0f);
		TRACKING = true;
		if(TRACKING){
			offset = transform.parent.transform.position - transform.position ;
			offset.z = 1.0f;
			transform.position = transform.parent.transform.position + offset;	
		}
	}

	protected override void Update(){
		opacity -= 0.8f * Time.deltaTime; 
		spriteRenderer.color = new Color(1.0f, 1.0f, 1.0f, opacity);
		if (opacity <= 0.0f) {
			Destroy(this.gameObject);		
		}
		transform.Rotate (0.0f, 0.0f, 4.0f);
		
		if(TRACKING){
			//offset = transform.parent.transform.position - transform.position ;
			transform.position = transform.parent.transform.position + offset;	
		}
	}
}
