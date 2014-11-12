using UnityEngine;
using System.Collections;

public class ItemSetter : Monument {
	
	//Act Type
	public enum ACT_TYPE{
		GOOD_ITEM,
		BAD_ITEM,
		RANDOM,
	};
	public ACT_TYPE actType;
	
	//Status
	private bool isReadyToRespawn;
	private bool isChildRemoved;
	private const float RESPAWN_DELAY = 5.0f; 
	private float respawnTimer;

	//Property
	public bool SetonAwake = true;

	//Game Object
	public GameObject goodItem;
	public GameObject badItem;	

	protected override void Start(){
		base.Start();

		SpriteRenderer sRenderer = GetComponent<SpriteRenderer>();
		sRenderer.enabled = false;
		
		if(SetonAwake){
			CreateItem();
		}
		isReadyToRespawn = false;
		respawnTimer = RESPAWN_DELAY;
	}

	protected override void Update(){
		base.Update();

		isChildRemoved = transform.childCount == 0;

		if(isReadyToRespawn){
			CreateItem();
			isReadyToRespawn = false;
		}else{
			if(isChildRemoved){
				respawnTimer -= Time.deltaTime;
				if(respawnTimer <= 0.0f){
					isReadyToRespawn = true;
					respawnTimer = RESPAWN_DELAY;
				}
			}
		}
	}
	
	protected virtual void CreateItem(){
		GameObject selectedItem;
		switch(actType){
		case ACT_TYPE.GOOD_ITEM:
			selectedItem = goodItem;
			break;
		case ACT_TYPE.BAD_ITEM:
			selectedItem = badItem;
			break;
		case ACT_TYPE.RANDOM:
			selectedItem = Random.Range(0, 1) > 0.5f ? goodItem : badItem;
			break;
		default:
			selectedItem = goodItem;
			break;
			
		}

		GameObject item = Instantiate(selectedItem, this.transform.position, this.transform.rotation) as GameObject;
		item.transform.parent = transform;			
		
	}

}

