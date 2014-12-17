using UnityEngine;
using System.Collections;

public class Label_CheckPoint : MonoBehaviour {

	//float m_alpha = 1.0f;
	Vector3 basePos;
	// Use this for initialization
	void Start () {
		basePos = transform.position;
		
		//iTween.ShakePosition(this.gameObject, new Vector3(0.0f, 0.5f, 0.0f), 0.2f);
		iTween.MoveTo(this.gameObject, iTween.Hash("y", basePos.y + 0.2f, "time", 0.4f, "looptype", iTween.LoopType.loop));
	}
	
	// Update is called once per frame
	void Update () {
	//	float val = Mathf.Cos(Time.frameCount) * 0.1f;
	//	transform.position = basePos + new Vector3(0.0f, val,  0.0f);
		
	}
}
