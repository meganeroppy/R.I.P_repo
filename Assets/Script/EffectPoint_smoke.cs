using UnityEngine;
using System.Collections;

public class EffectPoint_smoke : EffectPoint {
	
	protected override void Start () {
		LIFE_TIME = 1.0f;
		repopDealy_min = 0.1f;
		repopDealy_max = 0.5f;
		DENSITY = new Vector2 (2.0f, 2.0f);
	}
	// Update is called once per frame
	protected override void Update () {
		base.Update();
		if(m_lifeTimer >= LIFE_TIME){
			while (transform.childCount > 0){
				transform.GetChild(0).transform.parent = null;
			}
		
			Destroy(this.gameObject);
		}else{
			m_lifeTimer += Time.deltaTime;
		}
	}
}
