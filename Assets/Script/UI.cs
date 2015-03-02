using UnityEngine;
using System.Collections;

public class UI : MonoBehaviour {

	protected bool displaying = false;
	protected UISprite uiSprite;
	protected bool activated = false;
	
	// Use this for initialization
	protected virtual void Start () {
		uiSprite = GetComponent<UISprite>();
	}
	
	// Update is called once per frame
	protected virtual void  Update () {
		if(!activated){
			return;
		}
	}

	protected virtual void Activate(){
		activated = true;
	}
		
	protected virtual void UpdateOpacity(float val){
		uiSprite.alpha = val;
	}
}
