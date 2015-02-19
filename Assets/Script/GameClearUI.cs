using UnityEngine;
using System.Collections;

public class GameClearUI : UI {
	
	// Use this for initialization
	protected override void Start () {
		base.Start();
		displaying = false;
		uiSprite.enabled = false;
		//iTween.ValueTo(this.gameObject, iTween.Hash("from", 1, "to", 0.0f, "time", 1.5f, "onupdate", "UpdateOpacity"));
		//uiSprite.alpha = 0.75f;
		uiSprite.alpha = 0.0f;
	}
	
	// Update is called once per frame
	protected override void  Update () {
		if(! GameManager.GameClear() ){
			if(displaying){
				displaying = false;
				uiSprite.enabled = false;
				//uiSprite.alpha = 1.0f;
			}
		}else{//GameClear
			if(!displaying){
				displaying = true;
				uiSprite.enabled = true;
				iTween.ValueTo(this.gameObject, iTween.Hash("from", 0, "to", 1.0f, "time", 1.5f, "delay", 0.5f, "onupdate", "UpdateOpacity"));
			}
		}
	}
	
	
}
