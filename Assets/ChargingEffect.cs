using UnityEngine;
using System.Collections;

public class ChargingEffect : StageObject {

	public Sprite[] effect_pic;
	protected int current_frame = 0;
	private float counter = 0.0f;
	private float offsetY = 1.0f;
	private bool isChargeCompleted = false; 
	
	protected override void Start ()
	{
		base.Start ();
		SetAlpha(0);
	}

	
	// Update is called once per frame
	protected override void Update () {
		base.Update ();
		
		if(counter > 0.04f){
			counter = 0.0f;
			int charged = isChargeCompleted ? 4 : 0;
			current_frame = (current_frame + 1) % 4  == 0 ? 0 + charged : current_frame + 1;
			spriteRenderer.sprite = effect_pic[current_frame];
		}else{
			counter += Time.deltaTime;
		}
		
		if(transform.parent != null){
			Vector3 parentPos = transform.parent.transform.position;
			 Vector3 newPos = new Vector3(parentPos.x, parentPos.y + offsetY, transform.position.z);
			 transform.position = newPos;
		}
		
		if(InputManager.charging > 0.8f){
			isChargeCompleted = true;
		}else if(InputManager.charging > 0.3f && spriteRenderer.color.a == 0){
			SetAlpha(1);
		}else if(InputManager.charging <= 0.0f && spriteRenderer.color.a == 1){
			SetAlpha(0);
			isChargeCompleted = false;
			
		}
	}
}
