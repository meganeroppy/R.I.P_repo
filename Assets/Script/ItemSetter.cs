using UnityEngine;
using System.Collections;

public class ItemSetter : Monument {
	
	//Act Type
	public enum ACT_TYPE{
		CONSTANT,
		SEQUENTIAL,
		RANDOM,
	};
	protected ACT_TYPE actType;
	protected int choice = 0;
	
	//Status
	protected bool isReadyToRespawn;
	protected bool isChildRemoved;
	protected float respawnDelay = 5.0f; 
	protected const float NOTICE = 2.0f;
	protected float respawnTimer;
	protected float m_timer = 0;

	//Property
	protected bool SetonAwake = true;

	//Game Object
	public GameObject[] item = new GameObject[3];
	public GameObject notifyingEffect;

	protected override void Start(){
		builtOnGround = false;
		base.Start();

		SpriteRenderer sRenderer = GetComponent<SpriteRenderer>();
		sRenderer.enabled = false;
		
		if(SetonAwake){
			CreateItem();
		}
				
		isReadyToRespawn = false;
		respawnTimer = respawnDelay;
	}

	protected override void Update(){
		base.Update();
		
		isChildRemoved = true;
		
		for(int i = 0 ; i < transform.childCount ; i++){
			if(transform.GetChild(i).name.Contains(notifyingEffect.name) ){
				continue;
			}else{
				isChildRemoved = false;
			}
		}

		if(isReadyToRespawn){
			CreateItem();
			isReadyToRespawn = false;
		}else{
			if(isChildRemoved){
				respawnTimer -= Time.deltaTime;
				if(respawnTimer <= 0.0f){
					isReadyToRespawn = true;
					respawnTimer = respawnDelay;
				}else if(respawnTimer <= NOTICE){
				
					if (m_timer > 0.1f) {
						m_timer = 0.0f;
						Vector3 pos = transform.position;
						Quaternion rot = transform.rotation;
						
						Vector3 offset = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), 0.0f );
						
						GameObject obj = Instantiate (notifyingEffect, pos + offset, rot) as GameObject;
						Vector3 scale = obj.transform.localScale;
						obj.transform.localScale = new Vector3(scale.x * 0.7f, scale.y * 0.7f, scale.z);
						obj.transform.parent = transform;
					}else{
						m_timer += Time.deltaTime;
					}
					
				}
			}
		}
		

		
	}
	
	protected virtual void CreateItem(){
		GameObject selectedItem;
		switch(actType){
		case ACT_TYPE.CONSTANT:
			selectedItem = item[choice];
			break;
		case ACT_TYPE.SEQUENTIAL:
			selectedItem = item[choice++];
			break;
		case ACT_TYPE.RANDOM:
			selectedItem = item[ Random.Range(0, item.Length) ];
			break;
		default:
			selectedItem = item[choice];
			break;
			
		}

		GameObject obj = Instantiate(selectedItem) as GameObject;
		obj.transform.transform.position = transform.position + new Vector3(0.0f, 0.0f, -1.0f);
		obj.transform.parent = transform;
		
	}

}

