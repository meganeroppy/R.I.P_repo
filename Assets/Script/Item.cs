using UnityEngine;
using System.Collections;

public class Item : StageObject {

	protected enum ITEM_TYPE{
		GAIN_HEALTH,
		GAIN_SPIRIT,
		REVIVAL,
		DYING,
		CLEAR
	}
	protected ITEM_TYPE item_type;

	// Use this for initialization
	protected override void Start () {
		sound = GameObject.Find ("GameManager").GetComponent<SoundManager> ();
	}
	
	// Update is called once per frame
	protected override void Update () {
	
	}

	protected virtual void Remove(){
		sound.PlaySE ("GetItem", 1.0f);
		Destroy (this.gameObject);
	}

	public virtual string GetItemType(){
		if(item_type == ITEM_TYPE.REVIVAL){
			return "REVIVAL";
		}else if(item_type == ITEM_TYPE.DYING){
			return "DYING";
		}else if(item_type == ITEM_TYPE.GAIN_HEALTH){
			return "GAIN_HEALTH";
		}else if(item_type == ITEM_TYPE.GAIN_SPIRIT){
			return "GAIN_SPIRIT";
		}else if(item_type == ITEM_TYPE.CLEAR){
			return "CLEAR";
		} else{
			return "ERROR";
		}
	}
}
