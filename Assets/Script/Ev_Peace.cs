using UnityEngine;
using System.Collections;

public class Ev_Peace : Ev_Cat {
	protected override void Start () {
		base.Start();

	}	
	
	protected override void Update(){
		base.Update();
		anim.SetBool("b_ghost", ghostFlug);
	}
}
