using UnityEngine;
using System.Collections;

public class CheckPoint : Monument {

	private bool marked = false; 
	public GameObject label_checkPoint;
	public Sprite[] pic = new Sprite[2];

	protected override void Start ()
	{
		builtOnGround = true;
		spriteRenderer = GetComponent<SpriteRenderer>();
		base.Start ();
	}
			
	
	protected override void OnTriggerEnter2D(Collider2D col){
		OnEnter2D(col.gameObject);
	}
	
	protected override void OnCollisionEnter2D(Collision2D col){
		OnEnter2D(col.gameObject);
	}
	
	private void OnEnter2D(GameObject col){
		if(marked){
			return;
		}
		
		if(col.tag.Equals("Player")){
			marked = true;
			spriteRenderer.sprite = pic[1];
			Vector2 col_center = GetComponent<CircleCollider2D>().center;
			GameObject obj = Instantiate(label_checkPoint, transform.position + new Vector3(col_center.x, col_center.y + 0.5f, -0.5f), transform.rotation) as GameObject;
			obj.transform.parent = this.transform;
			GameObject manager = GameObject.Find("GameManager") as GameObject;
			manager.GetComponent<GameManager>().SendMessage("ApplyRespawnPoint", this.transform.position);
			manager.GetComponent<SoundManager>().SendMessage("PlaySE", "GetItem");
		}
	}
		
	
	private void Reactivate(){
		marked = false;
	}
}
