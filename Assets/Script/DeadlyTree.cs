using UnityEngine;
using System.Collections;



public class DeadlyTree : Monument {

	//GameObject
	public GameObject smog;

	// Use this for initialization
	protected override void Start () {
		base.Start ();
		offset = new Vector3 (2.0f, 2.0f);

		current_health = 5;
	
	}
	
	// Update is called once per frame
	protected override void Update () {
/*
		if (Time.frameCount % 20 == 0) {
			Vector3 pos = transform.position;
			float smogPosX = Random.Range(-offset, offset);
			float smogPosY = Random.Range(-offset, offset);
			GameObject tmp = Instantiate(smog, new Vector3(pos.x + smogPosX, pos.y + smogPosY, pos.z), transform.rotation) as GameObject;
			tmp.transform.parent = transform;
		}
*/
	}

}
