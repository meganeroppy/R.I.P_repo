using UnityEngine;
using System.Collections;

public class CheckPoint : MonoBehaviour {

	private bool marked = false; 
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	private void OnTriggerEnter2D(Collider2D col){
		OnEnter2D(col.gameObject);
	}
	
	private void OnCollisionEnter2D(Collision2D col){
		OnEnter2D(col.gameObject);
	}
	
	private void OnEnter2D(GameObject col){
		if(marked){
			return;
		}
		
		if(col.tag.Equals("Player")){
			Debug.Log("CheckPoint!");
			marked = true;
			GameObject.Find("GameManager").SendMessage("ApplyRespawnPoint", this.transform.position);
		}
	}
		
	
	private void Reactivate(){
		marked = false;
	}
}
