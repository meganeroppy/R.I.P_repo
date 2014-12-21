using UnityEngine;
using System.Collections;

public class Cloud : MonoBehaviour {

	//Status
	protected float speed;
	protected float scale;
	
	protected float SPEED_MIN = 0.5f;
	protected float SPEED_MAX = 3.5f;
	protected float SCALE_MIN = 1.0f;
	protected float SCALE_MAX = 1.8f;
	
	
	protected SpriteRenderer spriteRenderer;
	
	protected float distance = 0.0f;
	
	public Sprite[] pic; 
	
	protected virtual void Start () {
		scale = Random.Range(SCALE_MIN, SCALE_MAX);
		speed = Random.Range (SPEED_MIN, SPEED_MAX);
		speed = scale > 2.0f ? speed * 0.75f : speed;
		this.transform.localScale = new Vector3(scale, scale, 1.0f);
		spriteRenderer = GetComponent<SpriteRenderer>();
		
		
		SetPic();
		
	}
	
	protected virtual void Update () {
		if(!GameManager.Pause()){
			this.transform.Translate(speed * Time.deltaTime, 0.0f, 0.0f);
		}
		
		distance = transform.position.x - transform.parent.transform.position.x;
		if(Mathf.Abs( distance ) >= 20.0f){
			//transform.position = new Vector3 (transform.parent.transform.position.x - (distance - 1.5f), Random.Range(3,6), transform.position.z);
			transform.position = new Vector3 (transform.parent.transform.position.x - (distance - 1.5f), transform.position.y, transform.position.z);
			SetPic();
		}

	}
	
	 protected virtual void SetPic(){
		int cloudType = Random.Range(0, pic.Length);
		spriteRenderer.sprite = pic[cloudType];
		
		scale = Random.Range(SCALE_MIN, SCALE_MAX);
		speed = Random.Range ( SPEED_MIN, SPEED_MAX);
		speed = scale > 1.4f ? speed * 0.75f : speed;
		
		this.transform.localScale = new Vector3(scale, scale, 1.0f);
	}
	
	
}
