using UnityEngine;
using System.Collections;

public class StreetLight : Monument {
	
	public Sprite[] pic;
	protected int cur_frame = 0;
	protected float timeToTurnOn = 0.0f;
	protected bool m_awake = true;
	
	protected override void Start(){
		base.Start();
		m_awake = true;
	}
	
	protected override void Update(){
		
		if(GameManager.GameOver() || GameManager.Miss() || GameManager.Pause()){
			return;
		}

		if(m_awake){
		
			if(Random.Range(0.0f, 100.0f) < 1.0f && timeToTurnOn < 0.0f){
				cur_frame = 0;
				spriteRenderer.sprite = pic[cur_frame];
				timeToTurnOn = Random.Range(0.05f, 0.3f);
			}
			
			if(timeToTurnOn >= 0.0f){
				timeToTurnOn -= Time.deltaTime;
				if(timeToTurnOn < 0.0f){
					cur_frame = 1;
					spriteRenderer.sprite = pic[cur_frame];
				}
			}
			
		}else{//Not Awake
			if(cur_frame == 1){
				cur_frame = 0;
				spriteRenderer.sprite = pic[cur_frame];
			}
		}
		
	}

}
