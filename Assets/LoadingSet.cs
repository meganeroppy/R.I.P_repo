using UnityEngine;
using System.Collections;

public class LoadingSet : UI {

	protected UILabel uiLabel;
	// Use this for initialization
	protected override void Start () {
		uiSprite = transform.FindChild("Offset_blackScreen").FindChild("BlackScreen").GetComponent<UISprite>();
		uiSprite.enabled = false;
		uiLabel = transform.FindChild("Offset_loading").FindChild("Loading").GetComponent<UILabel>();
		uiLabel.enabled = false;
	}
	
	// Update is called once per frame
	protected override void Update () {
		if(!activated){
			return;
		}
	
	}
	
	protected override void Activate(){
		uiSprite.enabled = true;
		uiLabel.enabled = true;
		StartCoroutine(AnimateLoadingLabel());
		activated = true;
	}
	
	private IEnumerator AnimateLoadingLabel(){
		int frame = 3;
		while(true){
			
			int pastTime = (int)(Time.time - Time.realtimeSinceStartup);
			if(pastTime % 750 == 0){
				
				string dot = "";
				for(int i = 0 ; i < frame ; i++){
					dot = dot + ".";
				}
				
				uiLabel.text = "Loading" + dot;
				
				frame = frame >= 3 ? 0 : frame + 1;
			}
			
			yield return 0;
		}
	}
}
