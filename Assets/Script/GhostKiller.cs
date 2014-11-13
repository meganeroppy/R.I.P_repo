using UnityEngine;
using System.Collections;

public class GhostKiller : DeadZone {

	protected void OnCollisionEnter2D (Collision2D col)
	{
		if (col.gameObject.tag == "Player" && !GameManager.Miss()) {
			if(col.gameObject.GetComponent<Player>().CheckIsLiving()){
				return;
			}
			col.gameObject.SendMessage("Miss");
		}	
	}
}
