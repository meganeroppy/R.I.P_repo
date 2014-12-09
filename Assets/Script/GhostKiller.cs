using UnityEngine;
using System.Collections;

public class GhostKiller : DeadZone {

	protected Color32 m_colorVal;
	protected Color32 m_defaultColor;
	
	protected override void Start ()
	{
		base.Start();
		//GetComponent<SpriteRenderer>().color = new Color32(188,43,169,255);
		spriteRenderer.color = new Color32(188,43,169,255);
		m_colorVal = spriteRenderer.color;
		m_defaultColor = m_colorVal;
		m_awake = false;
	}
	
	protected override void Update ()
	{
		if(!GameManager.CheckCurrentPlayerIsGhost()){
			m_awake = false;
			SetAsDefault();
			return;
		}else{
			m_awake = true;
		}
		
		spriteRenderer.color = m_colorVal;
		
		Vector3 scale = skull.transform.localScale;
		skull.transform.localScale = new Vector3(m_defaultScale * m_scale, m_defaultScale * m_scale, scale.z);
		skull.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, m_alpha);
		
		if(!m_awake){
			return;
		}
		
		if(m_scale > 2.0f){
			m_scale = 1.0f;
			m_alpha = 1.0f;
		}else{
			m_scale += Time.deltaTime * 1f;
			m_alpha -= Time.deltaTime * 1f;
		}
		
		if(m_colorIncrease){
			if(m_colorVal.Equals(m_defaultColor)){
				m_colorIncrease = false;
			}else{
				m_colorVal_r += (Time.deltaTime * 255) * (188/255);
				m_colorVal_g += (Time.deltaTime * 255) * (43/255);
				m_colorVal_b += (Time.deltaTime * 255) * (169/255);
			}
			
		}else{
			if(m_colorVal.Equals(Color.black)){
				m_colorIncrease = true;
			}else{
				m_colorVal_r -= (Time.deltaTime * 255) * (188/255);
				m_colorVal_g -= (Time.deltaTime * 255) * (43/255);
				m_colorVal_b -= (Time.deltaTime * 255) * (169/255);	
			}
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
	
	private void SetAsDefault(){
		m_scale  = m_scale !=  m_defaultScale ? m_defaultScale : m_scale;
		m_alpha = m_alpha != 1.0f ? 1.0f : m_alpha;
		skull.transform.localScale = new Vector3(m_defaultScale, m_defaultScale, transform.localScale.z);
		
	}
}
