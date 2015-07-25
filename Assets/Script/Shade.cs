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
		WARP_START,
		WARP_END
	};
	private MODE cur_mode = MODE.NONE;

	private const float INTERVAL_BLOCKS = 3.2f;
	private int cur_phase = 0;
	
	private Vector3[] movePos = new Vector3[3];
	private float moveDuration = 5.0f;
	private float m_moving = 0.0f;
	private float[] switchVal = new float[3];
	private float preparationTime = 1.0f;
	private float prepareTime_sythe = 2.5f;
	
	private float m_attackTimer;
	private float m_warpTimer;
	private float m_shootTimer;
	private const float SHOOT_INTERVAL = 0.2f;
	private int bulletCount = 0;
	private int maxBullet = 5;
	private CircleCollider2D m_circleCollider;
	
	//Game Objects
	public WraithHead wraithHead;
	public GameObject[] pets;
	public GameObject effect_pop;
	public GameObject effect_pop2;
	public BossBullet energyBall;
	public AttackZone_sythe sythe;
	
	private bool m_escaping = false;
	private float warpDur = 0;
	
	protected override void Start ()
	{
		base.Start ();
		MAX_HEALTH = 1;//80;
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
		
		dyingDuration *= 3; 
		
		//temp
		spriteRenderer.color = new Color(1.0f, 0.5f, 0.5f, 1);
	}
	
	public override void ApplyHealthDamage (float value)
	{
		if(!m_awaking){
			return;
		}
		
		base.ApplyHealthDamage (value);
		
		if(current_health <= 0){
			KillPets();
			anim.SetBool("b_damaged", true);
			iTween.FadeTo(gameObject, 0, dyingDuration);
			GameObject.Find("GameManager").GetComponent<GameManager>().GameClear(2.0f);
		}else{
			if(cur_mode == MODE.NONE && Random.Range(0,3) > 0){
				m_escaping = true;
				Warp();
			}
		}
				
		if(cur_phase >= movePos.Length || current_status == STATUS.GONE){
			return;
		}
		
		float val = MAX_HEALTH - switchVal[cur_phase];
		
		if(m_moving <= 0.0f && current_health <= val){
			anim.SetTrigger("t_damaged");
			GoToNextPhase();
		}
	}
	/*
	private void OnGUI(){
		GUI.Box(new Rect(50, 50, 100, 100), cur_mode.ToString());
	}
	*/
	protected override void Update ()
	{
		
		base.Update ();
				
		if(current_health <= 0){
		CancelInvoke();
		}
		if(m_moving > 0.0f){
			invincible = true;
		}
		
		m_circleCollider.enabled = m_awaking;
		
		
		//Switching Attaack mode
		if(m_awaking){
//			Debug.Log(cur_mode);

			AnimUpdate();
			
			switch(cur_mode){
			case MODE.NONE :
				if(m_attackTimer <= 0.0f){
					//Choose attack skill
					int choice = cur_phase < 2 ? Random.Range(0,3) : Random.Range(0,4);
					if(choice == 0){
					
						int petCount = 0;
						for(int i = 0 ; i < transform.parent.transform.childCount ; i++){
							
							GameObject obj = transform.parent.transform.GetChild(i).gameObject;
							if(obj.tag.Equals("Enemy") && obj.gameObject != this.gameObject){
								petCount++;
							}
						}
						
						if(petCount < 8){
							cur_mode = MODE.PREP_SUMMON;
							
							m_attackTimer += 4.0f;
							
							Invoke("SummonPets", preparationTime);
						}else{
							break;
						}
						
					}else if(choice == 1){
						cur_mode = MODE.PREP_CUTTER;
						
						int row = 5;
						int line = cur_phase >= 2 ? 2 : 1;
						float timeInterval = 0.5f;
						
						m_attackTimer += timeInterval * (row * line) + 3.0f;
						
						Invoke("CutterRain", preparationTime);
					}else if(choice == 2){					
						m_attackTimer += 1.0f;
						
						//m_escaping = true;
						//Warp("SHOOT");
						cur_mode = MODE.PREP_SHOOT;
						Invoke("SwitchToShootMode", preparationTime);

					}else if(choice == 3){
						m_attackTimer += 4.0f;
						cur_mode = MODE.PREP_SYTHE;
						Invoke("SwingSythe", prepareTime_sythe);
					}
				}else{
					m_attackTimer -= Time.deltaTime;
					if(m_warpTimer <= 0.0f){
						Warp();
					}else{
						m_warpTimer -= Time.deltaTime;
					}
				}
				
				
				break;
			case MODE.SHOOT :{
				if(m_shootTimer <= 0.0f){
					
					m_shootTimer = SHOOT_INTERVAL;
					
					ShootToTarget();
					
					if(bulletCount >= maxBullet){
						m_attackTimer += 1.0f;
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
			case MODE.WARP_START:
				warpDur += Time.deltaTime;
					if(warpDur > 4.0f){
						warpDur = 0;
						ShowUp();
					}
				break;
			default:
				break;
			}
			

			
			
		}else{//Not Awaking
			if(PlayerIsInRange() && m_moving <= 0.0f){
//				Debug.Log("Awake");
				m_awaking = true;
			}
		}
		
		if(m_moving > 0.0f){
			m_moving -= Time.deltaTime;
		}
		
	}
	
	private void GoToNextPhase(){
		anim.SetTrigger("t_idle");
		cur_mode = MODE.NONE;
		
		///CancelInvoke();
		
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
		if (cur_phase == 3) {
			preparationTime *= 0.5f;
		
		}
	}
	
	//Swing a Sythe
	private void SwingSythe(){
		if(cur_mode != MODE.PREP_SYTHE || !anim.GetBool("b_chargeSwing")){
			cur_mode = MODE.NONE;
			return;
		}
		cur_mode = MODE.SYTHE;
		//anim.ResetTrigger("t_chargeSwing");
				
		//anim.SetTrigger("t_swing");
				
		Vector3 pos = transform.position;
		Vector3 center = new Vector3(current_side == SIDE.RIGHT ? 7.0f : -7.0f, 0.0f, -1.0f);
		
		AttackZone_sythe obj = Instantiate (sythe) as AttackZone_sythe;
		obj.ApplyParentAndExecute(this);
		
		obj.transform.position = new Vector3 (pos.x + center.x, pos.y + center.y, pos.z + center.z);
		sound.PlaySE("Attack2", 1.0f);
		
	}
	
	//Create Cutters
	private void CutterRain(){
		if(cur_mode != MODE.PREP_CUTTER || !anim.GetBool("b_chargeMagic")){
			cur_mode = MODE.NONE;
			return;
		}
		cur_mode = MODE.CUTTER;
		//anim.SetTrigger("t_magic");
		
		float interval = INTERVAL_BLOCKS;
		float timeInterval = 0.5f;
		int row = 5;
		int line = cur_phase >= 2 ? 2 : 1;
		float offsetToCenter = ( interval * ((row-1) * 0.5f) );
		float offsetX = 10.24f  * (cur_phase >= 2 ? (Random.Range(0,2) == 1 ? -1 : 1) : 1);
		float offsetY = 1.24f;
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
				WraithHead obj = Instantiate (wraithHead) as WraithHead;
				obj.transform.position = new Vector3(targetColsCenter.x + offsetX , targetBlockPos.y + ( (i * interval) - offsetToCenter ) + offsetY, pos.z);
				obj.transform.parent = transform.parent.transform;
				effect.transform.position = obj.transform.position + new Vector3(0,0,-1);
				effect.transform.parent = obj.transform.parent.transform;
				
				float delay = delayBase + (timeInterval * (reverse ? (row - i) : i) ) + ( (timeInterval * row) * j );
				obj.SetWait(delay);
				obj.Execute(offsetX > 0 ? SIDE.LEFT : SIDE.RIGHT);
			}
		}
		
	}
	
	private void ShootToTarget(){
		bulletCount++;
						
		Vector3 pos = transform.position;
		Vector3 myColsCenter = pos + new Vector3 (m_circleCollider.center.x, m_circleCollider.center.y, 0.0f);
		Vector3 targetPos = m_target.transform.position;
		Vector3 targetColsCenter = new Vector3( targetPos.x + (m_targetCols[0].center.x + m_targetCols[1].center.x)/2, targetPos.y + (m_targetCols[0].center.y + m_targetCols[1].center.y)/2, m_target.transform.position.z);
		
		Vector2 center;
		center.x = Random.Range (-1, 1);
		center.y = Random.Range (-0.1f, 0.1f);
		
		Vector3 dir = (targetColsCenter - myColsCenter).normalized;
		
		float rad = Mathf.Atan2(dir.y, dir.x);		
		float theta = rad * Mathf.Rad2Deg;
		
		float centerTheta =  Random.Range(-20.0f, 20.0f);
		
		float vx = Mathf.Cos(Mathf.PI / 180 * (theta + centerTheta ));
		float vy = Mathf.Sin(Mathf.PI / 180 * (theta + centerTheta ));
		
		GameObject effect = Instantiate(effect_pop2) as GameObject;
		effect.transform.localScale *= 2.5f;
		BossBullet obj= Instantiate (energyBall, new Vector3(myColsCenter.x + center.x, myColsCenter.y + center.y, pos.z), transform.rotation) as BossBullet;
		obj.transform.parent = transform.parent.transform;
		
		effect.transform.position = obj.transform.position + new Vector3(0,0,-1);
		effect.transform.parent = obj.transform.parent.transform;
		obj.SetAttackPower(attack_power * 0.5f);
		obj.SetDirectionAndExecute(new Vector2(vx, vy));

	}
	
	//Create some pets
	private void SummonPets(){
		if(cur_mode != MODE.PREP_SUMMON || !anim.GetBool("b_chargeMagic")){
			cur_mode = MODE.NONE;
			return;
		}
		cur_mode = MODE.SUMMON;
		anim.SetTrigger("t_magic");
		
		int num = cur_phase >= 2 ? 4 : 2;
		
		for(int i = 0 ; i < num ; i++){
			int petKey = Random.Range(cur_phase > 2 ? 2 : 0, cur_phase >= 2 ? pets.Length : pets.Length-1);
			
			float offsetY = 3.0f;
			float offsetX = i % 2 == 0 ? -6.0f : 6.0f;
			if(i >= 2){
				offsetY += 1.2f;
				offsetX *= 2.0f;
			}
			
			
			Vector3 summonPos = m_target.transform.position + new Vector3(offsetX, offsetY, 0);
			
			GameObject effect = Instantiate(effect_pop2) as GameObject;
			effect.transform.localScale *= 2.5f;
			
			GameObject obj = Instantiate (pets[petKey], summonPos, this.transform.rotation) as GameObject;
			obj.transform.parent = transform.parent.transform;
			effect.transform.position = obj.transform.position + new Vector3(0,0,-1);
			effect.transform.parent = obj.transform.parent.transform;
			//obj.SetAsOneShot(gameObject);
		}
		
	}
	
	private void KillPets(){
		int idx = transform.parent.transform.childCount-1;
		
		while(idx > -1){
			
			GameObject obj = transform.parent.transform.GetChild(idx).gameObject;
			if(obj.tag.Equals("Enemy") && obj.gameObject != this.gameObject){
				obj.transform.parent = null;
				obj.GetComponent<Enemy>().InstantDeath();
			}else if(obj.name.Contains("Setter") || obj.name.Contains("Wraith")){
				obj.transform.parent = null;
				Destroy(obj);
			}
			idx--;
			
		}
	}
	
	private Vector3 AnalizeBlockPos(Vector3 colPos){
		float blockPosX = 0;
		int dir = colPos.x > 0 ? 1 : -1;
		while(Mathf.Abs( blockPosX ) < Mathf.Abs( colPos.y )){
			blockPosX += INTERVAL_BLOCKS * dir;
		}
		float blockPosY = 0;
		dir = colPos.y > 0 ? 1 : -1;
		while(Mathf.Abs( blockPosY ) < Mathf.Abs( colPos.y )){
			blockPosY += INTERVAL_BLOCKS * dir;
		}
				
		return new Vector3(blockPosX, blockPosY, colPos.z);
	}
	
	private void Warp(string cmd){
	
		if(cur_mode == MODE.WARP_START){
			return;
		}else{
			cur_mode = MODE.WARP_START;
		}
		
		m_warpTimer = 4.0f;		
		
		m_circleCollider.enabled = false;
		float dur = 0.5f * Random.Range(1, 3);
		Invoke("ShowUp", dur);
		if(cmd == "SHOOT"){
			Invoke("ShootAfterWarp", dur + 0.5f);
		}
	}
	
	private void ShootAfterWarp(){
		cur_mode = MODE.PREP_SHOOT;
		Invoke("SwitchToShootMode", preparationTime);
	}
	
	private void Warp(){
		Warp("NONE");
	}
		
	private void ShowUp(){
	if (cur_mode != MODE.WARP_START  ||!anim.GetBool ("b_warpStart")) {
			return;
		}
		
		cur_mode = MODE.WARP_END;		
		m_circleCollider.enabled = true;
		
		//anim.SetTrigger("t_warpEnd");
		
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
		Invoke("ToDefault", 0.5f);
	}
	
	private void ToDefault(){
		cur_mode = MODE.NONE;
	}
	
	private void AnimUpdate(){
		anim.SetBool("b_warpStart", cur_mode == MODE.WARP_START ? true : false);
		anim.SetBool("b_warpEnd",cur_mode == MODE.WARP_END ? true : false);
		if(cur_mode == MODE.WARP_START || cur_mode == MODE.WARP_END){
			return;
		}
		anim.SetBool("b_chargeMagic", cur_mode == MODE.PREP_CUTTER || cur_mode == MODE.PREP_SHOOT || cur_mode == MODE.PREP_SUMMON ? true : false);
		anim.SetBool("b_chargeSwing", cur_mode == MODE.PREP_SYTHE ? true : false);
		anim.SetBool("b_magic",cur_mode == MODE.CUTTER || cur_mode == MODE.SHOOT || cur_mode == MODE.SUMMON ? true : false);
		anim.SetBool("b_swing",cur_mode == MODE.SYTHE ? true : false);
		
	}
	
	
	
	private void SwitchToShootMode(){
		if(cur_mode != MODE.PREP_SHOOT || !anim.GetBool("b_chargeMagic")){
			cur_mode = MODE.NONE;
			return;
		}
		//anim.SetTrigger("t_magic");
		
		bulletCount = 0;
		maxBullet = 4 + (2 * cur_phase);
		m_attackTimer += 1.0f;
		m_shootTimer = 0.0f;
		cur_mode = MODE.SHOOT;
	}
}
