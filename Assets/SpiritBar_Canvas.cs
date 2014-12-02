using UnityEngine;
using System.Collections;

public class SpiritBar_Canvas : MonoBehaviour
{
	
	private Vector2 m_basePos_spiritBar;
	private Vector2 m_scale_spiritBarFrame;	
	
	public Vector2 pos = new Vector2 (0.0f, 0.0f);
	public Vector2 size = new Vector2 (100.0f, 100.0f);
	
	//Player 
	private Player player;
	private RectTransform m_rect;
	
	// Use this for initialization
	void Start () {
		if(Application.loadedLevelName == "Title"){
			return;
		}
		m_rect = this.GetComponent<RectTransform>();
		
		m_rect.position = pos;
		m_rect.sizeDelta = size;
		m_rect.pivot = new Vector2(0.0f, 0.5f);
	}
	
	// Update is called once per frame
	void Update () {
	
		if(player == null){
			player = GameObject.FindWithTag ("Player").GetComponent<Player> ();
		}
				
		float[] spirit = player.GetSpiritInfo ();
		float value_percent = spirit[1] / spirit[0];
		m_rect.position = pos;
		m_rect.sizeDelta = size;
		m_rect.pivot = new Vector2(0.0f, 0.5f);
		m_rect.transform.localScale = new Vector3(value_percent, 1, 1);
		
	}
}

