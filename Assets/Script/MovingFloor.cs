using UnityEngine;
using System.Collections;

public class MovingFloor : StageObject {

	//Property
	private float root_pos_x;
	private float root_pos_y;
	private float max_pos_x;
	private float max_pos_y;
	private float min_pos_x;
	private float min_pos_y;

	//Status
	private float moveSpeed;
	
	public enum PETTERN{
		STATIC,
		VERTICAL,
		HORIZONTAL,
		VERTICAL_WAIT
	}

	public PETTERN current_pettern = PETTERN.STATIC;
	public bool activated = true;
	public float starting_position_x = 0.0f;
	public float starting_position_y = 0.0f;
	public bool onDebug = false;

	// Use this for initialization
	protected override void Start () {
		base.Start ();
		Vector3 pos = transform.position;
		root_pos_x = pos.x - starting_position_x;
		root_pos_y = pos.y - starting_position_y;

		max_pos_x = root_pos_x + 10.0f;
		max_pos_y = root_pos_y + 6.0f;
		min_pos_x = root_pos_x + -10.0f;
		min_pos_y = root_pos_y + -6.0f;

		moveSpeed = Random.Range(5.0f, 10.0f);
		switch (current_pettern) {
		case PETTERN.VERTICAL_WAIT:
			activated = false;
			//transform.position = new Vector3(root_pos_x, root_pos_y + starting_position_y, pos.z);
			break;
		default:
			break;
		}
	}
	
	// Update is called once per frame
	protected override void Update () {

		Vector3 pos = transform.position;

		switch (current_pettern) {
		case PETTERN.STATIC:
			break;
		case PETTERN.HORIZONTAL:
			if( moveSpeed >= 0){
				if(pos.x >= max_pos_x){
					moveSpeed *= -1.0f;
				}
			}else{
				if(pos.x <= min_pos_x){
					moveSpeed *= -1.0f;
				}
			}
			transform.position += new Vector3(moveSpeed * Time.deltaTime, 0.0f, 0.0f);

			break;
		case PETTERN.VERTICAL:
		case PETTERN.VERTICAL_WAIT:
			if(activated){
				if( moveSpeed >= 0){
					if(pos.y >= max_pos_y){
						moveSpeed *= -1.0f;
					}
				}else{
					if(pos.y <= min_pos_y){
						moveSpeed *= -1.0f;
					}
				}
				transform.Translate(0.0f, moveSpeed * Time.deltaTime, 0.0f);
			}
			break;
		default:
			break;
		
		}
	}

	protected void OnCollisionEnter2D(Collision2D col){
		if (col.gameObject.tag == "Player") {
			StartCoroutine(WaitAndExecute(1.0f));
		}
	}

	protected IEnumerator WaitAndExecute(float delay){

		yield return new WaitForSeconds (delay);
		activated = true;
	}

	public void SetSpeed(float speed){
		moveSpeed = speed;
	}

	public void SetPettern(PETTERN pettern){
		current_pettern = pettern;
	}
}
