using UnityEngine;
using System.Collections;

public class SpiritBar : MonoBehaviour {

	public GameObject spiritBarPrefab;
	private GameObject spiritBar;

	public GameObject spiritBarFramePrefab;
	private GameObject spiritBarFrame;

	private Vector2 m_basePos_spiritBar;
	private Vector2 m_scale_spiritBarFrame;

	Vector3 offset = new Vector3 (-14.0f, 5.5f, 1.0f);

	private GameObject m_camera;
	
	//Player 
	private Player player;
	STATUS status = STATUS.IDLE;

	// Use this for initialization
	void Start () {
		if(Application.loadedLevelName == "Title"){
			return;
		}
		player = GameObject.FindWithTag ("Player").GetComponent<Player> ();
		
		m_camera = GameObject.Find ("Main Camera");
		Vector3 cameraPos = m_camera.transform.position;

		spiritBarFrame = Instantiate(spiritBarFramePrefab, cameraPos + offset, transform.rotation) as GameObject;
		spiritBarFrame.transform.parent = m_camera.transform;              

		Vector3 pos = spiritBarFrame.transform.position;
		spiritBar = Instantiate(spiritBarPrefab, new Vector3(pos.x, pos.y, pos.z + 1.0f), spiritBarFrame.transform.rotation) as GameObject;
		
		spiritBar.transform.parent = spiritBarFrame.transform;

	}
	
	// Update is called once per frame
	void Update () {

		status = player.GetStatus();

		if(status == STATUS.GHOST){
			
		}
		float[] spirit = player.GetSpiritInfo ();
		float value_percent = spirit[1] / spirit[0];
		if(spiritBar != null){
			spiritBar.transform.localScale = new Vector3(value_percent, 1, 1);
		}
	}
}
