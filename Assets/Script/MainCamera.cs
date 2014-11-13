using UnityEngine;
using System.Collections;

public class MainCamera : MonoBehaviour {

	private static GameObject m_target;
	float posY_origin;
	float v_bottom;
	Vector3 m_playerPos;

	// Use this for initialization
	void Start () {
		//posY_origin = transform.position.y;
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
			m_playerPos = m_target.transform.position;
			transform.position = new Vector3(m_playerPos.x, m_playerPos.y + 3.0f, m_playerPos.z - 5.0f);
			//v_bottom = posY_origin - 25.0f;
		}
		
		//Vector3 pos = transform.position;
		Vector3 playerPos;
		if (!GameManager.GameOver()) {
			playerPos = m_target.transform.position;
		}else{
			return;
		}

		//if (pos.y < v_bottom) {
		//	transform.position = new Vector3 (playerPos.x, pos.y, playerPos.z - 5.0f);
		//} else {
			transform.position = new Vector3 (playerPos.x, playerPos.y + 3.0f, playerPos.z - 5.0f);
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
}
