using UnityEngine;
using System.Collections;

public class NewBehaviourScript : MonoBehaviour {

	protected float offsetEularZ = -90.0f;
	private Vector3 dir;
	private float degree;
	private GameObject target;
	// Use this for initialization
	void Start () {
		transform.rotation = Quaternion.Euler(0.0f, 0.0f, 45.0f);
	}
	
	// Update is called once per frame
	void Update () {
		if(target == null){
			target = GameObject.FindWithTag("Player");
		}
		dir = (target.transform.position - transform.position).normalized;//make the unit vector 
		float radian = Mathf.Atan2(dir.x, dir.y);
		degree = radian * Mathf.Rad2Deg;//Radian To Eular Degree
		transform.rotation = Quaternion.Euler(0.0f, 0.0f, -degree + offsetEularZ);
		
	}
}
