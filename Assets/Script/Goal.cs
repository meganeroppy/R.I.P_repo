using UnityEngine;
using System.Collections;

public class Goal : MonoBehaviour {

	private GameManager gameManager;

	// Use this for initialization
	void Start () {
		gameManager = GameObject.Find ("GameManager").GetComponent<GameManager> ();

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.tag == "Player") {
			gameManager.GameClear(true);		
		}
	}

	void OnCollisionEnter2D(Collision2D col){
		if (col.gameObject.tag == "Player") {
			gameManager.GameClear(true);		
		}
	}

}
