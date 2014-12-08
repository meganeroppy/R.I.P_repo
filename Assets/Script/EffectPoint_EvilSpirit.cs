using UnityEngine;
using System.Collections;

public class EffectPoint_EvilSpirit : EffectPoint {

	protected override void Start () {
		OFFSET_Z = 1.0f;
		FREQUENCY = 10.0f; 
		DENSITY = new Vector2 (1.0f, 1.0f);
	}
}
