using UnityEngine;
using System.Collections;

public class HealthUI : UI {

	private Player m_target;
	//public GameObject iconPrefab; 
	
	public Transform panel;
	public GameObject iconPrefab;
	private const int LIMIT_HEALTH = 10;
	private GameObject[] icon = new GameObject[LIMIT_HEALTH];
	private UISprite[] uiSprite_icon = new UISprite[LIMIT_HEALTH];
	private float[] healthInfo;//0 :MAX / 1 : Current
	
	protected override void Start(){
	
		base.Start();
		uiSprite.enabled = false;
		
	}
	
	// Update is called once per frame
	protected override void Update () {
		if(!activated){
			return;
		}
		
		healthInfo = m_target.GetLifeInfo();
		
		for(int i = 0 ; i < healthInfo[0] ; i++){
			
			if(i < healthInfo[1]){
				uiSprite_icon[i].spriteName = "hp_heart_max";
			}else{
				uiSprite_icon[i].spriteName = "hp_heart_min";
			}	
		}
		
	//	if(healthInfo[1] == 0){
	//		for(int i = 0 ; i < healthInfo[0] ; i++){
	//			Destroy(icon[i].gameObject);	
	//		}
	//		activated = false;
	//	}
	}
	
	public override void Activate(){
	
		m_target = GameObject.FindWithTag("Player").GetComponent<Player>();
		
		healthInfo = m_target.GetLifeInfo();
		//GameObject[] icon = new GameObject[LIMIT_HEALTH];
		float interval = 75.0f;
		for(int i = 0 ; i < LIMIT_HEALTH ; i++){
		
			if(i >= healthInfo[0]){
				break;
				//icon[i].GetComponent<UISprite>().enabled = false;
			}
			
			if(icon[i] != null){
				continue;
			}
			
			icon[i] = Instantiate( iconPrefab ) as GameObject;
			icon[i].transform.parent = panel.Find("Offset_health");
			icon[i].transform.localPosition = transform.localPosition + new Vector3(i * interval, 0.0f, 0.0f);
			icon[i].transform.localScale = transform.localScale;
			uiSprite_icon[i] = icon[i].GetComponent<UISprite>();
			//if(i >= max_health){
			//	icon[i].GetComponent<UISprite>().enabled = false;
			//}
		}
		
		activated = true;
	}
}
