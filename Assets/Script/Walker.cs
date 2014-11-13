using UnityEngine;
using System.Collections;

public class Walker : Character {
	
	protected override void ApplyHealthDamage(int value){
		base.ApplyHealthDamage (value);
		//anim.SetTrigger("t_damage");
	}
}
