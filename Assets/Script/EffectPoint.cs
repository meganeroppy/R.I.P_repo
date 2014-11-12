using UnityEngine;
using System.Collections;

public class EffectPoint : MonoBehaviour {

	public float LIFE_TIME = 0.0f;
	public Vector2 DENSITY = new Vector2 (10.0f, 10.0f);
	public float FREQUENCY = 1.0f;
	protected float timer = 0.0f;
	protected float OFFSET_Z = 0.0f;
	public GameObject effect;


	// Use this for initialization

	protected virtual void Start () {
	//	LIFE_TIME = 1.0f;
	//	DENSITY = new Vector2 (10.0f, 10.0f);
	}
	
	// Update is called once per frame
	protected virtual void Update () {

		if (Mathf.Floor( Time.frameCount * Time.deltaTime * 100) % FREQUENCY == 0.0f) {
			Vector3 pos = transform.position;
			Quaternion rot = transform.rotation;

			Vector3 offset = new Vector3(Random.Range(-DENSITY.x, DENSITY.x), Random.Range(-DENSITY.y, DENSITY.y), OFFSET_Z );

			GameObject obj = Instantiate (effect, pos + offset, rot) as GameObject;
			obj.transform.parent = transform;

		}
	}
}
