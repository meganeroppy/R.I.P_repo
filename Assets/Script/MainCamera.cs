using UnityEngine;
using System.Collections;

public class MainCamera : MonoBehaviour {

	private static GameObject m_target;
	float posY_origin;
	float v_bottom;
	//Vector3 m_playerPos;
	//private float[] ends = new float[4]{0.0f, 9999.0f, 9999.0f, 0.0f};
	//private float offset = 15.0f;

	// Use this for initialization
	void Start () {
		//posY_origin = transform.position.y;
		transform.position = new Vector3(20.0f, 20.0f);
	}
	
	// Update is called once per frame
	void Update () {
		if(GameManager.Miss()){
			return;
		}
		
		if(m_target == null){
			if(GameManager.GameOver()){
				return;
			}
			m_target = GameObject.FindWithTag ("Player");
		//	m_playerPos = m_target.transform.position;
			//v_bottom = posY_origin - 25.0f;
		}
		
		//Vector3 pos = transform.position;
		Vector3 playerPos;
		if (!GameManager.GameOver()) {
			playerPos = m_target.transform.position;
		}else{
			return;
		}
		/*
		print(ends[0].ToString() + ends[2].ToString());
		Vector3 pos = transform.position;
		if(playerPos.x - offset < ends[3] || playerPos.x + offset > ends[1]){
			transform.position = new Vector3(pos.x, playerPos.y + 3.0f, playerPos.z - 5.0f);
		}else if(playerPos.y < ends[0] || playerPos.y  > ends[2]){
			transform.position = new Vector3(playerPos.x, pos.y, playerPos.z - 5.0f);
		}else{
		*/
			transform.position = new Vector3(playerPos.x, playerPos.y + 3.0f, playerPos.z - 5.0f);
		//}
	}
	
	public static void  SetTarget(GameObject target){
		m_target = target;
	}
	
	public void ReleaseTarget(){
		if(m_target != null){
			m_target = null;
		}
	}
	
	private void SetEndsOfStage(float[] ends){
	//	this.ends = ends;
	}
}
