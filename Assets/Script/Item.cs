using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour {

	private SoundManager sound;

	protected enum TYPE{
		GAIN_HEALTH,
		GAIN_SPIRIT,
		REVIVAL,
		DYING,
		CLEAR
	}
	protected TYPE item_type;

	// Use this for initialization
	protected virtual void Start () {
		sound = GameObject.Find ("GameManager").GetComponent<SoundManager> ();
	}
	
	// Update is called once per frame
	protected virtual void Update () {
	
	}

	protected virtual void Remove(){
		sound.PlaySE ("GetItem", 1.0f);
		Destroy (this.gameObject);
	}

	public virtual string GetItemType(){
		if(item_type == TYPE.REVIVAL){
			return "REVIVAL";
		}else if(item_type == TYPE.DYING){
			return "DYING";
		}else if(item_type == TYPE.GAIN_HEALTH){
			return "GAIN_HEALTH";
		}else if(item_type == TYPE.GAIN_SPIRIT){
			return "GAIN_SPIRIT";
		}else if(item_type == TYPE.CLEAR){
			return "CLEAR";
		} else{
			return "ERROR";
		}
	}
}
