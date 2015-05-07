using UnityEngine;
using System.Collections;

public class BossBullet : Bullet {

protected override void Update(){
		if(!executed){
			return;
		}
		
		base.Update();
		
		transform.Rotate(0,0,15);
		}
}
