using UnityEngine;
using System.Collections;

public class Doll : Item {

	private bool m_awake;

	// Use this for initialization
	protected override void Start () {
		base.Start();
		item_type = ITEM_TYPE.REVIVAL;
		m_awake = false;
		m_collider.enabled = false;
		effect.enabled = false;
		m_alpha = 0.0f;
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update();
		
		if(m_awake){
			if(!GameManager.CheckCurrentPlayerIsGhost()){
				m_awake = false;
				m_collider.enabled = false;
				effect.enabled = false;
			}
			
			if(m_alpha <= 1.0f){
				m_alpha += Time.deltaTime;
				SetAlpha(m_alpha);
			}	
		
		}else{//Not awake
			if(GameManager.CheckCurrentPlayerIsGhost()){
				m_awake = true;
				m_collider.enabled = true;
				effect.enabled = true;
				
				Instantiate(effect_pop, transform.position + new Vector3(0,0,-1), transform.rotation);
				
			}
			
			if(m_alpha > 0.0f){
				m_alpha -= Time.deltaTime;
				SetAlpha(m_alpha);
			}	
		}
		
	}
}
