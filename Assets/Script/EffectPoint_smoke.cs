using UnityEngine;
using System.Collections;

public class EffectPoint_smoke : EffectPoint {
	
	protected override void Start () {
		LIFE_TIME = 1.0f;
		DENSITY = new Vector2 (2.0f, 2.0f);
	}
	// Update is called once per frame
	protected override void Update () {
		base.Update();
		timer += 1.0f * Time.deltaTime;
		if(timer >= LIFE_TIME){
			Destroy(this.gameObject);
		}
	}
}
