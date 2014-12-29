using UnityEngine;
using System.Collections;

public class Ev_Peace : EventCharacter {
	protected override void Start () {
		base.Start();
		excited = true;
		flipFlug = Random.Range(60, 180);
		
	}	
	
	protected override void Update(){
		base.Update();
		
		if(excited){
			if(Time.frameCount % flipFlug == 0.0f){
				Flip();
				flipFlug = Random.Range(60, 180);
			}
		}
	}
}
