using UnityEngine;
using System.Collections;

public class SpiritBar_nGUI : MonoBehaviour {
	
	private Player player;
	private bool activated = false;
	private UISprite back;
	private UISprite fore;
	private Color32 DEFAULT_COLOR = new Color32(58, 124, 157, 255);
	private float colorDiff = 0.0f;
	private Color tempColor;
	
		
		// Use this for initialization
	void Start () {
		back = GameObject.Find("Background").GetComponent<UISprite>();
		fore = GameObject.Find("Foreground").GetComponent<UISprite>();
	}
		
		// Update is called once per frame
		void Update () {
		if(!activated){
			return;
		}
		
		
		if(player == null){
			player = GameObject.FindWithTag ("Player").GetComponent<Player> ();
		}

		float[] spirit = player.GetSpiritInfo ();
		float value_percent = spirit[1] / spirit[0];
		UISlider slider = GetComponent<UISlider>();
		slider.direction = UISlider.Direction.Horizontal;
		slider.sliderValue = value_percent;
		
		if(spirit[1] <= 0.0f){
			back.color = Color.gray;
		}else{
			if(spirit[2] > 0){
				float val = Mathf.PingPong(Time.time * 1.5f, 1.0f);
				back.color = Color.Lerp(DEFAULT_COLOR, Color.white, val);
			}else if(spirit[2] < 0){
				float val = Mathf.PingPong(Time.time * 1.5f, 1.0f);
				back.color = Color.Lerp(DEFAULT_COLOR, Color.red, val);	
			}else{
				if(back.color != DEFAULT_COLOR){
					if(colorDiff <= 0.0f){
						colorDiff = 1.0f;
						tempColor = back.color;
					}
					colorDiff -= Time.deltaTime * 3.0f;
					back.color = Color.Lerp(DEFAULT_COLOR, tempColor, colorDiff);	
					
				
				}
				
				//back.color = DEFAULT_COLOR;
				
			}
		}
		
		
	}
	
	private void Activate(){
		activated = true;
	}
	
}
