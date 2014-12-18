﻿using UnityEngine;
using System.Collections;

public class PauseUI : UI {

	
	// Use this for initialization
	protected override void Start () {
		base.Start();
		displaying = false;
		uiSprite.enabled = false;
		//iTween.ValueTo(this.gameObject, iTween.Hash("from", 1, "to", 0.0f, "time", 1.5f, "onupdate", "UpdateOpacity"));
		uiSprite.alpha = 0.75f;
		
	}
	
	// Update is called once per frame
	protected override void  Update () {
		if(! GameManager.Pause() ){
			if(displaying){
				displaying = false;
				uiSprite.enabled = false;
				//uiSprite.alpha = 1.0f;
			}
		}else{
			if(!displaying){
				displaying = true;
				uiSprite.enabled = true;
				//iTween.ValueTo(this.gameObject, iTween.Hash("from", 1, "to", 0.0f, "time", 1.5f, "onupdate", "UpdateOpacity"));
			}
			//uiSprite.alpha -= 0.025f;
			//Debug.Log( uiSprite.alpha );
		}
	}
	
	private void Reset(){
		uiSprite.alpha = 1.0f;
	}
	

}
