using UnityEngine;
using System.Collections;

public class SoulfulLight : Monument {
	public float gain_rate = 0.25f;

	public GameObject effect_good;
	private GameObject target;
	private float HEAL_RANGE = 2.0f;
	public Sprite lamp_turnOff;
	public Sprite lamp_turnOn;
	private float timeToTurnOn = 0.0f;

	protected override void Start(){
		base.Start();
		target = GameObject.FindWithTag("Player");
	}

	protected override void Update(){

		if(GameManager.GameOver() || GameManager.Miss()){
			return;
		}
		
		if(target == null){
			target = GameObject.FindWithTag ("Player");
		}
		Vector3 pos = transform.position;
		Vector3 targetPos = target.transform.position;		
		Vector3 distance = targetPos - pos;

		if( Mathf.Abs( distance.x) < HEAL_RANGE && Mathf.Abs( distance.y ) < HEAL_RANGE){
			if(Mathf.Floor( Time.frameCount * Time.deltaTime * 1000) % 1 == 0 ){					
				if(target != null){
					Heal(target);
				}
			}
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
	private void Heal(GameObject target){		
		target.gameObject.SendMessage("GainSpirit", 0.25f);
		//Vector3 tarGetpos = transform.position;
		//Instantiate(effect_good, pos + offset, transform.rotation );
	}
}
