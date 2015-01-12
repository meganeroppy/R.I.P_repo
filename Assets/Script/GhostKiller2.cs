using UnityEngine;
using System.Collections;

public class GhostKiller2 : StageObject {

	protected bool m_awake = true;
	public Sprite[] m_pic = new Sprite[4];
	protected int m_picIdx = 0;
	protected float m_counter = 0.0f;
	protected const float SPAN = 0.025f; 
	private Collider2D[] m_colliders;
	private float attackPower = 30.0f;
	protected Vector2 blow_impact =  new Vector2(50.0f, 50.0f);
	

	protected override void Start () {
		
		spriteRenderer = GetComponent<SpriteRenderer>();
		spriteRenderer.sprite = m_pic[0];
		float scale = GameManager.PIECE_SCALE / GameManager.DEFAULT_PIECE_SCALE;
		transform.localScale = new Vector3(transform.localScale.x * scale, transform.localScale.x * scale, scale);
		
		m_colliders = GetComponents<Collider2D>();
	}
	
	protected override void Update ()
	{
	
		if(!GameManager.CheckCurrentPlayerIsGhost()){
			if(m_awake){
				m_awake = false;
				spriteRenderer.sprite = m_pic[3];
				
				foreach(Collider2D child in m_colliders){
					child.isTrigger = true;
				}
			}
			return;
		}else{
			if(!m_awake){
				m_awake = true;
				spriteRenderer.sprite = m_pic[0];
				foreach(Collider2D child in m_colliders){
					child.isTrigger = false;
				}
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
	
	protected void Crash(GameObject col){
		if(!m_awake){
			return;
		}
		if (col.gameObject.tag == "Player" && !GameManager.Miss()) {
			if(col.gameObject.GetComponent<Player>().CheckIsLiving()){
				return;
			}
			col.SendMessage("ApplySpiritDamage", attackPower);
			float dirX =  col.transform.position.x > transform.position.x ? 1.0f : -1.0f;
			float dirY =  col.transform.position.y > transform.position.y ? 1.0f : -1.0f;
			
			col.rigidbody2D.velocity = Vector2.zero;
			col.rigidbody2D.AddForce (new Vector2 (blow_impact.x * dirX, blow_impact.y * dirY));
			
		}else if(col.gameObject.tag == "Enemy" || col.gameObject.tag == "Bullet"){
			col.SendMessage("ApplyHealthDamage", attackPower);
		} 
	}
	
	protected override void OnCollisionEnter2D (Collision2D col)
	{
		Crash(col.gameObject);
	}

	protected override void OnTriggerEnter2D(Collider2D col){
		Crash(col.gameObject);
	}
	
	protected virtual void OnTriggerStay2D(Collider2D col){
		if(!m_awake){
			return;
		}
		if (col.gameObject.tag == "Player" && !GameManager.Miss()) {
			if(col.gameObject.GetComponent<Player>().CheckIsLiving()){
				return;
			}
			col.SendMessage("ApplySpiritDamage", attackPower);
			
			float dirX =  col.transform.position.x > transform.position.x ? 1.0f : -1.0f;
			float dirY =  col.transform.position.y > transform.position.y ? 1.0f : -1.0f;
			
			col.rigidbody2D.velocity = Vector2.zero;
			col.rigidbody2D.AddForce (new Vector2 (blow_impact.x * dirX, blow_impact.y * dirY));
			
		}else if(col.gameObject.tag == "Enemy" || col.gameObject.tag == "Bullet"){
			col.SendMessage("ApplyHealthDamage", attackPower);
		} 
	}
	
}
