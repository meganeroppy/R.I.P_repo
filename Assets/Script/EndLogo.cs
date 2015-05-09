using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EndLogo : UI {

	private UISprite uiLogo;
	private bool completeDisplaying = false;
	private bool pressed = false;
	
	protected UILabel uiLabel;
	// Use this for initialization
	protected void Awake () {
		uiSprite = GameObject.Find("BlackScreen_l").GetComponent<UISprite>();
		uiSprite.enabled = true;
		uiLogo = GameObject.Find("Logo").GetComponent<UISprite>();
		uiLogo.enabled = true;
		activated = false;
	}
	
	protected override void Start () {
	}
	
	// Update is called once per frame
	protected override void Update () {
		if(!activated){
		//	return;
		}

		if( !completeDisplaying ){
			
			if(uiSprite.alpha <= 0.0f){
				completeDisplaying = true;
			}else{
				uiSprite.alpha -= Time.deltaTime * 0.25f;
			}
		}else if (pressed){
			if(uiSprite.alpha >= 1.0f){
				SceneManager.LoadLevelAdditive("Title");
			}else{
				uiSprite.alpha += Time.deltaTime * 0.2f;
			}
		}else if (Input.anyKeyDown){
			pressed = true;
		}
	}
	
	public override void Activate(){
		uiSprite.enabled = true;
		uiLabel.enabled = true;
		activated = true;
	}
	
	protected  void Deactivate(){
		if(!activated){
			return;
		}
		uiSprite.enabled = false;
		uiLabel.enabled = false;
		activated = false;
	}

}
