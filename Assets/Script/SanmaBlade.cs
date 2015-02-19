using UnityEngine;
using System.Collections;

public class SanmaBlade : AttackZone {

	private bool charged = false;

	//GameObjects
	public GameObject sonicBoom;

	protected override void Start ()
	{
		base.Start ();
		attack_power = 3.0f;
	}

	protected override void Update(){
		base.Update ();
		if (master != null) {
			Vector3 masterPos = master.transform.position;
			Vector3 offset = new Vector3(current_side == SIDE.RIGHT ? 1.3f : -1.3f, 1.5f, -1.0f);

			transform.position = masterPos + offset;
		}
	}

	private void ApplyParentAndExecute(StageObject master){
		this.master = master;
		
		//Flip(master.current_side);
		
		//Judge whether player faces to right side or left side
		if (transform.position.x >= master.transform.position.x) {
			Flip(SIDE.RIGHT);
		} else {
			Flip(SIDE.LEFT);
		}
		
		if(charged){
			//Create a sonicboom
			GameObject obj = Instantiate (sonicBoom, this.transform.position, this.transform.rotation) as GameObject;
			obj.SendMessage ("Execute", current_side);
			
			charged = false;
		}
	}
	
	protected void SetAsCharged(){
		charged = true;
	}
}
