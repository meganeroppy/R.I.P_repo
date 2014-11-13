using UnityEngine;
using System.Collections;

public class BG : MonoBehaviour {

	private GameObject m_camera;

	// Use this for initialization
	void Start () {
		m_camera = GameObject.Find ("Main Camera");
		Vector3 cameraPos = m_camera.transform.position;
		transform.position = new Vector3 (cameraPos.x, cameraPos.y, cameraPos.z + 50.0f);
		transform.parent = m_camera.transform;
	}
}
