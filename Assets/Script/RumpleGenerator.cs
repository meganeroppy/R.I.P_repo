using UnityEngine;
using System.Collections;

public class RumpleGenerator : Generator {
	protected override void Start(){
		//base.Start ();	//Maybe it had better not to use the base Start(), but it could work well again later. 
		if(transform.parent != null){
			offset = transform.position - transform.parent.transform.position;
		}
		m_isWorking = true;

		GENERATE_INTERVAL = 3.0f;
	}

	protected override void Generate(){
		
		Vector2 offset;
		offset.x = Random.Range (-offset_range, offset_range);
		offset.y = Random.Range (-offset_range, offset_range);
		
		Vector3 pos = transform.position;
		GameObject rumple = Instantiate (child, new Vector3 (pos.x + offset.x, pos.y + offset.y, pos.z), transform.rotation) as GameObject;
		rumple.transform.parent = transform.parent;
		rumple.SendMessage("SwitchPettern");
	}

}
