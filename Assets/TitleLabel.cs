using UnityEngine;
using System.Collections;

public class TitleLabel : UI {

	protected UILabel uiLabel;
	//public Transform panel;
	//public Transform uiLabelPrefab;
	//private const int NUMOFSELECTION = 4;
	//private GameObject[] label = new GameObject[NUMOFSELECTION];
	//private UILabel[] uiLabel_selection = new UILabel[NUMOFSELECTION];

	protected override void Start ()
	{
		uiLabel = GetComponent<UILabel>();
		activated = true;
		uiLabel.enabled = true;
	}
	
	// Update is called once per frame
	protected override void Update () {
	
		if(!activated){
			return;
		}
	
		if(gameObject.name.Equals("PressAnyKeyToGetStarted")){
			if(GameManager.GetTitleStatus().Equals("WAITFORKEY")){
				uiLabel.alpha = 1.0f;
			}else{
				uiLabel.alpha = 0.0f;
			}
		}else{
		
			uiLabel.alpha = 0.0f;
			
			switch(GameManager.GetTitleStatus()){
				
			case "MAIN":
				if(gameObject.name.Equals("Stage0")){
					uiLabel.alpha = 1.0f;
				}else{
					uiLabel.alpha = 0.5f;
				}
				break;
			
			case "TESTSTAGE1":
				if(gameObject.name.Equals("Stage1")){
					uiLabel.alpha = 1.0f;
				}else{
					uiLabel.alpha = 0.5f;
				}
				break;
			case "TESTSTAGE2":
				if(gameObject.name.Equals("Stage2")){
					uiLabel.alpha = 1.0f;
				}else{
					uiLabel.alpha = 0.5f;
				}
				break;
			case "OPTION":
				if(gameObject.name.Equals("Option")){
					uiLabel.alpha = 1.0f;
				}else{
					uiLabel.alpha = 0.5f;
				}
				break;
				/*
				for(int i = 0 ; i < NUMOFSELECTION ; i++){
					
					uiLabel_selection[i].text = "SELECT";
				}
	*/
				break;
			default:
			uiLabel.alpha = 0.0f;
				break;
				
			}
		}
	}
	
	protected override void Activate(){
	
		return;
		/*
		float interval = 75.0f;
		for(int i = 0 ; i < NUMOFSELECTION ; i++){
			label[i] = Instantiate(uiLabelPrefab) as GameObject;
			label[i].transform.parent = panel;
			label[i].transform.localPosition = transform.localPosition + new Vector3(i * interval, 0.0f, 0.0f);
			label[i].transform.localScale = transform.localScale;
			uiLabel_selection[i] = label[i].GetComponent<UILabel>();
		}
		
		activated = true;
	*/
	}
}
