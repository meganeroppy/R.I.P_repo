using UnityEngine;
using System.Collections;

public class TreasureUI : UI {
	
	
	public Transform panel;
	public GameObject iconPrefab;
	private GameObject[] icon = new GameObject[5];
	private UISprite[] uiSprite_icon = new UISprite[5];
	private bool[] treasureInfo;//0 :Not Yet / 1 : Obtained
	private GameManager gameManager;
	
	protected override void Start(){
		
		gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
		base.Start();
		uiSprite.enabled = false;
		
	}
	
	// Update is called once per frame
	protected override void Update () {
		if(!activated){
			return;
		}
		
		treasureInfo = gameManager.GetTreasureInfo();
		
		for(int i = 0 ; i < treasureInfo.Length == true ; i++){
			
			if(!treasureInfo[i]){
				uiSprite_icon[i].spriteName = "neco_icon2";
			}else{
				uiSprite_icon[i].spriteName = "neco_icon1";
			}	
		}
		
	}
	
	protected override void Activate(){
				
		treasureInfo = gameManager.GetTreasureInfo();
		float interval = 100.0f;
		for(int i = 0 ; i < treasureInfo.Length ; i++){
						
			icon[i] = Instantiate( iconPrefab ) as GameObject;
			icon[i].transform.parent = panel.Find("Offset_treasure");
			icon[i].transform.localPosition = transform.localPosition + new Vector3(-(interval * treasureInfo.Length) + ((i+1) * interval), 0.0f, 0.0f);
			icon[i].transform.localScale = transform.localScale;
			uiSprite_icon[i] = icon[i].GetComponent<UISprite>();
			//if(i >= max_health){
			//	icon[i].GetComponent<UISprite>().enabled = false;
			//}
		}
		
		activated = true;
	}
}
