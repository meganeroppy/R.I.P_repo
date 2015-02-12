using UnityEngine;
using System.Collections;

public class Boss : Enemy {


	private int cur_phase = 0;
	
	private Vector3[] movePos = new Vector3[3];
	private float moveDuration = 5.0f;
	private float m_moving = 0.0f;
	private float[] switchVal = new float[3];
	
	private float m_attackTimer;
	private float attackInterval = 4.0f;
	
	//Game Objects
	public GameObject sythe;
	public GameObject[] pets;
	
	protected override void Start ()
	{
		base.Start ();
		MAX_HEALTH = 32;
		current_health = MAX_HEALTH;
		
		cur_phase = 0;
		m_attackTimer = 0.0f;
		
		m_awaking = false;
		
		for(int i = 0 ; i < movePos.Length ; i++){
			string str = "MovePos" + i.ToString() + "(Clone)";
			movePos[i] = GameObject.Find(str).transform.position;
			switchVal[i] = (MAX_HEALTH / (movePos.Length+1) ) * (i+1);
		}
		
		//temp
		spriteRenderer.color = new Color(1.0f, 0.5f, 0.5f, 1);
	}
	
	protected override void ApplyHealthDamage (int value)
	{
		if(!m_awaking){
			return;
		}
		
		base.ApplyHealthDamage (value);
		if(current_health <= 0){
			GameObject.Find("GameManager").SendMessage("GameClear",true);
		}
		
		Debug.Log(current_health);
		
		if(cur_phase >= movePos.Length){
			return;
		}
		
		float val = MAX_HEALTH - switchVal[cur_phase];
		
		if(m_moving <= 0.0f && current_health <= val){
			GoToNextPhase();
		}
	}
	
	protected override void Update ()
	{
		base.Update ();
		
		invincible = m_moving > 0.0f ? true : false;
		
		
		if(m_awaking){
		
			if(m_attackTimer > attackInterval){
				m_attackTimer = 0.0f;
				
			SummonPets();
			//SwingSythe();
			}else{
				m_attackTimer += Time.deltaTime;
			}
			

		
		}else{//Not Awaking
			if(PlayerIsInRange() && m_moving <= 0.0f){
				Debug.Log("Awake");
				m_awaking = true;
			}
		}
		
		if(m_moving > 0.0f){
			m_moving -= Time.deltaTime;
		}
		
	}
	
	private void GoToNextPhase(){
	/*
		if(cur_phase >= movePos.Length){
			return;
		}
	*/	
		iTween.MoveTo(gameObject, movePos[cur_phase], moveDuration);
		m_moving = 5.0f;
		
		m_awaking = false;
		
		for(int i = 0 ; i < transform.childCount ; i++){
			GameObject obj = transform.GetChild(i).gameObject;
			if(obj.tag.Equals("Enemy")){
				obj.transform.parent = null;
				
				obj.SendMessage("InstantDeath");
			}else if(obj.name.Contains("Setter")){
				Destroy(obj);
			}
		}
		
		cur_phase++;
	}
	
	//Create a Sythe
	private void SwingSythe(){
		GameObject obj = Instantiate (sythe, this.transform.position, this.transform.rotation) as GameObject;
		obj.SendMessage ("Execute", current_side);
	}
	
	//Create some pets
	private void SummonPets(){
	
		int num = cur_phase > 2 ? 4 : 2;
		
		for(int i = 0 ; i < num ; i++){
			int petKey = Random.Range(0, pets.Length);
			
			float offsetY = 3.0f;
			float offsetX = i % 2 == 0 ? -4.0f : 4.0f;
			if(i >= 2){
				offsetY += 1.0f;
				offsetX *= 0.75f;
			}
			
			
			Vector3 summonPos = m_target.transform.position + new Vector3(offsetX, offsetY, 0);
			
			GameObject obj = Instantiate (pets[petKey], summonPos, this.transform.rotation) as GameObject;
			obj.SendMessage("SetAsOneShot", gameObject);
		}
	}
}
