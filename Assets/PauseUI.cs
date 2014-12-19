using UnityEngine;
using System.Collections;

public class PauseUI : UI {

	private UILabel[] uiLabel = new UILabel[3];
	private const float opacity = 0.75f;
	
	// Use this for initialization
	protected override void Start () {
		//base.Start();
		uiSprite = transform.FindChild("Pause").GetComponent<UISprite>();
		uiLabel[0] = transform.FindChild("Resume").GetComponent<UILabel>();
		uiLabel[1] = transform.FindChild("Restart").GetComponent<UILabel>();
		uiLabel[2] = transform.FindChild("Quit").GetComponent<UILabel>();
		
		displaying = false;
		uiSprite.enabled = false;
		//iTween.ValueTo(this.gameObject, iTween.Hash("from", 1, "to", 0.0f, "time", 1.5f, "onupdate", "UpdateOpacity"));
		uiSprite.alpha =  opacity;
		for (int i = 0 ; i < 3 ; i++){
			uiLabel[i].enabled = false;
			uiLabel[i].alpha = opacity;
		}
	}
	
	// Update is called once per frame
	protected override void  Update () {
		if(! GameManager.Pause() ){
			if(displaying){
				displaying = false;
				uiSprite.enabled = false;
				for (int i = 0 ; i < 3 ; i++){
					uiLabel[i].enabled = false;
				}
				//uiSprite.alpha = 1.0f;
			}
		}else{
			if(!displaying){
				displaying = true;
				uiSprite.enabled = true;
				//
				StartCoroutine(UpdateWhilePause());
				//
				

				//iTween.ValueTo(this.gameObject, iTween.Hash("from", 1, "to", 0.0f, "time", 1.5f, "onupdate", "UpdateOpacity"));
			}
			//uiSprite.alpha -= 0.025f;
			//Debug.Log( uiSprite.alpha );
		}
	}
	
	private void Reset(){
		uiSprite.alpha = 1.0f;
	}
	
	private IEnumerator UpdateWhilePause(){
		
		while(true){
			if(displaying){

				for (int i = 0 ; i < 3 ; i++){
					uiLabel[i].enabled = true;
					if(i == GameManager.GetPauseStatus()){
						uiLabel[i].color = Color.yellow;
					}else{
						uiLabel[i].color = Color.white;
					}
				}
			}
			yield return 0;
		}
	
	}

}
