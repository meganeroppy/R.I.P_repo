using UnityEngine;
using System.Collections;

public class GhostKiller : DeadZone {
	
	//private bool colorSwitchFlug = false;
	private float m_colorVal;
	
	protected override void Start ()
	{
		base.Start();
		m_colorVal = 0.0f;
	}
	
	protected override void Update ()
	{
		if(!GameManager.CheckCurrentPlayerIsGhost()){
			if(m_awake){
				SetAsDefault();
				spriteRenderer.color = Color.black;
				spriteRenderer_skull.color = new Color(1, 1, 1, 0);
				//SwitchColor();
				m_awake = false;
				
			}
			return;
		}else{
			if(!m_awake){
				m_awake = true;
				//colorSwitchFlug = true;
				//
				//SwitchColor();
			}
		}
		
		
		Vector3 scale = skull.transform.localScale;
		skull.transform.localScale = new Vector3(m_defaultScale * m_scale, m_defaultScale * m_scale, scale.z);
		spriteRenderer_skull.color = new Color(1, 1, 1, m_alpha);
		
		if(!m_awake){
			return;
		}else{		
			m_colorVal = Mathf.PingPong(Time.time * 1.5f, 1.0f);
			spriteRenderer.color = Color.Lerp(Color.black, Color.magenta, m_colorVal);
		}
		
		if(m_scale > 2.0f){
			m_scale = 1.0f;
			m_alpha = 1.0f;
		}else{
			m_scale += Time.deltaTime * 1f;
			m_alpha -= Time.deltaTime * 1f;
		}
		
		//if(colorSwitchFlug){
		//	colorSwitchFlug = false;
		//	SwitchColor();
		//}
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
		skull.transform.localScale = new Vector3(m_scale, m_scale, transform.localScale.z);
		Color newColor = new Color(0, 0, 0, m_alpha);
		skull.GetComponent<SpriteRenderer>().color = newColor;
	}
	
	
	/*
	private void SwitchColor(){
		int colorFrom = 0;
		int colorTo = 1;
		string funcName = "ColorToMagenta";
		
		if(spriteRenderer.color == Color.magenta){
			colorTo = 0;
			colorFrom = 1;
			funcName = "ColorToBlack";
		}
		
		iTween.ValueTo(gameObject, iTween.Hash("from", colorFrom, "to", colorTo, "time", 0.5f, "onupdate", funcName));
	}
	
	*/
	
	private void SwitchColor(){

		int colorFrom = 0;
		int colorTo = 1;
		string funcName = "ColorToMagenta";
		if(spriteRenderer.color == Color.magenta || !m_awake){
			colorTo = 0;
			colorFrom = 1;
			funcName = "ColorToBlack";
		}
		
		if(!m_awake){
			iTween.ValueTo(gameObject, iTween.Hash("from", colorFrom, "to", colorTo, "time", 1.0f, "onupdate", funcName));
		}else{
			iTween.ValueTo(gameObject, iTween.Hash("from", colorFrom, "to", colorTo, "time", 1.0f, "onupdate", funcName, "oncomplete", "SwitchColor"));
		}
	}
	
	private void ColorToBlack(float val){
		spriteRenderer.color = new Color(val, 0, val, 1);
		if(spriteRenderer.color == new Color(0, 0, 0, 1)){
			//colorSwitchFlug = true;
		}
	}
	
	private void ColorToMagenta(float val){
		spriteRenderer.color = new Color(val, 0, val, 1);
		if(spriteRenderer.color == new Color(1,0,1,1) ){
			//colorSwitchFlug = true;
		}
	}
}
