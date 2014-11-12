using UnityEngine;
using System.Collections;

public class GhostKiller : DeadZone {

	protected override void OnTriggerEnter2D (Collider2D col)
	{
		if (col.gameObject.tag == "Player" && !GameManager.Miss()) {
			if(col.GetComponent<Player>().CheckIsLiving()){
				return;
			}
			col.SendMessage("Miss");
		}	}
}
