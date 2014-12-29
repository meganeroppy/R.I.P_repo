using UnityEngine;
using System.Collections;

public class Ev_Boss : EventCharacter {

	
	protected override void Start(){
		base.Start();
		renderer.material.color = new Color(1,1,1,0);
	}
	
	protected override void AdvancePhase(int phase){
		base.AdvancePhase(phase);
		//print(cur_phase);
	}
}
