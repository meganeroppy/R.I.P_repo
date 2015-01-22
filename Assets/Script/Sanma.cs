using UnityEngine;
using System.Collections;

public class Sanma : Item {

	private const float LIFETIME = 5.0f;
	private float m_timer = 0.0f;
	private float m_alpha = 1.0f;
	private bool dying = false;
	

	protected override void Start () {
		base.Start();
		item_type = ITEM_TYPE.GAIN_SPIRIT;
		
	}
	
	protected override void Update () {
		if(!dying){
			if(m_timer > LIFETIME){
				dying = true;	
			}
			m_timer += Time.deltaTime;
		}else{
			if(m_alpha < 0.0f){
				Destroy(this.gameObject);
			}else{
				m_alpha -= Time.deltaTime;
				SetAlpha(m_alpha);
			}
		}
	}
	
	
}
