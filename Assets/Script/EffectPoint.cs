using UnityEngine;
using System.Collections;

public class EffectPoint : MonoBehaviour {

	public float LIFE_TIME = 0.0f;
	public Vector2 DENSITY = new Vector2 (10.0f, 10.0f);
	protected float nextPop = 0.0f;
	protected float OFFSET_Z = 0.0f;
	public GameObject effect;
	protected float m_timer = 0.0f;
	protected float m_lifeTimer = 0.0f;
	protected float repopDealy_min = 0.5f;
	protected float repopDealy_max = 1.2f;
	

	// Use this for initialization

	protected virtual void Start () {
	//	LIFE_TIME = 1.0f;
	//	DENSITY = new Vector2 (10.0f, 10.0f);
	}
	
	// Update is called once per frame
	protected virtual void Update () {
		
		if(GameManager.Pause()){
			return;
		}

		if (m_timer > nextPop) {
			m_timer = 0.0f;
			nextPop = Random.Range(repopDealy_min, repopDealy_min);
			Vector3 pos = transform.position;
			Quaternion rot = transform.rotation;

			Vector3 offset = new Vector3(Random.Range(-DENSITY.x, DENSITY.x), Random.Range(-DENSITY.y, DENSITY.y), OFFSET_Z );

			GameObject obj = Instantiate (effect, pos + offset, rot) as GameObject;
			obj.transform.parent = transform;
		}else{
			m_timer += Time.deltaTime;
		}
		
	}
}
