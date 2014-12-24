using UnityEngine;
using System.Collections;

public class Label_CheckPoint : MonoBehaviour {

	//Vector3 basePos;
	
	// Use this for initialization
	void Start () {
		//basePos = transform.position;
		
		//iTween.MoveTo(this.gameObject, iTween.Hash("y", basePos.y + 0.2f, "time", 0.4f, "looptype", iTween.LoopType.loop));

	}
	
	// Update is called once per frame
	void Update () {
	
		Vector3 pos = transform.position;
		Vector3 newPos = new Vector3( pos.x, pos.y  + ( Mathf.PingPong(Time.time * 2.0f, 0.2f) - 0.1f), pos.z);
		transform.position = newPos;
	
		
	}
}
