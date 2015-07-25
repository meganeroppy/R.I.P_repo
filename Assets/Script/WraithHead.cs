using UnityEngine;
using System.Collections;

public class WraithHead : SonicBoom {

protected override void Start ()
	{
		base.Start ();
		attack_power = 33.0f;
		m_base_speed = 16.0f;
		LIFE_TIME = 1.4f;
	}

	protected override void OnTriggerEnter2D(Collider2D col){
		
		if (col.gameObject.tag == "Player") {
				Crash(col.gameObject.GetComponent<StageObject>());
		}
	}
	
	// Update is called once per frame
	protected override void Update () {
		if (m_isExecuted) {
			if(m_delay > 0.0f){
				m_delay -= Time.deltaTime;
				return;
			}
			int dir = current_side == SIDE.RIGHT ? 1 : -1;
			transform.Translate (new Vector3 (m_base_speed * dir * Time.deltaTime, 0.0f, 0.0f));
			m_pasted_time_from_born += 1.0f * Time.deltaTime;
			
			//About Animaion
			if(m_timer > animInterval){
				m_timer = 0.0f;
				cur_frame = cur_frame == 2 ? 0 : cur_frame + 1;
				spriteRenderer.sprite = pic[cur_frame];
				
			}else{
				m_timer += Time.deltaTime;
			}
			
			if (m_pasted_time_from_born >= LIFE_TIME) {
				Destroy (this.gameObject);		
			}
		}
	}
	
	
	protected override void Crash (StageObject other)
	{
		other. ApplySpiritDamage(attack_power);
	}
	
	public void SetWait(float time){
		m_delay = time;
	}
}
