using UnityEngine;
using System.Collections;

public class Item : StageObject {

	protected EffectPoint_Flash effect;
	public GameObject effect_pop;

	protected enum ITEM_TYPE{
		GAIN_SPIRIT,
		REVIVAL,
		DYING,
		TREASURE
	}
	protected ITEM_TYPE item_type;

	// Use this for initialization
	protected override void Start () {
		base.Start();
		sound = GameObject.Find ("GameManager").GetComponent<SoundManager> ();
		effect = GetComponent<EffectPoint_Flash>();
	}
	
	// Update is called once per frame
	protected override void Update () {
	
	}

	public virtual void Remove(){
		Destroy (this.gameObject);
	}

	public virtual string GetItemType(){
		if(item_type == ITEM_TYPE.REVIVAL){
			return "REVIVAL";
		}else if(item_type == ITEM_TYPE.DYING){
			return "DYING";
		}else if(item_type == ITEM_TYPE.GAIN_SPIRIT){
			return "GAIN_SPIRIT";
		}else if(item_type == ITEM_TYPE.TREASURE){
			return "TREASURE";
		} else{
			return "It's ITEM_TYPE has not been defined!";
		}
	}
}
