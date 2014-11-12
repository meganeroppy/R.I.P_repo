using UnityEngine;
using System.Collections;

public class EffectPoint_Flash : EffectPoint {
	
	protected override void Update () {
		
		if (Time.frameCount % FREQUENCY == 0.0f) {
			Vector3 pos = transform.position;
			Quaternion rot = transform.rotation;
			
			Vector3 offset = new Vector3(Random.Range(-DENSITY.x, DENSITY.x), Random.Range(-DENSITY.y, DENSITY.y), 0.0f );
			
			GameObject obj = Instantiate (effect, pos + offset, rot) as GameObject;
			obj.transform.parent = transform;
		}
	}
}
