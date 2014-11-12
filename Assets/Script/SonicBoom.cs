using UnityEngine;
using System.Collections;

public class SonicBoom : AttackZone {

	private bool m_isExecuted = false;

	public float m_base_speed;

	private float LIFE_TIME = 0.7f;
	private float m_pasted_time_from_born;

	public GameObject effect_slush;


	// Use this for initialization
	protected override void Start () {
		base.Start ();
		m_pasted_time_from_born = 0.0f;
	}

	private void Execute(SIDE dir){
		current_side = dir;
		Flip (dir);		
		//this.transform.localScale.x = dir == 1 ? -1 : 1; 
		m_isExecuted = true;
	}

	protected override void Crash(GameObject other){
		base.Crash(other);
		Vector3 pos = transform.position;
		Vector3 otherPos = other.transform.position;
		
		Instantiate(effect_slush, new Vector3(otherPos.x, otherPos.y, pos.z + 1.0f), transform.rotation);
	}

	// Update is called once per frame
	protected override void Update () {
		if (m_isExecuted) {
			int dir = current_side == SIDE.RIGHT ? 1 : -1;
			transform.Translate (new Vector3 (m_base_speed * dir * Time.deltaTime, 0.0f, 0.0f));
			m_pasted_time_from_born += 1.0f * Time.deltaTime;
			if (m_pasted_time_from_born >= LIFE_TIME) {
					Destroy (this.gameObject);		
				}
		}
	}

}