using UnityEngine;
using System.Collections;

public class  Shade : Enemy {

	enum MODE{
		NONE,
		PREP_CUTTER,
		CUTTER,
		PREP_SUMMON,
		SUMMON,
		PREP_SYTHE,
		SYTHE,
		PREP_SHOOT,
		SHOOT,
	};
	private MODE cur_mode = MODE.NONE;

	private const float INTERVAL_BLOCKS = 3.2f;
	private int cur_phase = 0;
	
	private Vector3[] movePos = new Vector3[3];
	private float moveDuration = 5.0f;
	private float m_moving = 0.0f;
	private float[] switchVal = new float[3];
	private float preparationTime = 2.0f;
	
	private float m_attackTimer;
	private float m_warpTimer;
	private float m_shootTimer;
	private const float SHOOT_INTERVAL = 0.2f;
	private int bulletCount = 0;
	private int maxBullet = 5;
	private CircleCollider2D m_circleCollider;
	
	//Game Objects
	public GameObject wraithHead;
	public GameObject[] pets;
	public GameObject effect_pop;
	public GameObject energyBall;
	public GameObject sythe;
	
	private bool m_escaping = false;
	
	protected override void Start ()
	{
		base.Start ();
		MAX_HEALTH = 100;
		current_health = MAX_HEALTH;
		
		cur_mode = MODE.NONE;
		
		attack_power = 25.0f;
		cur_phase = 0;
		m_attackTimer = 1.0f;
		m_warpTimer = 2.0f;
		m_awaking = false;
		m_circleCollider = GetComponent<CircleCollider2D>();
		
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
			KillPets();
			GameObject.Find("GameManager").SendMessage("GameClear", 2.0f);
		}
		
		//Debug.Log(current_health);
		
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
		
		if(current_health <= 0){
		CancelInvoke();
		}
		if (m_moving > 0.0f) {
			invincible = true;
		}

		m_circleCollider.enabled = m_awaking;

		Debug.Log (current_health);
		//Switching Attaack mode
		if(m_awaking){
			if(m_attackTimer <= 0.0f && cur_mode == MODE.NONE){
			
				//Choose attack skill
				int choice = cur_phase < 2 ? Random.Range(0,3) : Random.Range(0,4);
				if(choice == 0){
				cur_mode = MODE.PREP_SUMMON;
					anim.SetTrigger("t_chargeMagic");
					int petCount = 0;
					for(int i = 0 ; i < transform.parent.transform.childCount ; i++){
						
						GameObject obj = transform.parent.transform.GetChild(i).gameObject;
						if(obj.tag.Equals("Enemy") && obj.gameObject != this.gameObject){
							petCount++;
						}
					}
					
					if(petCount < 8){
						m_attackTimer += 4.0f;
						
						Invoke("SummonPets", preparationTime);
					}
					
				}else if(choice == 1){
				cur_mode = MODE.PREP_CUTTER;
					anim.SetTrigger("t_chargeMagic");
					int row = 5;
					int line = cur_phase >= 2 ? 2 : 1;
					float timeInterval = 0.5f;
					
					m_attackTimer += timeInterval * (row * line) + 3.0f;
					
					Invoke("CutterRain", preparationTime);
				}else if(choice == 2){
					anim.SetTrigger("t_chargeMagic");
					m_escaping = true;
					Warp();
					cur_mode = MODE.PREP_SHOOT;
					Invoke("SwitchToShootMode", preparationTime);
				}else if(choice == 3){
					anim.SetTrigger("t_chargeSwing");
					m_attackTimer += 4.0f;
					cur_mode = MODE.PREP_SYTHE;
					Invoke("SwingSythe", preparationTime);
				}
			
			}else{
				m_attackTimer -= Time.deltaTime;
			}
			
			//Debug.Log(m_attackTimer);
			
			switch(cur_mode){
				case MODE.SHOOT :{
					if(m_shootTimer <= 0.0f){
	
						m_shootTimer = SHOOT_INTERVAL;
						
						ShootToTarget();
						
						if(bulletCount >= maxBullet){
						m_attackTimer += 3.0f;
							cur_mode = MODE.NONE;
						}
						
					}else{
						m_shootTimer -= Time.deltaTime;
					}
				}
				
				break;
			case MODE.CUTTER:
			case MODE.SUMMON:
			case MODE.SYTHE:
				cur_mode = MODE.NONE;
			break;
				
			default:
				break;
			}
			
			if(m_warpTimer <= 0.0f){
				Warp();
			}else if(cur_mode == MODE.NONE){
				m_warpTimer -= Time.deltaTime;
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
		anim.SetTrigger("t_idle");
		
	/*
		if(cur_phase >= movePos.Length){
			return;
		}
	*/
	
		int num = 0;
		float[] spirit = m_target.GetSpiritInfo();
		float spiritRate =spirit[1] / spirit[0];
		if(spiritRate <= 0.25f){
			num = 2;
		}else if(spiritRate <= 0.5f){
			num = 1;
		}
		
		while(num > 0){
			Instantiate(m_dropItem, transform.position, transform.rotation);
			num--;
		}
		iTween.MoveTo(gameObject, movePos[cur_phase], moveDuration);
		m_moving = 5.0f;
		
		m_awaking = false;
		
		KillPets();
		
		cur_phase++;
	}
	
	//Swing a Sythe
	private void SwingSythe(){
		cur_mode = MODE.SYTHE;
	
		anim.SetTrigger("t_swing");
				
		Vector3 pos = transform.position;
		Vector3 offset = new Vector3(current_side == SIDE.RIGHT ? 7.0f : -7.0f, 0.0f, -1.0f);
		
		GameObject obj = Instantiate (sythe) as GameObject;
		obj.SendMessage("ApplyParentAndExecute", this);
		
		obj.transform.position = new Vector3 (pos.x + offset.x, pos.y + offset.y, pos.z + offset.z);
		sound.PlaySE("Attack2", 1.0f);
				
		/*
		float offsetX = 15.36f  * (cur_phase >= 2 ? (Random.Range(0,2) == 1 ? -1 : 1) : 1);
		float delayBase = 2.5f;
		
		Vector3 pos = m_circleCollider.transform.position;
		Vector3 targetPos = m_target.transform.position;
		Vector3 targetColsCenter = new Vector3( targetPos.x + (m_targetCols[0].center.x + m_targetCols[1].center.x)/2, targetPos.y + (m_targetCols[0].center.y + m_targetCols[1].center.y)/2, m_target.transform.position.z);
		
		Vector3 targetBlockPos = AnalizeBlockPos(targetColsCenter);  
		
		//effect
		GameObject effect = Instantiate(effect_pop) as GameObject;
		//sythe
		GameObject obj = Instantiate (sythe) as GameObject;
		obj.transform.position = new Vector3(targetBlockPos.x + offsetX , targetBlockPos.y , pos.z);
		obj.transform.SetParent(transform.parent.transform);
		Vector3 scale = obj.transform.localScale;
		obj.transform.localScale = new Vector3(scale.x * 5.0f, scale.y * 5.0f, scale.z);
		effect.transform.position = obj.transform.position + new Vector3(0,0,-1);
		effect.transform.SetParent(obj.transform.parent.transform);
		scale = obj.transform.localScale;
		effect.transform.localScale = new Vector3(scale.x * 3.0f, scale.y * 3.0f, scale.z);
		
		float delay = delayBase ;
		obj.SendMessage("Wait",  delay);
		obj.SendMessage ("Execute", offsetX > 0 ? SIDE.LEFT : SIDE.RIGHT);
*/
		
	}
	
	//Create Cutters
	private void CutterRain(){
		cur_mode = MODE.CUTTER;
		anim.SetTrigger("t_magic");
		
		float interval = INTERVAL_BLOCKS;
		float timeInterval = 0.5f;
		int row = 5;
		int line = cur_phase >= 2 ? 2 : 1;
		float offsetToCenter = ( interval * ((row-1) * 0.5f) );
		float offsetX = 10.24f  * (cur_phase >= 2 ? (Random.Range(0,2) == 1 ? -1 : 1) : 1);
		float offsetY = -0.5f;
		float delayBase = 0.5f;
		bool reverse = Random.Range(0, 2) == 0 ? true : false;
		
		Vector3 pos = m_circleCollider.transform.position;
		Vector3 targetPos = m_target.transform.position;
		Vector3 targetColsCenter = new Vector3( targetPos.x + (m_targetCols[0].center.x + m_targetCols[1].center.x)/2, targetPos.y + (m_targetCols[0].center.y + m_targetCols[1].center.y)/2, m_target.transform.position.z);
		
		Vector3 targetBlockPos = AnalizeBlockPos(targetColsCenter);
		
		
		for(int j=0 ; j < line ; j++){
			for(int i=0 ; i < row ; i++){
				//effect
				GameObject effect = Instantiate(effect_pop) as GameObject;
				//wraithHead
				GameObject obj = Instantiate (wraithHead) as GameObject;
				obj.transform.position = new Vector3(targetColsCenter.x + offsetX , targetBlockPos.y + ( (i * interval) - offsetToCenter ) + offsetY, pos.z);
				obj.transform.parent = transform.parent.transform;
				effect.transform.position = obj.transform.position + new Vector3(0,0,-1);
				effect.transform.parent = obj.transform.parent.transform;
				
				float delay = delayBase + (timeInterval * (reverse ? (row - i) : i) ) + ( (timeInterval * row) * j );
				obj.SendMessage("Wait",  delay);
				obj.SendMessage ("Execute", offsetX > 0 ? SIDE.LEFT : SIDE.RIGHT);
			}
		}
		
	}
	
	private void ShootToTarget(){
		bulletCount++;
		
		//anim.SetTrigger("t_attack");
				
		Vector3 pos = transform.position;
		Vector3 myColsCenter = pos + new Vector3 (m_circleCollider.center.x, m_circleCollider.center.y, 0.0f);
		Vector3 targetPos = m_target.transform.position;
		Vector3 targetColsCenter = new Vector3( targetPos.x + (m_targetCols[0].center.x + m_targetCols[1].center.x)/2, targetPos.y + (m_targetCols[0].center.y + m_targetCols[1].center.y)/2, m_target.transform.position.z);
		
		Vector2 offset;
		offset.x = Random.Range (-1, 1);
		offset.y = Random.Range (-0.1f, 0.1f);
		
		Vector3 dir = (targetColsCenter - myColsCenter).normalized;
		
		float rad = Mathf.Atan2(dir.y, dir.x);		
		float theta = rad * Mathf.Rad2Deg;
		
		float offsetTheta =  Random.Range(-20.0f, 20.0f);
		
		float vx = Mathf.Cos(Mathf.PI / 180 * (theta + offsetTheta ));
		float vy = Mathf.Sin(Mathf.PI / 180 * (theta + offsetTheta ));
		
		GameObject obj= Instantiate (energyBall, new Vector3(myColsCenter.x + offset.x, myColsCenter.y + offset.y, pos.z), transform.rotation) as GameObject;
		obj.SendMessage("SetAttackPower", attack_power * 0.5f);
		obj.SendMessage("SetDirectionAndExecute", new Vector2(vx, vy));
		
		
		///


	}
	
	//Create some pets
	private void SummonPets(){
		cur_mode = MODE.SUMMON;
		anim.SetTrigger("t_magic");
		
		int num = cur_phase > 2 ? 4 : 2;
		
		for(int i = 0 ; i < num ; i++){
			int petKey = Random.Range(0, cur_phase > 2 ? pets.Length : pets.Length-1);
			
			float offsetY = 3.0f;
			float offsetX = i % 2 == 0 ? -6.0f : 6.0f;
			if(i >= 2){
				offsetY += 1.2f;
				offsetX *= 2.0f;
			}
			
			
			Vector3 summonPos = m_target.transform.position + new Vector3(offsetX, offsetY, 0);
			
			GameObject obj = Instantiate (pets[petKey], summonPos, this.transform.rotation) as GameObject;
			obj.transform.parent = transform.parent.transform;
			obj.SendMessage("SetAsOneShot", gameObject);
		}
		
	}
	
	private void KillPets(){
		int idx = transform.parent.transform.childCount-1;
		
		while(idx > -1){
			
			GameObject obj = transform.parent.transform.GetChild(idx).gameObject;
			if(obj.tag.Equals("Enemy") && obj.gameObject != this.gameObject){
				obj.transform.parent = null;
				obj.SendMessage("InstantDeath");
			}else if(obj.name.Contains("Setter") || obj.name.Contains("wraithHead")){
				obj.transform.parent = null;
				Destroy(obj);
			}
			idx--;
			
		}
	}
	
	private Vector3 AnalizeBlockPos(Vector3 colPos){
		float val = Mathf.Floor(colPos.x / INTERVAL_BLOCKS);
		float blockPosX = ( val * INTERVAL_BLOCKS ) + INTERVAL_BLOCKS;
		val = Mathf.Floor((colPos.y / INTERVAL_BLOCKS));
		float blockPosY = ( val * INTERVAL_BLOCKS ) + INTERVAL_BLOCKS;
		
		return new Vector3(blockPosX, blockPosY, colPos.z);
	}
	
	private void Warp(){
		m_warpTimer =2.0f;		
		
		m_circleCollider.enabled = false;
		anim.SetTrigger("t_warpStart");
		Invoke("ShowUp", 0.5f);
	}
	
	private void ShowUp(){
	
		
		m_circleCollider.enabled = true;
		anim.SetTrigger("t_warpEnd");
		
		Vector3 newPos;
		int row = 5;
		int line = 2;
		Vector3[] posChioces = new Vector3[(line * row) - 1];
		int idx = 0;
		float diff = INTERVAL_BLOCKS;
		Vector3 targetPos = m_target.transform.position;
		Vector3 basePos = targetPos + new Vector3( -diff * (row / 2),0,0);
		
		for(int i = m_escaping ? 2 : 1; i < (m_escaping ? line+1 : line) ; i++){
			for(int j = 0 ; j < row ; j++){
				Vector3 tmpPos = new Vector3(basePos.x + (diff*j), basePos.y + (diff*i), basePos.z);
				if(tmpPos != targetPos && !(i == 1 && tmpPos.x == targetPos.x)){
					posChioces[idx] = tmpPos; 
					idx++;
				}
			}
		}
		
		int numOfChoices = 0;
		for(int i = 0 ;  i < posChioces.Length ; i++){
			numOfChoices += posChioces[i] == Vector3.zero ? 0 : 1;
		}
		
		newPos = posChioces[Random.Range(0, numOfChoices)];
		
		//GameObject obj = Instantiate(effect_pop) as GameObject;
		//obj.transform.position = newPos;
		transform.position = newPos;
		
		//Look at peace
		if(transform.position.x < m_target.transform.position.x){
			Flip(SIDE.RIGHT);//Means Left
		}else{
			Flip(SIDE.LEFT);//Means Right
		}
		
		m_escaping = false;
		
	}
	
	private void SwitchToShootMode(){
		anim.SetTrigger("t_magic");
		Debug.Log("Switch");
		
		bulletCount = 0;
		maxBullet = 4 + (2 * cur_phase);
		m_attackTimer += 1.0f;
		m_shootTimer = 0.0f;
		cur_mode = MODE.SHOOT;
	}
}
