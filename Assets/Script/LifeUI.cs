using UnityEngine;
using System.Collections;

public class LifeUI : UI {

	protected override void Update(){
	
		if(!activated){
			return;
		}
		string lifeStr = GameManager.player_life == 9999 ? "stock_infinity" : "stock_n" +  GameManager.player_life.ToString();
		uiSprite.spriteName = lifeStr;
	}
}
