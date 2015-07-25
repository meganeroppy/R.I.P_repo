using UnityEngine;
using System.Collections;

public class GraveStone : Monument {
	public float gain_rate = 0.25f;

	public GameObject effect_good;
	private Player m_target;
	private float HEAL_RANGE = 2.0f;

	protected override void Start(){
		base.Start();
	}

	protected override void Update(){

		if(GameManager.GameOver() || GameManager.Miss()){
			return;
		}
		
		if(m_target == null){
			m_target = GameObject.FindWithTag ("Player").GetComponent<Player>();
		}
		Vector3 pos = transform.position;
		Vector3 targetPos = m_target.transform.position;		
		Vector3 distance = targetPos - pos;

		if( Mathf.Abs( distance.x) < HEAL_RANGE && Mathf.Abs( distance.y ) < HEAL_RANGE){
			if(Mathf.Floor( Time.frameCount * Time.deltaTime * 1000) % 1 == 0 ){					
				if(m_target != null){
					Heal(m_target);
				}
			}
		}
		
	}
	private void Heal(Player target){		
		target.GainSpirit(0.25f);
		//Vector3 tarGetpos = transform.position;
		//Instantiate(effect_good, pos + offset, transform.rotation );
	}
}
