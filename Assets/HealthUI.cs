using UnityEngine;
using System.Collections;

public class HealthUI : UI {

	private Player m_target;
	public GameObject iconPrefab; 
	
	protected override void Start(){
	
		base.Start();
		uiSprite.enabled = false;
		
	}
	
	// Update is called once per frame
	protected override void Update () {
		if(!activated){
			return;
		}
	}
	
	protected override void Activate(){
	
		m_target = GameObject.FindWithTag("Player").GetComponent<Player>();
		
		int max_health = m_target.GetLifeInfo()[0];
		GameObject[] icon = new GameObject[max_health];
		float interval = 1.5f;
		for(int i = 0 ; i < icon.Length ; i++){
			icon[i] = this.gameObject;
			//icon[i] = Instantiate(iconPrefab, this.transform.position + new Vector3(i * interval, 0,0), this.transform.rotation) as GameObject;
			icon[i].transform.parent = this.gameObject.transform;
			
		}
		
	activated = true;
	}
}
