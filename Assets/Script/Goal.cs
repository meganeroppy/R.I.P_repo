using UnityEngine;
using System.Collections;

public class Goal : Monument {

	private GameManager gameManager;

	// Use this for initialization
	protected override void Start () {
		gameManager = GameObject.Find ("GameManager").GetComponent<GameManager> ();
		base.Start();
		m_offset.y -= 0.2f;
		transform.FindChild("Gate_R").transform.Translate(0,0,-7);
	}
	
	protected override void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.tag == "Player") {
			gameManager.GameClear(true);		
		}
	}

	protected override void OnCollisionEnter2D(Collision2D col){
		if (col.gameObject.tag == "Player") {
			gameManager.GameClear(true);		
		}
	}

}
