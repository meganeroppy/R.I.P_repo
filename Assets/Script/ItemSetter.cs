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
	protected float respawnInterval = 5.0f; 
	protected const float NOTICE = 2.0f;
	protected float respawnTimer;
	protected float m_timer = 0;
	protected float m_childAlpha = 0.0f;

	//Property
	protected bool SetonAwake = true;
	protected bool oneShot = false;
	protected bool summoned = false;

	//Game Object
	public GameObject[] item = new GameObject[3];
	public GameObject notifyingEffect;
	protected GameObject child;

	protected override void Start(){
		builtOnGround = false;
		base.Start();

		SpriteRenderer sRenderer = GetComponent<SpriteRenderer>();
		sRenderer.enabled = false;
		
		
		if(SetonAwake && !summoned){
			CreateItem();
			isReadyToRespawn = false;
			respawnTimer = respawnInterval;
		}

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
			if(oneShot){
				child.SendMessage("SetAsOrphan", true);
				
				for(int i = 0 ; i < transform.childCount ; i++){
					GameObject obj = transform.GetChild(i).gameObject;
					if(obj.tag.Equals("Enemy")){
						obj.transform.parent = transform.parent.transform;
					}
				}
				transform.DetachChildren();
				Destroy(gameObject);
			}else{
				isReadyToRespawn = false;
			}
		}else{
			if(isChildRemoved){
				respawnTimer -= Time.deltaTime;
				if(respawnTimer <= 0.0f){
					isReadyToRespawn = true;
					respawnTimer = respawnInterval;
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
			}else{
				UpdateChildrenAlpha();
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

		child = Instantiate(selectedItem) as GameObject;
		child.transform.transform.position = transform.position + new Vector3(0.0f, 0.0f, -1.0f);
		child.transform.parent = transform;
		m_childAlpha = 0.0f;
		child.SendMessage("SetAlpha", m_childAlpha);
		
	}

	protected virtual void UpdateChildrenAlpha(){
		if(m_childAlpha < 1.0f && child != null){
			m_childAlpha += Time.deltaTime;
			child.SendMessage("SetAlpha", m_childAlpha);
		}
	}
	
	protected void SetAsOneShot(GameObject caller){
		transform.parent = caller.transform;
		oneShot = true;
		summoned = true;
		respawnTimer = NOTICE;
	}
}

