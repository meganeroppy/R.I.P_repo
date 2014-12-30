using UnityEngine;
using System.Collections;

public class Ev_Cat : EventCharacter {

	// Use this for initialization
	protected override void Start () {
		base.Start();
		excited = true;
		flipFlug = Random.Range(0.7f, 2.5f);
		
	}

	protected override void Update(){
		base.Update();
		
		if(excited){
			if(counter > flipFlug){
				counter = 0.0f;
				Flip();
				flipFlug = Random.Range(0.7f, 2.5f);
			}else{
				counter += Time.deltaTime;
			}
		}
	}
}
