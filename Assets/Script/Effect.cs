using UnityEngine;
using System.Collections;

public class Effect : StageObject {

	protected const float MAX_LIFETIME = 3.0f;
	protected float timer = 0.0f;

	public bool TRACKING = false;
	
	[HideInInspector]
	public bool finished = false;//This variable is handled by the Animation

	protected Vector3 offset;
	protected Animator animator;
	
	// Use this for initialization
	
	protected override void Start () {
		animator = GetComponent<Animator>();
		if(TRACKING){
			float posZ = transform.position.z;
			offset = transform.position - transform.parent.transform.position;
			offset.z = posZ;
			transform.position = transform.position + offset;	
		}
	}
	
	// Update is called once per frame
	protected override void Update () {
		timer += Time.deltaTime;
		if(timer >= MAX_LIFETIME || finished){
			Destroy(this.gameObject);
		}
		if(TRACKING){
			transform.position = transform.position + offset;
		}
	}
}
