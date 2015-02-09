using UnityEngine;
using System.Collections;

public class Boss : Goop {
	
	protected override void ApplyHealthDamage (int value)
	{
		if(!m_awaking){
			return;
		}
		base.ApplyHealthDamage (value);
		if(current_health <= 0){
			GameObject.Find("GameManager").SendMessage("GameClear",true);
		}
	}
}
