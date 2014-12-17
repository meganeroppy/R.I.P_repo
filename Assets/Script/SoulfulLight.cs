using UnityEngine;
using System.Collections;

public class SoulfulLight : Monument {
	public float donate_rate = 24.0f;

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

		if(GameManager.GameOver() || GameManager.Miss() || GameManager.Pause()){
			return;
		}
		
		if(target == null){
			target = GameObject.FindWithTag ("Player");
		}
		Vector3 pos = transform.position;
		Vector3 targetPos = target.transform.position;		
		Vector3 distance = targetPos - pos;

		if( Mathf.Abs( distance.x) < HEAL_RANGE && Mathf.Abs( distance.y ) < HEAL_RANGE){
			//if(Mathf.Floor( Time.frameCount * Time.deltaTime * 1000) % 1 == 0 ){					
				if(target != null){
					DonateSpirit(target);
				}
				
			//}
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
		
		
		
		if(Mathf.Floor( Time.frameCount ) % Random.Range(50, 70) == 0 ){					
			Vector3 offset = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(0.0f, 2.0f), 1);
			GameObject obj = Instantiate(effect_good, transform.position + offset, transform.rotation) as GameObject;
			obj.transform.localScale = new Vector3(0.5f,0.5f,0.5f);
			obj.transform.parent = transform;
			//Vector3 tarGetpos = transform.position;
			//Instantiate(effect_good, pos + offset, transform.rotation );
		}
		
	}
	private void DonateSpirit(GameObject target){		
		target.gameObject.SendMessage("GainSpirit", donate_rate * Time.deltaTime);
		
		//Make Effects
	//	if(Mathf.Floor( Time.frameCount * Time.deltaTime * 1000) % 10 == 0 ){					
		if(Mathf.Floor( Time.frameCount ) % 5 == 0 ){					
				Vector3 offset = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(0.0f, 2.0f), 1);
			GameObject obj = Instantiate(effect_good, target.transform.position + offset, transform.rotation) as GameObject;
			obj.transform.localScale = new Vector3(0.5f,0.5f,0.5f);
			
			obj.transform.parent = target.transform;
		//Vector3 tarGetpos = transform.position;
		//Instantiate(effect_good, pos + offset, transform.rotation );
		}
	}
}
