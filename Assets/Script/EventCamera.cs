using UnityEngine;
using System.Collections;

public class EventCamera : MonoBehaviour {

	private Vector3[] focusPos = new Vector3[13];
	private Vector3 previousCameraPos;
	private Vector3 currentCameraPos;
	private const float DEFAULT_ZPOS = -10.0f;
	private float moveingTime = 2.5f;

	// Use this for initialization
	void Start () {
	
		for(int i = 0 ; i < focusPos.Length ; i++){
			Vector3 pos = GameObject.Find("FocusPoint" + (i+1).ToString()).transform.position;
			focusPos[i] = new Vector3(pos.x, pos.y, DEFAULT_ZPOS);
		}
		
		transform.position = focusPos[0];
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	protected void MoveToStartPos(){
		transform.position = focusPos[0];
	} 
	
	public void AdvancePhase(int nextPhase){
		if(nextPhase >= focusPos.Length){
			return;
		}
		
		float time = nextPhase == 1 ? moveingTime * 3.0f : moveingTime;
		
		iTween.MoveTo(gameObject, focusPos[nextPhase], time);
	
		//transform.position = focusPos[phase];
	}
}
