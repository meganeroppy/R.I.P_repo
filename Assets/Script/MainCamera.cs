using UnityEngine;
using System.Collections;

public class MainCamera : MonoBehaviour {

	private static GameObject m_target;
	float posY_origin;
	float v_bottom;
	//Vector3 m_playerPos;
	private float LimitY;
	private bool SetLimit = false;
	private float offset = 5.0f;

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
		
		if(GameManager.playerIsBorn && !SetLimit){
			LimitY = GameObject.FindWithTag("DeadZone").transform.position.y;
			SetLimit = true;
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
		
		
		if(transform.transform.position.y - offset < LimitY && GameManager.Miss()){
			transform.position = new Vector3(playerPos.x, transform.position.y, playerPos.z - 5.0f);
		}else{
			transform.position = new Vector3(playerPos.x, playerPos.y + 3.0f, playerPos.z - 5.0f);
		}
		
	}
	
	public void  SetTarget(GameObject target){
		transform.position = new Vector3(20.0f, 20.0f);
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
