using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {

	public AudioClip se_goal;
	public AudioClip se_jump;
	public AudioClip se_attack;
	public AudioClip se_damage;
	public AudioClip se_getItem;

	public AudioClip bgm_asa;
	public AudioClip bgm_hiru;
	public AudioClip bgm_yoru;
	
	// Use this for initialization
	void Start () {
		switch(Application.loadedLevelName){
			case "Main":
		case "Tutorial":
		case "Test01":
			
				SetBGM(System.DateTime.Now.Hour);
				audio.Play();
			break;
			case "Title":
			break;
			default:
			break;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void PlaySE(string clip, float volume){
		switch (clip) {
		case "Jump":
			audio.PlayOneShot(se_jump, volume);
			break;
		case "Attack":
			audio.PlayOneShot(se_attack, volume);
			break;
		case "Damage":
			audio.PlayOneShot(se_damage, volume);
			break;
		case "GetItem" :
			audio.PlayOneShot(se_getItem, volume);
			break;
		default:
			break;
		}
	}

	private void SetBGM(int hour){
		switch(hour){
		case 0:
		case 3:
		case 6:
		case 9:
		case 12:
		case 15:
		case 18:
		case 21:
			audio.clip = bgm_asa;
			break;
		case 1:
		case 4:
		case 7:
		case 10:
		case 13:
		case 16:
		case 19:
		case 22:
			audio.clip = bgm_hiru;
			break;
		case 2:
		case 5:
		case 8:
		case 11:
		case 14:
		case 17:
		case 20:
		case 23:
			audio.clip = bgm_yoru;
			break;
		}
	} 
}
