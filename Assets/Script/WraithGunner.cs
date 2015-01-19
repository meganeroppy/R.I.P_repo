using UnityEngine;
using System.Collections;

public class WraithGunner : Wraith {

	public GameObject WraithShot;
	protected float eulerZ_shotTexture = -90.0f;
	private const float SHOT_INTERVAL = 0.25f;
	private float m_shotTimer = 0.0f;
	private int shotCount = 0;
	private CircleCollider2D m_col;
	private const float MAX_SHOOT_INTERVAL = 35.0f; 
	private float current_shootAngle = 0;
	
	protected override void Start ()
	{
		base.Start ();
		m_col = GetComponent<CircleCollider2D>();
		AWAKE_RANGE = 15.4f;
	}
	
	protected override void Update ()
	{
		base.Update ();
		
		if (!GameManager.GameOver()){
			if(m_target == null){
				m_target = GameObject.FindWithTag ("Player").GetComponent<Player> ();
			}else if(m_targetCols == null){
				m_targetCols = m_target.GetComponents<CircleCollider2D>(); 
			}
		}
		
		if(readyToAct){
			if(m_shotTimer <= 0.0f){
				ShotToTarget();
				shotCount++;
				//m_shotTimer = shotCount % 9 == 0 ? SHOT_INTERVAL * 6.0f : shotCount % 3 == 0 ? SHOT_INTERVAL * 3.0f : SHOT_INTERVAL;
				m_shotTimer = SHOT_INTERVAL;				
			}else{
				m_shotTimer -= Time.deltaTime;
			}
		}
	}
	
	private void ShotToTarget(){
		//anim.SetTrigger("t_attack");
		
		Vector3 pos = transform.position;
		Vector3 myColsCenter = pos + new Vector3 (m_col.center.x, m_col.center.y, 0.0f);
		Vector3 targetPos = m_target.transform.position;
		Vector3 targetColsCenter = new Vector3( targetPos.x + (m_targetCols[0].center.x + m_targetCols[1].center.x)/2, targetPos.y + (m_targetCols[0].center.y + m_targetCols[1].center.y)/2, m_target.transform.position.z);
		
		int dir = current_side == SIDE.RIGHT ? 1 : -1;
		Vector2 offset = new Vector2 (2.0f * dir, -1.8f);
		
		GameObject obj =  Instantiate(WraithShot, new Vector3(myColsCenter.x + offset.x, myColsCenter.y + offset.y, myColsCenter.z), transform.rotation) as GameObject;
		
		//Move Direction
		Vector3 baseDir = (targetColsCenter - obj.transform.position).normalized;
		float radian = Mathf.Atan2(baseDir.y, baseDir.x);
		float baseAngle = radian * Mathf.Rad2Deg;
		current_shootAngle = current_shootAngle == MAX_SHOOT_INTERVAL ? -MAX_SHOOT_INTERVAL : current_shootAngle + (MAX_SHOOT_INTERVAL * 2 * 0.25f); 
		float offsetAngle = Random.Range(-MAX_SHOOT_INTERVAL, MAX_SHOOT_INTERVAL);
		//float offsetAngle = current_shootAngle;
		float fixedAngle = baseAngle + offsetAngle;
		
		Vector3 fixedDir = new Vector3(Mathf.Cos(Mathf.PI / 180 * fixedAngle), Mathf.Sin(Mathf.PI / 180 * fixedAngle), 0.0f).normalized; 
		
		obj.SendMessage("SetAttackPower", attack_power * 0.5f);
		obj.SendMessage("SetDirectionAndExecute", fixedDir);
		
		//Rotation
		radian = Mathf.Atan2(fixedDir.x, fixedDir.y);
		float degree = radian * Mathf.Rad2Deg;
		obj.transform.rotation = Quaternion.Euler(0.0f, 0.0f, -degree + eulerZ_shotTexture);
	}
}
