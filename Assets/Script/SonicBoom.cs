using UnityEngine;
using System.Collections;

public class SonicBoom : AttackZone {

	protected bool m_isExecuted = false;

	protected float m_base_speed = 20.0f;

	protected float LIFE_TIME = 0.7f;
	protected float m_pasted_time_from_born;
	
	
	public Sprite[] pic = new Sprite[2];
	protected int cur_frame = 0; 
	public float animInterval = 1.0f;
	protected float m_timer;
	protected float m_delay = 0.0f;

	// Use this for initialization
	protected override void Start () {
	
		base.Start ();
		m_pasted_time_from_born = 0.0f;
		attack_power = 2.0f;
		cur_frame = 0;
		spriteRenderer.sprite = pic[cur_frame];
		m_timer = 0.0f;
		
	}

	protected virtual void Execute(SIDE dir){
		current_side = dir;
		Flip (dir);		
		//this.transform.localScale.x = dir == 1 ? -1 : 1; 
		m_isExecuted = true;
	}

	protected override void Crash(GameObject other){
		base.Crash(other);
		Vector3 pos = transform.position;
		
		Instantiate(effect_slush, new Vector3(pos.x, pos.y, pos.z), transform.rotation);
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
				cur_frame = cur_frame == 1 ? 0 : cur_frame + 1;
				spriteRenderer.sprite = pic[cur_frame];
				
			}else{
				m_timer += Time.deltaTime;
			}
			
			if (m_pasted_time_from_born >= LIFE_TIME) {
				Destroy (this.gameObject);		
			}
		}
	}

}