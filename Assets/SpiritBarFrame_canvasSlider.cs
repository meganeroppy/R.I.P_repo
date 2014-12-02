using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SpiritBarFrame_canvasSlider : MonoBehaviour {

	
	private Vector2 m_basePos_spiritBar;
	private Vector2 m_scale_spiritBarFrame;	
	
	public Vector2 pos = new Vector2 (0.0f, 0.0f);
	public Vector2 size = new Vector2 (100.0f, 100.0f);
	
	//Player 
	private Player player;
	private Slider slider;
	
	// Use this for initialization
	void Start () {
		if(Application.loadedLevelName == "Title"){
			return;
		}

	}
	
	// Update is called once per frame
	void Update () {
		
		if(player == null){
			player = GameObject.FindWithTag ("Player").GetComponent<Player> ();
		}
		
		float[] spirit = player.GetSpiritInfo ();
		float value_percent = spirit[1] / spirit[0];
		slider = this.GetComponent<Slider>();
		
		//slider.position = pos;
		//slider.sizeDelta = size;
		//slider.pivot = new Vector2(0.0f, 0.5f);
		slider.value = value_percent;
		
	}
}
