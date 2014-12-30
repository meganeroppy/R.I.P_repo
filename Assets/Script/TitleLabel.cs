using UnityEngine;
using System.Collections;

public class TitleLabel : UI {

	protected UILabel[] uiLabel = new UILabel[5];
	string[] label = new string[5]{"PressAnyKeyToGetStarted", "Story", "Stage1", "Stage2", "Option"};
	//public Transform panel;
	//public Transform uiLabelPrefab;
	//private const int NUMOFSELECTION = 4;
	//private GameObject[] label = new GameObject[NUMOFSELECTION];
	//private UILabel[] uiLabel_selection = new UILabel[NUMOFSELECTION];

	protected override void Start ()
	{
		activated = true;
		
		for(int i = 0 ; i < uiLabel.Length ; i++){
			uiLabel[i] = GameObject.Find(label[i]).GetComponent<UILabel>();
			uiLabel[i].enabled = true;
		}
	}
	
	// Update is called once per frame
	protected override void Update () {
	
		if(!activated){
			return;
		}
		switch(GameManager.GetTitleStatus()){
			
			case "WAITFORKEY":
			for(int i = 0 ; i < uiLabel.Length ; i++){
				
				uiLabel[i].alpha = i == 0 ? 1.0f : 0.0f;
			}
			
				break;
			case "EVENT1":
			for(int i = 0 ; i < uiLabel.Length ; i++){
				uiLabel[i].alpha = i == 0 ? 0.0f :  i == 1 ? 1.0f : 0.5f;
			}
			break;
			case "TESTSTAGE1":
			for(int i = 0 ; i < uiLabel.Length ; i++){
				uiLabel[i].alpha = i == 0 ? 0.0f :  i == 2 ? 1.0f : 0.5f;
			}
			break;
			case "TESTSTAGE2":
			for(int i = 0 ; i < uiLabel.Length ; i++){
				uiLabel[i].alpha = i == 0 ? 0.0f :  i == 3 ? 1.0f : 0.5f;
			}
			break;
			case "OPTION":
			for(int i = 0 ; i < uiLabel.Length ; i++){
				uiLabel[i].alpha = i == 0 ? 0.0f :  i == 4 ? 1.0f : 0.5f;
			}
			break;
			default:
			for(int i = 0 ; i < uiLabel.Length ; i++){
				uiLabel[i].alpha = 0.0f;
			}
			break;
				
				
		}
	}
	
	protected override void Activate(){
	
		return;

	}
}
