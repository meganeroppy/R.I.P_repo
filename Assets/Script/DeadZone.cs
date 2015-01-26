using UnityEngine;
using System.Collections;

public class DeadZone : StageObject {

	public GameObject skull;
	protected float m_scale = 1.0f;
	protected float m_defaultScale;
	protected float m_alpha = 1.0f;
	protected float m_colorVal_r = 0.0f;
	protected float m_colorVal_g = 0.0f;
	protected float m_colorVal_b = 0.0f;
	protected bool m_colorIncrease = true;
	
	protected SpriteRenderer spriteRenderer_skull;
	protected bool m_awake = true;
	protected GameManager gameManager;
	
	// Use this for initialization
	protected override void Start () {
	//	gameManager = GameObject.Find ("GameManager").GetComponent<GameManager> ();
		Vector3 pos = transform.position;
		skull = Instantiate(skull, new Vector3 (pos.x, pos.y, pos.z - 1.0f), transform.rotation) as GameObject;
		skull.transform.parent = transform;
		m_defaultScale = skull.transform.localScale.x;
		spriteRenderer_skull = skull.GetComponent<SpriteRenderer>();
		
		spriteRenderer = GetComponent<SpriteRenderer>();
		spriteRenderer.color = new Color(m_colorVal_r, m_colorVal_g, m_colorVal_b, 1);
		
		gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
		
	}
	
	// Update is called once per frame
	protected override void Update () {
	
		spriteRenderer.color = new Color(m_colorVal_r, m_colorVal_g, m_colorVal_b, 1);
	
		Vector3 scale = skull.transform.localScale;
		skull.transform.localScale = new Vector3(m_defaultScale * m_scale, m_defaultScale * m_scale, scale.z);
		spriteRenderer_skull.color = new Color(1, 1, 1, m_alpha);
	
		if(!m_awake){
			if(GameManager.CheckCurrentPlayerIsGhost()){
				m_awake = true;
				spriteRenderer.enabled = true;;
				spriteRenderer_skull.enabled = true;
			}
		}else{
			
			if(m_scale > 2.0f){
				m_scale = 1.0f;
				m_alpha = 1.0f;
			}else{
				m_scale += Time.deltaTime * 1f;
				m_alpha -= Time.deltaTime * 1f;
			}
		
			if(m_colorIncrease){
				if(m_colorVal_r >= 0.5f){
					m_colorIncrease = false;
				}
				m_colorVal_r += Time.deltaTime;
			}else{
				if(m_colorVal_r <= 0.0f){
					m_colorIncrease = true;
				}
				m_colorVal_r -= Time.deltaTime;
			}
			
			if(!GameManager.CheckCurrentPlayerIsGhost()){
				m_awake = false;
				spriteRenderer.enabled = false;;
				spriteRenderer_skull.enabled = false;
			}
		}
	}

	protected override void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.tag == "Player" && !GameManager.Miss()){
			if(GameManager.CheckCurrentPlayerIsGhost()){
				col.SendMessage("GetExorcised");
			}else{
				gameManager.SendMessage("Miss", true);
			}
		}else if(col.gameObject.tag == "Enemy" || col.gameObject.tag == "Bullet"){
			col.SendMessage("ApplyHealthDamage", 9999.9f);
		} 
	}
}
