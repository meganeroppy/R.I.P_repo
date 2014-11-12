using UnityEngine;
using System.Collections;

public class Generator : Monument {

	public GameObject child;
	protected bool m_isWorking;

	protected float GENERATE_INTERVAL = 1.5f;
	protected float generate_timer = 0.0f;
	protected float offset_range = 2.0f;

	protected override void Start(){
		base.Start ();
		m_isWorking = true;
	}
	
		// Update is called once per frame
	protected override void Update () {
		generate_timer += 1.0f * Time.deltaTime;
		if (generate_timer >= GENERATE_INTERVAL) {
			if(m_isWorking){
				Generate();
				generate_timer = 0.0f;
			}
			if(GameManager.GameOver()){

				m_isWorking = false;
			}
		}

	}

	protected override void ApplyHealthDamage (int value)
	{
		base.ApplyHealthDamage (value);

		//Generator is undead
		current_health += value;
	}

	protected virtual void Generate(){

		Vector2 offset;
		offset.x = Random.Range (-offset_range, offset_range);
		offset.y = Random.Range (-offset_range, offset_range);

		Vector3 pos = transform.position;
		 Instantiate (child, new Vector3(pos.x + offset.x, pos.y + offset.y, pos.z), transform.rotation);
	} 
}
