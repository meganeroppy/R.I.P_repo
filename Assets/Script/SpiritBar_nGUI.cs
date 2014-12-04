using UnityEngine;
using System.Collections;

public class SpiritBar_nGUI : MonoBehaviour {

						
		//Player 
		private Player player;
		private bool activated = false;
		
		// Use this for initialization
		void Start () {
					
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
		}
	
	private void Activate(){
		activated = true;
	}
	
}
