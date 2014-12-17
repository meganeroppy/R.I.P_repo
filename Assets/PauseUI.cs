using UnityEngine;
using System.Collections;

public class PauseUI : MonoBehaviour {

	private bool displaying = false;
	
	private UISprite uiSprite;
	// Use this for initialization
	void Start () {
		uiSprite = GetComponent<UISprite>();
		uiSprite.enabled = false;
		//iTween.ValueTo(this.gameObject, iTween.Hash("from", 1, "to", 0.0f, "time", 1.5f, "onupdate", "UpdateOpacity"));
		uiSprite.alpha = 0.75f;
		
	}
	
	// Update is called once per frame
	void Update () {
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
	
	protected void Reset(){
		uiSprite.alpha = 1.0f;
	}
	
	private void UpdateOpacity(float val){
		uiSprite.alpha = val;
	
	}
}
