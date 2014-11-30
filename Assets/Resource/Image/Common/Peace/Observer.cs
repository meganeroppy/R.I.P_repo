using UnityEngine;
using System.Collections;

// This Script is for great examination

public class Observer : MonoBehaviour {

	private Vector3 pos;
	private Vector3 screenToWorldPointPosition;
	public float speed = 400.0f;
	public GameObject bullet;

	// Use this for initialization
	void Update () {
		//float h = Input.GetAxis ("Horizontal");
		//float v = Input.GetAxis ("Vertical");
		//Vector2 direction = new Vector2 (h, v).normalized;
		//rigidbody2D.velocity = direction * speed;
		
		//		this.transform.position = screenToWorldPointPosition;
		
		pos = Input.mousePosition;//get mouse position
		screenToWorldPointPosition = Camera.main.ScreenToWorldPoint(pos);
		screenToWorldPointPosition.z = 0f;
		
		//Debug.Log(screenToWorldPointPosition.ToString() + " : " + pos.ToString());
		//gameObject.transform.position = screenToWorldPointPosition;
		
		
//		Debug.Log(screenToWorldPointPosition.ToString() + " : " + transform.position.ToString());
		
						
		float angle =	Vector2.Angle (Vector2.up,screenToWorldPointPosition-transform.position);
		

		if(screenToWorldPointPosition.x >= transform.position.x){
			transform.eulerAngles = Vector3.forward * -angle;
		}else{
			transform.eulerAngles = Vector3.forward * angle;
		}
		
		Debug.Log(transform.eulerAngles);

		if(Input.GetKeyDown(KeyCode.Space)){
			GameObject obj = Instantiate(bullet, transform.position, transform.rotation) as GameObject;
			obj.SendMessage("SetDirectionAndExecute", transform.eulerAngles);
		}

		
	}
}
