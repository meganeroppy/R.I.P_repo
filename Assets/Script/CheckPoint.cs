using UnityEngine;
using System.Collections;

public class CheckPoint : MonoBehaviour {

	private bool marked = false; 
	public GameObject label_checkPoint;
	
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
			marked = true;
			Instantiate(label_checkPoint, transform.position + new Vector3(0.0f, 0.0f, -1.0f), transform.rotation);
			GameObject manager = GameObject.Find("GameManager") as GameObject;
			manager.GetComponent<GameManager>().SendMessage("ApplyRespawnPoint", this.transform.position);
			manager.GetComponent<SoundManager>().SendMessage("PlaySE", "GetItem");
		}
	}
		
	
	private void Reactivate(){
		marked = false;
	}
}
