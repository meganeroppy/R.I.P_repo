using UnityEngine;
using System.Collections;

public class Treasure : Item {

	private float m_timer = 0.0f;
	private int cur_frame = 0;
	public Sprite[] pic;
	public GameObject label_treasure;
	LayerMask layerMask;
	private int index = -1;
	
	protected override void Start ()
	{
		base.Start ();
		item_type = ITEM_TYPE.TREASURE;
		spriteRenderer.sprite = pic[cur_frame];
		
		// on ground
		layerMask = 1 << 8;
		RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up, 100.0f, layerMask);
		
		if(hit.point.y - transform.position.y >= 100.0f){
			return;
		}else{
			Vector3 newPos = new Vector3(hit.point.x, hit.point.y, transform.position.z);
			transform.position = newPos;
		}
	}
	
	protected override void Update ()
	{
		base.Update ();
		
		if(m_timer > 0.2f){
			m_timer = 0.0f;
			cur_frame = cur_frame == 1 ? 0 : cur_frame + 1;
			spriteRenderer.sprite = pic[cur_frame];
		}else{
			m_timer += Time.deltaTime;
		}
	}
	
	protected override void Remove ()
	{
		base.Remove ();
		GameObject obj = Instantiate(label_treasure) as GameObject;
		obj.transform.position = transform.position + new Vector3(0,0,-6);
		GameObject.Find("GameManager").GetComponent<GameManager>().SendMessage("UpdateTreasureInfo", index);
	}
	
	 protected void SetIndex(int val){
		index = val;
	 }
}
