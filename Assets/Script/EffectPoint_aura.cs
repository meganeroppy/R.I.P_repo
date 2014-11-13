using UnityEngine;
using System.Collections;

public class EffectPoint_aura : EffectPoint {

	protected override void Start () {
		OFFSET_Z = 1.0f;
		FREQUENCY = 30.0f; 
		DENSITY = new Vector2 (1.0f, 1.0f);
	}
}
