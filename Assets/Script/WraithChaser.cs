using UnityEngine;
using System.Collections;

public class WraithChaser : Wraith {

	protected override void Update ()
	{
		base.Update ();
		
		if(m_awaking){
		
			if(PlayerIsInRange() && readyToAct && !m_attacking){
				atHome = false;
				m_attacking = true;
			}
			if(m_attacking){
			//Chasing Player
				Vector2 dir = (m_target.transform.position - transform.position).normalized ;
				dir = dir * flying_move_speed * Time.deltaTime;
				transform.Translate (new Vector3 (dir.x, dir.y, 0.0f));
				
				if(!IAmInRange()){
					m_attacking = false;
				}
			}else{
				//Back To Home Pos
				if(Mathf.Abs(m_homePos.x - transform.position.x) < 0.1f && Mathf.Abs(m_homePos.y - transform.position.y) < 0.1f){
					atHome = true;
				}else{
					
					Vector2 dir = (m_homePos - transform.position).normalized ;
					dir = dir * flying_move_speed * Time.deltaTime;
					transform.Translate (new Vector3 (dir.x, dir.y, 0.0f));
					
					if (transform.position.x > m_homePos.x) {
						if (transform.localScale.x < 0) {
							Flip (SIDE.LEFT);
						}
					} else {
						if(transform.localScale.x > 0){
							Flip(SIDE.RIGHT);
						}		
					}	
				}
			}
		}
	}
	
	

}
