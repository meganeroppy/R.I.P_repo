﻿using UnityEngine;
using System.Collections;

public class RumpleGenerator : Generator {
	protected override void Start(){
		base.Start ();
		GENERATE_INTERVAL = 3.0f;
	}

	protected override void Generate(){
		
		Vector2 offset;
		//offset.x = Random.Range (-offset_range, offset_range);
		offset.x = 0;
		//offset.y = Random.Range (-offset_range, offset_range);
		offset.y = 0;
		
		Vector3 pos = transform.position;
		Rumple rumple = Instantiate (child, new Vector3 (pos.x + offset.x, pos.y + offset.y, pos.z), transform.rotation) as Rumple;
		rumple.transform.parent = transform;
		rumple.SetPettern("B");
	}

}
