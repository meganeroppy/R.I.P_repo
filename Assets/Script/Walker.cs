using UnityEngine;
using System.Collections;

public class Walker : Character {
	
	protected override void ApplyHealthDamage(int value){
		base.ApplyHealthDamage (value);
		//anim.SetTrigger("t_damage");
	}
	
	protected override void ApplySpiritDamage(float value){
		base.ApplySpiritDamage (value);
	}
	
	protected override void Update (){
		base.Update();
		
		anim.SetBool("b_jump_down", current_status == STATUS.JUMP_DOWN ? true : false);
		anim.SetBool("b_jump_up", current_status == STATUS.JUMP_UP ? true : false);
		anim.SetBool("b_run", current_status == STATUS.WALK ? true : false);
		anim.SetBool("b_idle", current_status == STATUS.IDLE ? true : false);
		anim.SetBool("b_ghost", current_status == STATUS.GHOST_IDLE ? true : false);
		anim.SetBool("b_damaged", current_status == STATUS.DAMAGE ? true : false);
		anim.SetBool("b_dying", current_status == STATUS.DYING ? true : false);
		anim.SetBool("b_grounded", grounded);
		anim.SetBool("b_input", Input.GetAxis("Horizontal") != 0);
	}
}
