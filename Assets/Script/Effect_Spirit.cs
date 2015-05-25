using UnityEngine;
using System.Collections;

public class Effect_Spirit : Effect {
	private float opacity;

	// Use this for initialization
	protected override void Start () {
		spriteRenderer = GetComponent<SpriteRenderer> ();
		opacity = 0.9f;
		transform.Rotate(0.0f, 0.0f, 180.0f);
		Flip(SIDE.LEFT);
	}
	
	// Update is called once per frame
	protected override void Update () {
	
		if(GameManager.Pause()){
			return;
		}
		
		if(transform.localScale.x < 0.0f){
		//	Flip();
		}
	
		opacity -= 0.8f * Time.deltaTime; 
		spriteRenderer.color = new Color(1.0f, 1.0f, 1.0f, opacity);
		if (opacity <= 0.0f) {
			Destroy(this.gameObject);		
		}
		transform.Rotate (0.0f, 0.0f, 4.0f);
		Vector3 pos = transform.position;
		transform.position = new Vector3(pos.x, pos.y + 1.0f * Time.deltaTime, pos.z);
	}

}
