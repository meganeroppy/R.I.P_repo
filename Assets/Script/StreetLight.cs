using UnityEngine;
using System.Collections;

public class StreetLight : Monument {
	
	public Sprite lamp_turnOff;
	public Sprite lamp_turnOn;
	protected float timeToTurnOn = 0.0f;
	
	protected override void Start(){
		base.Start();
	}
	
	protected override void Update(){
		
		if(GameManager.GameOver() || GameManager.Miss() || GameManager.Pause()){
			return;
		}

		
		if(Random.Range(0.0f, 100.0f) < 1.0f && timeToTurnOn < 0.0f){
			GetComponent<SpriteRenderer>().sprite = lamp_turnOff;
			timeToTurnOn = Random.Range(0.05f, 0.3f);
		}
		
		if(timeToTurnOn >= 0.0f){
			timeToTurnOn -= Time.deltaTime;
			if(timeToTurnOn < 0.0f){
				GetComponent<SpriteRenderer>().sprite = lamp_turnOn;
			}
		}
		
	}

}
