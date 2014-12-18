using UnityEngine;
using System.Collections;

public class UI : MonoBehaviour {

	protected bool displaying;
	protected UISprite uiSprite;
	
	// Use this for initialization
	protected virtual void Start () {
		uiSprite = GetComponent<UISprite>();
	}
	
	// Update is called once per frame
	protected virtual void  Update () {
	}
	
	protected virtual void UpdateOpacity(float val){
		uiSprite.alpha = val;
	}
}
