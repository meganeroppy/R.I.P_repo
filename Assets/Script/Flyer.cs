using UnityEngine;
using System.Collections;

public class Flyer : Enemy {
	
	// Use this for initialization
	protected override void Start () {
		base.Start ();

	}
	
	// Update is called once per frame
	protected override void Update () {
				switch (current_status) {
				case STATUS.IDLE:
						break;

				case STATUS.ATTACK:
						rigorState -= 1.0f * Time.deltaTime;
						if (rigorState <= 0) {
								current_status = STATUS.IDLE;
						}
						transform.position += new Vector3 (move_speed.x * WALK_SPEED_BASE * Time.deltaTime, 0.0f, 0.0f);
						break;
				case STATUS.DAMAGE:
						rigorState -= 1.0f * Time.deltaTime;
						if (rigorState <= 0.0f) {
								current_status = STATUS.IDLE;
						}
						break;	
				case STATUS.DYING:
						rigorState -= 1.0f * Time.deltaTime;
						if (rigorState <= 0.0f && grounded) {
								StartCoroutine (Die ());
								current_status = STATUS.GONE;
						}
						break;

				case STATUS.GONE:
						break;
				default:
						break;	
				}
		}

}
