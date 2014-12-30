using UnityEngine;
using System.Collections;

public class EventCharacter : MonoBehaviour {

	protected float counter = 0.0f;
	protected bool excited = false;
	protected bool ghostFlug = false;
	protected int cur_phase = 0;
	protected Animator anim;
	public string[] emotions;
	protected float flipFlug = 0.0f;
	protected SoundManager sound;
	
	// Use this for initialization
	protected virtual void Start () {
		anim = GetComponent<Animator>();
		sound = transform.parent.GetComponent<SoundManager>();
	}
	
	// Use this for initialization
	protected virtual void Update () {
	}
	
	protected virtual void AdvancePhase(int phase){
		cur_phase = phase;
		if(cur_phase > emotions.Length){
			print("AnimSetEnd");
			return;
		}else{
			string[] str = emotions[phase-1].Split (',');
			excited = str[0].Equals("t_idle") ? true : false;
			
			if(!str[0].StartsWith("t")){
				SetMovement(str[0]);
			}else{
				anim.SetTrigger(str[0]);
			}
			
			if(str.Length > 1){
				if(str[1].Equals("g")){
					ghostFlug = true;
				}
			}
			if(str[0].Equals("t_attack")){
				sound.PlaySE("Attack");
			}else if(str[0].Equals("t_die")){
				iTween.ColorTo(gameObject, new Color(1,1,1,0), 2.0f);
				Destroy(this.gameObject, 2.0f);
			}
		}
	}
	
	protected virtual void SetMovement(string key){
		if(key.Equals("FlipR")){
			Flip(true);
		}else if(key.Equals("FadeIn")){
			iTween.ColorTo(gameObject, new Color(1,1,1,1), 3.5f);
		}else if(key.Equals("FadeOut")){
			iTween.ColorTo(gameObject, new Color(1,1,1,0), 1.5f);
			Destroy(this.gameObject, 1.5f);
		}else if(key.Equals("moveOutR")){
			Flip(true);
			iTween.MoveTo(gameObject, iTween.Hash("x", transform.position.x + 17.0f, "time", 5.0f, "easetype", iTween.EaseType.linear)); 
			Destroy(this.gameObject, 5.0f);
		}
	}
	
	protected virtual void Flip(){
		Vector3 scale = transform.localScale;
		float newScaleX = -transform.localScale.x;
		this.transform.localScale = new Vector3(newScaleX, scale.y, scale.z); 
	}
	
	protected virtual void Flip(bool right){
		Vector3 scale = transform.localScale;
		scale.x = scale.x < 0.0f ? scale.x : -scale.x;
		this.transform.localScale = new Vector3(scale.x, scale.y, scale.z); 
	}
	
	

}
