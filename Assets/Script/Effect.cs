using UnityEngine;
using System.Collections;

public class Effect : MonoBehaviour {

	protected float LIFE_TIME;
	protected float timer = 0.0f;

	public bool TRACKING = false;

	protected Vector3 offset;
	
	// Use this for initialization
	
	protected virtual void Start () {
		LIFE_TIME = 0.5f;
		if(TRACKING){
			float posZ = transform.position.z;
			offset = transform.position - transform.parent.transform.position;
			offset.z = posZ;
			transform.position = transform.position + offset;	
		}
	}
	
	// Update is called once per frame
	protected virtual void Update () {
		timer += 1.0f * Time.deltaTime;
		if(timer >= LIFE_TIME){
			Destroy(this.gameObject);
		}
		if(TRACKING){
			transform.position = transform.position + offset;
		}
	}
}
