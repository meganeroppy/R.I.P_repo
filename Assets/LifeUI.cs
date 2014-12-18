using UnityEngine;
using System.Collections;

public class LifeUI : UI {

	protected override void Update(){
	
		if(!activated){
			return;
		}
		string lifeStr = "stock_n" +  GameManager.player_life.ToString();
		uiSprite.spriteName = lifeStr;
	}
}
