using UnityEngine;
using System.Collections;

public class DeadZone : StageObject {

	//private GameManager gameManager;

	// Use this for initialization
	protected override void Start () {
	//	gameManager = GameObject.Find ("GameManager").GetComponent<GameManager> ();
	}
	
	// Update is called once per frame
	protected override void Update () {
	}

	protected override void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.tag == "Player" && !GameManager.Miss()) {
			col.SendMessage("Miss");
		}
	}
}
