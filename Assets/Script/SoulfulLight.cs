using UnityEngine;
using System.Collections;

public class SoulfulLight : StreetLight {
	public float donate_rate = 24.0f;

	public GameObject effect_good;
	private GameObject target;
	private float HEAL_RANGE = 2.0f;
	private float counter_self = 0.0f; 
	private float counter_donate = 0.0f; 
	

	protected override void Start(){
		base.Start();
		target = GameObject.FindWithTag("Player");
	}

	protected override void Update(){

		if(GameManager.GameOver() || GameManager.Miss() || GameManager.Pause()){
			return;
		}
		
		base.Update();
		
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
		
		if(counter_self > 0.5f){			
			counter_self = 0.0f;
			Vector3 offset = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(0.0f, 2.0f), -1);
			GameObject obj = Instantiate(effect_good) as GameObject;
			obj.transform.position = transform.position + offset;
			obj.transform.localScale = new Vector3(0.5f,0.5f,0.5f);
			obj.transform.parent = transform;
			//Vector3 tarGetpos = transform.position;
			//Instantiate(effect_good, pos + offset, transform.rotation );
		}else{
			counter_self += Time.deltaTime;
		}
		
	}
	private void DonateSpirit(GameObject target){		
		target.gameObject.SendMessage("GainSpirit", donate_rate * Time.deltaTime);
		
		//Make Effects
		if(counter_donate > 0.1f){
			counter_donate = 0.0f;
			Vector3 offset = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(0.0f, 2.0f), 1);
			GameObject obj = Instantiate(effect_good) as GameObject;
			obj.transform.position = target.transform.position + offset;
			obj.transform.localScale = new Vector3(0.5f,0.5f,0.5f);
			
			obj.transform.parent = target.transform;
		//Vector3 tarGetpos = transform.position;
		//Instantiate(effect_good, pos + offset, transform.rotation );
		}else{
			counter_donate += Time.deltaTime;
		}
	}
}
