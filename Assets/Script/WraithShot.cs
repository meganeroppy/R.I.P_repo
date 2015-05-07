using UnityEngine;
using System.Collections;

public class WraithShot : Bullet {

	protected float offsetEularZ = -90.0f;
	public Sprite[] pics;
	protected int current_frame = 0;
	protected float m_timer = 0.0f;
	protected float animInterval = 0.075f;
	
	protected override void Start ()
	{
		base.Start ();
		speed = 7.0f;
		spriteRenderer = GetComponent<SpriteRenderer>();
		lifeTime = 4.0f;
		transform.localScale = new Vector3(0.7f,0.7f,0.7f);
		m_alpha = 1;
	}
	
	protected override void Update ()
	{
		base.Update ();
		
		if(m_timer > animInterval && !dying){
			m_timer = 0.0f;
			current_frame = current_frame == 2 ? 0 : current_frame+1;
			spriteRenderer.sprite = pics[current_frame];
		}else{
			m_timer += Time.deltaTime;
		}
		
		if(dying){
			if(m_alpha <= 0.0f){
				Destroy(this.gameObject);
			}else{
				m_alpha -= Time.deltaTime;
				SetAlpha(m_alpha);
			}
		}
		
		if(!GameManager.CheckCurrentPlayerIsGhost() || GameManager.CheckCurrentPlayerIsHidden()){
			Die();
		}
	}
	
	protected override void OnTriggerEnter2D(Collider2D col){
		return;
	}
	
	protected override void OnCollisionEnter2D(Collision2D col){
		if(col.gameObject.tag.Equals("Player")){
			Crash(col.gameObject);
			Die();
		}
	}	
	
	protected override void ApplyHealthDamage(int value){
	//	Die();
	}
	
	protected override void Die(){
		if(dying){
			return;
		}
			
		dying = true;
		
		rigidbody2D.velocity = Vector2.zero;
		spriteRenderer.sprite = pics[3];
		m_collider.enabled = false;
	}
	
	protected override void Crash(GameObject other){
		
		//Debug.Log("HIT" + Time.realtimeSinceStartup.ToString());
		
		Player  m_target = other.GetComponent<Player> ();
		
		if (m_target.GetStatus() != STATUS.DYING) {
			m_target.SendMessage ("ApplySpiritDamage", attack_power);
			float dir = 1.0f;
			if (this.gameObject.transform.position.x > m_target.transform.position.x) {
				dir *= -1.0f;
			}
			m_target.rigidbody2D.velocity = Vector2.zero;
			m_target.rigidbody2D.AddForce (new Vector2 (blow_impact.x * dir, blow_impact.y));
		}
	}
}
