using UnityEngine;
using System.Collections;

public class GhostKiller2 : StageObject {

	protected bool m_awake = true;
	public Sprite[] m_pic = new Sprite[4];
	protected int m_picIdx = 0;
	protected float m_counter = 0.0f;
	protected const float SPAN = 0.025f; 
	protected SpriteRenderer spriteRenderer;

	protected override void Start () {
		spriteRenderer = GetComponent<SpriteRenderer>();
		spriteRenderer.sprite = m_pic[0];
		float scale = GameManager.PIECE_SCALE / GameManager.DEFAULT_PIECE_SCALE;
		transform.localScale = new Vector3(transform.localScale.x * scale, transform.localScale.x * scale, scale);
	}
	
	protected override void Update ()
	{
	
		if(!GameManager.CheckCurrentPlayerIsGhost()){
			if(m_awake){
				m_awake = false;
				spriteRenderer.sprite = m_pic[3];
			}
			return;
		}else{
			if(!m_awake){
				m_awake = true;
				spriteRenderer.sprite = m_pic[0];
			}
		}
	
		
		if(!m_awake){
			return;
		}
		
		if(m_counter > SPAN){
			m_counter = 0.0f;
			m_picIdx = m_picIdx == 2 ? 0 : m_picIdx + 1;
			spriteRenderer.sprite = m_pic[m_picIdx];
		}else{
			m_counter += Time.deltaTime;
		}
		

	}
	
	protected void OnCollisionEnter2D (Collision2D col)
	{
		if (col.gameObject.tag == "Player" && !GameManager.Miss()) {
			if(col.gameObject.GetComponent<Player>().CheckIsLiving()){
				return;
			}
			col.gameObject.SendMessage("Miss");
		}	
	}

	protected override void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.tag == "Player" && !GameManager.Miss()) {
			if(col.gameObject.GetComponent<Player>().CheckIsLiving()){
				return;
			}
			col.SendMessage("GetExorcised");
		}else if(col.gameObject.tag == "Enemy" || col.gameObject.tag == "Bullet"){
			col.SendMessage("ApplyHealthDamage", 9999.9f);
		} 
	}
}
