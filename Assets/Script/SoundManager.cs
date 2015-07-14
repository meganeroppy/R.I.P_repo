using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {
	public AudioClip bgm_op;
	public AudioClip bgm_tutorial;
	public AudioClip bgm_ed;
	public AudioClip bgm_stage;
	public AudioClip bgm_boss;
	
	public AudioClip se_goal;
	public AudioClip se_jump;
	public AudioClip se_attack;
	public AudioClip se_attack2;
	public AudioClip se_damage;
	public AudioClip se_damage_electro;
	public AudioClip se_getItem;
	
	public AudioClip se_raith_wakeup;
	public AudioClip se_bubble_throw;
	public AudioClip se_bubble_die;	
	
	void Start(){	
		switch(SceneManager.currentLevelName){
		case "Main":
		case "Tutorial":
			GetComponent<AudioSource>().clip = bgm_tutorial;
			break;
		case "Stage01":
		case "Stage02":
			
			GetComponent<AudioSource>().clip = bgm_stage;
			
			//SetBGM(System.DateTime.Now.Hour);

			break;
		case "StageBoss":
			GetComponent<AudioSource>().clip = bgm_boss;
			
			break;
			case "Title":
			return;
			
			default:
			break;
		}
		GetComponent<AudioSource>().loop = true;
		GetComponent<AudioSource>().Play();
	}
	
	// Update is called once per frame
	void Update () {
	}
	
	public void PlaySE(string clip){
		PlaySE(clip, 1.0f);
	}

	public void PlaySE(string clip, float volume){
		switch (clip) {
		case "Jump":
			GetComponent<AudioSource>().PlayOneShot(se_jump, volume);
			break;
		case "Attack":
			GetComponent<AudioSource>().PlayOneShot(se_attack, volume);
			break;
		case "Attack2":
			GetComponent<AudioSource>().PlayOneShot(se_attack2, volume);
			break;
		case "Damage":
			GetComponent<AudioSource>().PlayOneShot(se_damage, volume);
			break;
		case "Damage_electro":
			GetComponent<AudioSource>().PlayOneShot(se_damage_electro, volume);
			break;
		case "GetItem" :
			GetComponent<AudioSource>().PlayOneShot(se_getItem, volume);
			break;
		case "Wraith_wakeup" :
			GetComponent<AudioSource>().PlayOneShot(se_raith_wakeup, volume);
			break;
		case "Bubble_throw" :
			GetComponent<AudioSource>().PlayOneShot(se_bubble_throw, volume);
			break;
		case "Bubble_die" :
			GetComponent<AudioSource>().PlayOneShot(se_bubble_die, volume);
			break;
		default:
			break;
		}
	}

	public void SetBGM(int hour){
		switch(hour){
		case 0:
		case 3:
		case 6:
		case 9:
		case 12:
		case 15:
		case 18:
		case 21:
			GetComponent<AudioSource>().clip = bgm_op;
			break;
		case 1:
		case 4:
		case 7:
		case 10:
		case 13:
		case 16:
		case 19:
		case 22:
			GetComponent<AudioSource>().clip = bgm_tutorial;
			break;
		case 2:
		case 5:
		case 8:
		case 11:
		case 14:
		case 17:
		case 20:
		case 23:
			GetComponent<AudioSource>().clip = bgm_ed;
			break;
		}
	}
	
	public void FadeoutBGM(){
		iTween.ValueTo (gameObject, iTween.Hash ("from", 0.5f, "to", 0, "time", 3.0f, "onupdate", "SetSoundVolume"));
	}
	
	public void SetSoundVolume(float val){
		GetComponent<AudioSource>().volume = val;
	}
}
