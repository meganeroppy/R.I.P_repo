using UnityEngine;
using System.Collections;

public class GhostKiller2 : Monument {

	private Player m_target;
	protected bool m_awake = true;
	public Sprite[] m_pic = new Sprite[4];
	protected int m_picIdx = 0;
	protected float m_counter = 0.0f;
	protected const float SPAN = 0.025f; 
	//private Collider2D[] m_colliders;
	private float attackPower = 30.0f;
	protected Vector2 blow_impact =  new Vector2(20.0f, 20.0f);
	private float m_timer = 0.0f;
	private const float DELAY = 0.2f;

	protected override void Start () {
		builtOnGround = false;
		
		base.Start();
		
		spriteRenderer = GetComponent<SpriteRenderer>();
		spriteRenderer.sprite = m_pic[0];
		float scale = GameManager.PIECE_SCALE / GameManager.DEFAULT_PIECE_SCALE;
		transform.localScale = new Vector3(transform.localScale.x * scale, transform.localScale.x * scale, scale);
		
		//m_colliders = GetComponents<Collider2D>();
	}
	
	protected override void Update ()
	{
		if(m_timer > 0.0f){
			m_timer -= Time.deltaTime;
		}
	
		if(!GameManager.CheckCurrentPlayerIsGhost()){
			if(m_awake){
				m_awake = false;
				spriteRenderer.sprite = m_pic[3];
				
				//foreach(Collider2D child in m_colliders){
				//	child.isTrigger = true;
				//}
			}
			return;
			
		}else{//Player is Ghost
			if(!m_awake){
				m_awake = true;
				m_collider.enabled = false;
				m_collider.enabled = true;
				spriteRenderer.sprite = m_pic[0];
				//foreach(Collider2D child in m_colliders){
				//	child.isTrigger = false;
				//}
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
		if(m_timer > 0.0f){
			return;
		}
		
		if (col.gameObject.tag == "Player" && !GameManager.Miss()) {
		
			if (m_target == null){
				m_target = col.GetComponent<Player> ();
			}
			
		
			if(m_target.GetStatus() != STATUS.GHOST_IDLE && m_target.GetStatus() != STATUS.GHOST_DAMAGE && m_target.GetStatus() != STATUS.DYING){
				return;
			}
			
			float dirX =  m_target.transform.position.x > transform.position.x ? 1.0f : -1.0f;
			float dirY =  m_target.transform.position.y > transform.position.y ? 1.0f : -1.0f;
			
			m_target.rigidbody2D.velocity = Vector2.zero;
			
			Vector2 force = new Vector2 (blow_impact.x * dirX, blow_impact.y * dirY);
			m_target.SendMessage("ApplyForce", force);		
			m_timer = DELAY;
			
			if(m_target.GetStatus() != STATUS.DYING){
				m_target.SendMessage("ApplySpiritDamage", attackPower);
			}
			
			
		}else if(col.gameObject.tag == "Enemy" || col.gameObject.tag == "Bullet"){
			col.SendMessage("ApplyHealthDamage", attackPower);
		} 
		
	}
	/*
	protected override void OnCollisionEnter2D (Collision2D col)
	{
		Crash(col.gameObject);
	}
*/
	protected override void OnTriggerEnter2D(Collider2D col){
		Crash(col.gameObject);
	}
	
	protected override void OnCollisionEnter2D(Collision2D col){
		Crash(col.gameObject);
	}
	
}
