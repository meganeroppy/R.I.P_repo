using UnityEngine;
using System.Collections;

public class SoulfulLight : StreetLight {
	public float donate_rate = 36.0f;

	public GameObject effect_good;
	private Player m_target;
	private float HEAL_RANGE = 2.5f;
	private float counter_self = 0.0f; 
	private float counter_donate = 0.0f;
	protected Vector3 m_healRangeCenterOffset;
	

	protected override void Start(){
		base.Start();
		m_healRangeCenterOffset = new Vector3(current_side == SIDE.RIGHT ? 1 : -1, 1, 0);
	}

	protected override void Update(){

		if(GameManager.GameOver() || GameManager.Miss() || GameManager.Pause()){
			return;
		}
		
		base.Update();
		
		if(m_target == null){
			m_target = GameObject.FindWithTag ("Player").GetComponent<Player>();
		}
		
		Vector3 pos = transform.position + m_healRangeCenterOffset;
		Vector3 targetPos = m_target.transform.position;		
		Vector3 distance = targetPos - pos;

		if( Mathf.Abs( distance.x) < HEAL_RANGE && Mathf.Abs( distance.y ) < HEAL_RANGE && GameManager.GetPlayerIsGhost()){
			if(m_target != null){
				DonateSpirit(m_target);
			}
		}else{ 
			if(!GameManager.GetPlayerIsGhost() && m_awake){
				m_awake = false;
			}else if(GameManager.GetPlayerIsGhost()){
				if(!m_awake){
					m_awake = true;
					cur_frame = 1;
					spriteRenderer.sprite = pic[cur_frame];
				}
				
			}
		}
		
		if(m_awake){
		
			if(counter_self > 0.5f){			
				counter_self = 0.0f;
				Vector3 offset = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(0.0f, 2.0f), -1);
				GameObject obj = Instantiate(effect_good) as GameObject;
				obj.transform.position = transform.position + offset;
				obj.transform.localScale = new Vector3(0.5f,0.5f,0.5f);
				obj.transform.parent = transform;
			}else{
				counter_self += Time.deltaTime;
			}
			
		}
		
	}
	private void DonateSpirit(Player target){		
		target.GainSpirit(donate_rate * Time.deltaTime);
		
		//Make Effects
		if(counter_donate > 0.1f){
			counter_donate = 0.0f;
			Vector3 offset = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(0.0f, 2.0f), 1);
			GameObject obj = Instantiate(effect_good) as GameObject;
			obj.transform.position = target.transform.position + offset;
			obj.transform.localScale = new Vector3(0.5f,0.5f,0.5f);
			
			obj.transform.parent = target.transform;
		//Vector3 tarGetpos = transform.position;
		//Instantiate(effect_good, pos + offset, transform.rotation );
		}else{
			counter_donate += Time.deltaTime;
		}
	}
}
