using UnityEngine;
using System.Collections;

public class OpeningSet : UI {

	private const float DURATION = 0.5f;
	private float timer_screen = 0.0f; 
	
	protected UILabel uiLabel;
	private bool completeDisplayingLabel = false;
	// Use this for initialization
	protected void Awake () {
		uiSprite = GameObject.Find("BlackScreen_o").GetComponent<UISprite>();
		uiLabel = GameObject.Find("Title").GetComponent<UILabel>();
		activated = false;
	}
	
	protected override void Start () {

	}
	
	// Update is called once per frame
	protected override void Update () {
		if(!activated){
			return;
		}
		
		if( !completeDisplayingLabel ){
		
		if(uiLabel.alpha > 1.0f){
			completeDisplayingLabel = true;
		}else{
			uiLabel.alpha += Time.deltaTime * 0.5f;
			}
		}else{
			if(timer_screen >= DURATION){
				if(uiLabel.alpha < 0.0f && uiSprite.alpha < 0.0f){
					FinishDirection();

				}else{
					uiSprite.alpha -= Time.deltaTime * 0.5f;
					uiLabel.alpha -= Time.deltaTime * 0.5f;
				}
					
			}else{
				timer_screen += Time.deltaTime;
			}
		}
		
	}
	
	protected override void Activate(){
		uiSprite.enabled = true;
		uiLabel.enabled = true;
		uiSprite.alpha = 1.0f;
		uiLabel.alpha = 0.0f;
		uiLabel.text = Application.loadedLevelName;
		activated = true;
	}
	
	private void FinishDirection(){
		uiLabel.enabled = false;
		uiSprite.enabled = false;
		GameManager.CompleteOpneingDirection(true);
	}
}
