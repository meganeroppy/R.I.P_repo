using UnityEngine;
using System.Collections;

public class Flyer : Character {
	
	protected int attack_Power = 1;
	private Vector2 blow_impact =  new Vector2(200.0f, 100.0f);
	
	// Use this for initialization
	protected override void Start () {
		base.Start ();
		if (!GameManager.GameOver()){
			m_target = GameObject.FindWithTag ("Player").GetComponent<Player> ();
		}
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
						transform.position += new Vector3 (horizontal_move_speed * WALK_SPEED_BASE * Time.deltaTime, 0.0f, 0.0f);
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

	protected override void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.tag == "Player") {
			Crash(col.gameObject);
		}
	}

	protected override void ApplyHealthDamage(int value){
		base.ApplyHealthDamage(value);
		if (current_health <= 0) {
			Instantiate (effectPoint_destroy, transform.position, transform.rotation);
			current_status = STATUS.GONE;
		}
	}
	
	protected void Crash(GameObject target){
		
		//Debug.Log("HIT" + Time.realtimeSinceStartup.ToString());

		if (m_target == null){
			m_target = target.GetComponent<Player> ();
		}

		if (m_target.GetStatus() != STATUS.DYING) {
			m_target.SendMessage ("Hit", attack_Power);
			
			float dir =  target.transform.position.x > transform.position.x ? 1.0f : -1.0f;
			//float dir = m_target.current_side == SIDE.LEFT ? -1.0f : 1.0f;

			if (m_target.GetStatus() != STATUS.GHOST) {
			m_target.rigidbody2D.AddForce (new Vector2 (blow_impact.x * dir, blow_impact.y));
			}
		}
	}
}
