using UnityEngine;
using System.Collections;

public class PauseUI : MonoBehaviour {

	//private bool displaying = false;
	
	private UISprite uiSprite;
	// Use this for initialization
	void Start () {
	uiSprite = GetComponent<UISprite>();
	
		
		//iTween.ColorTo(this.gameObject, iTween.Hash("color", new Color(1,1,1, 0.5f), "time", 0.5f));
		iTween.ValueTo(this.gameObject, iTween.Hash("from", 1, "to", 0.5f, "time", 0.5f, "onupdate", "UpdateOpacity"));
	}
	
	// Update is called once per frame
	void Update () {
		
		if(! GameManager.Pause() ){
			uiSprite.enabled = false;
			uiSprite.alpha = 1.0f;
			
			return;
		}else{
			uiSprite.enabled = true;
			
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
