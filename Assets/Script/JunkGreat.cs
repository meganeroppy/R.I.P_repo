using UnityEngine;
using System.Collections;

public class JunkGreat : Junk {

	protected override void ApplyHealthDamage (int val)
	{
		if(current_health <= 0.0f){
			return;
		}
	
		base.ApplyHealthDamage (val);
		if(current_health <= 0.0f){
			Explode();
		}		
		
	}


	private void Explode(){
		Vector3 pos = transform.position;
		//Vector3 targetPos = m_target.transform.position;
		//Vector3 targetColsCenter = new Vector3( targetPos.x + (m_targetCols[0].center.x + m_targetCols[1].center.x)/2, targetPos.y + (m_targetCols[0].center.y + m_targetCols[1].center.y)/2, m_target.transform.position.z);
		
		float offsetY = 1.0f;
		
		int numOfBullet = 8;
		float offsetAngle = Random.Range(-4.0f, 4.0f);
		
		for(int i = 0 ; i < numOfBullet ; i++){
			
			GameObject obj =  Instantiate(bubble, new Vector3(pos.x, pos.y + offsetY, pos.z), transform.rotation) as GameObject;
			
			//Move Direction
			//Vector3 baseDir = (targetColsCenter - obj.transform.position).normalized;
			//float radian = Mathf.Atan2(baseDir.y, baseDir.x);
			//float baseAngle = radian * Mathf.Rad2Deg;
			float baseAngle = 90;
			float fixedAngle = baseAngle + ( (360 / numOfBullet) * i ) + offsetAngle;
			
			Vector3 fixedDir = new Vector3(Mathf.Cos(Mathf.PI / 180 * fixedAngle), Mathf.Sin(Mathf.PI / 180 * fixedAngle), 0.0f).normalized; 
			obj.SendMessage("SetAttackPower", attack_power);
			
			obj.SendMessage("SetDirectionAndExecute", fixedDir);
			//obj.rigidbody2D.AddForce(new Vector2 (baseDir.x * speed, baseDir.y * speed));
			
			//Rotation
			float radian = Mathf.Atan2(fixedDir.x, fixedDir.y);
			float degree = radian * Mathf.Rad2Deg;
			obj.transform.rotation = Quaternion.Euler(0.0f, 0.0f, -degree + eulerZ_bubbleTexture);
		}
	}

/*
	protected float offsetEularZ = -90.0f;
	private Vector3 dir;
	private float degree;
	private GameObject target;
	// Use this for initialization
	void Start () {
		transform.rotation = Quaternion.Euler(0.0f, 0.0f, 45.0f);
	}
	
	// Update is called once per frame
	void Update () {
		if(target == null){
			target = GameObject.FindWithTag("Player");
		}
		dir = (target.transform.position - transform.position).normalized;//make the unit vector 
		float radian = Mathf.Atan2(dir.x, dir.y);
		degree = radian * Mathf.Rad2Deg;//Radian To Eular Degree
		transform.rotation = Quaternion.Euler(0.0f, 0.0f, -degree + offsetEularZ);
		
	}
	*/
}

