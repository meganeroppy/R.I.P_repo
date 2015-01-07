using UnityEngine;
using System.Collections;

/*
× ボタン	KeyCode.JoystickButton0 / Y 
○ ボタン	KeyCode.JoystickButton1 / C
□ ボタン	KeyCode.JoystickButton2 / Z
△ ボタン	KeyCode.JoystickButton3 / V
L ボタン	KeyCode.JoystickButton4 / B
R ボタン	KeyCode.JoystickButton5 / N
SELECT ボタン	KeyCode.JoystickButton6 / Space
START ボタン	KeyCode.JoystickButton7 / Enter
方向キー上	KeyCode.JoystickButton8 
方向キー右	KeyCode.JoystickButton9
方向キー下	KeyCode.JoystickButton10
方向キー左	KeyCode.JoystickButton11
*/

public class EventManager : MonoBehaviour
{
	private float counter = 0.0f;
		private int cur_phase = 0;
		private const int numOfPhase = 8;
		private bool ended = false;
		public GameObject peace;
		public GameObject boss;
		public GameObject[] cats;
		private float waiting = 0.0f;
		private int[] wait = {1,1,2,1,2,2,5};
		private UILabel label_pressButton;
		private UISprite blackScreen;
		private AudioSource m_audio;
		private float fadeOutSpeed = 4.5f;

	private void Awake(){
		Application.targetFrameRate = 30;
	}

		private void Start (){
		
		m_audio = GetComponent<AudioSource>();
		
		GameObject obj;
		Vector3 pos = transform.position;
		obj = Instantiate (peace, new Vector3 (pos.x + -5.0f, pos.y + -2.0f, pos.z), transform.rotation) as GameObject;
		obj.transform.parent = transform;
		
		Vector3 peacePos = obj.transform.position;
		
		obj = Instantiate (boss, new Vector3 (pos.x + 6.0f, pos.y + 2.0f, pos.z), transform.rotation) as GameObject;
		obj.transform.localScale = new Vector2 (1.2f, 1.2f);
		obj.transform.parent = transform;
		
		
		for (int i = 0; i < cats.Length; i++) {
			obj = Instantiate (cats [i], new Vector3 (peacePos.x + Random.Range (-4.0f, 4.0f), peacePos.y + Random.Range (-2.5f, 2.5f), peacePos.z), transform.rotation) as GameObject;
			obj.transform.localScale = new Vector2 (0.7f, 0.7f);
			obj.transform.parent = transform;
		}
		
		//About UI
		label_pressButton = GameObject.FindWithTag ("UI").GetComponent<UILabel> ();
		blackScreen = GameObject.FindWithTag ("BlackScreen").GetComponent<UISprite> ();
		blackScreen.enabled = true;

		StartEvent();
	}
	
	private void Update ()
	{
		if (Input.GetKeyDown (KeyCode.S) || Input.GetKeyDown(KeyCode.JoystickButton6)) {
			SkipEvent ();	
		}

		if ( ended ) {
			label_pressButton.alpha = 0.0f;
		}else if (waiting > 0.0f) {
			waiting -= Time.deltaTime;
			label_pressButton.color = Color.gray;
			label_pressButton.alpha = 0.5f;
			return;
		} else {
			label_pressButton.alpha = 1.0f;
			if(  counter > 0.75f ){
				counter = 0.0f;
				label_pressButton.color = label_pressButton.color == Color.white ? Color.yellow : Color.white;
			}else{
				counter += Time.deltaTime;
			}
		}
		
		if (Input.GetKeyDown (KeyCode.Space) || Input.GetKeyDown(KeyCode.JoystickButton0) ||  Input.GetKeyDown(KeyCode.JoystickButton1) ||Input.GetKeyDown(KeyCode.JoystickButton2) ||Input.GetKeyDown(KeyCode.JoystickButton3)) {
			AdvanceEvent ();
		}
	}
		
	private void AdvanceEvent ()
	{
		cur_phase++;
		if (cur_phase == numOfPhase-1) {
			EndEvent ();
		}
		
		
		for (int i = 0; i < this.transform.childCount; i++) {
			GameObject obj = transform.GetChild (i).gameObject;
			obj.SendMessage ("AdvancePhase", cur_phase);
		}
		
		if(cur_phase == 4){
			iTween.ValueTo (gameObject, iTween.Hash ("from", 0.5f, "to", 0, "time", 3.0f, "onupdate", "UpdateSoundVolume"));
		}
		
		waiting = wait [cur_phase - 1];
	}
	
	private void SkipEvent ()
	{
		fadeOutSpeed = 2.0f;
		EndEvent ();
	}
	
	private void StartEvent ()
	{
		iTween.ValueTo (gameObject, iTween.Hash ("from", 1, "to", 0, "time", 3.5f, "onupdate", "UpdateBlackScreenAlpha", "oncomplete", "EnableInput"));
	}
		
	private void EndEvent ()
	{
		ended = true;
		
		iTween.ValueTo (gameObject, iTween.Hash ("from", blackScreen.alpha, "to", 1, "time", fadeOutSpeed, "onupdate", "UpdateBlackScreenAlpha", "oncomplete", "LoadNextLevel"));	
	}
	
	private void LoadNextLevel (){
	
		GameObject.FindWithTag("Loading").SendMessage("Activate");
		Application.LoadLevel ("Tutorial");
	}
	
	private void UpdateBlackScreenAlpha (float val)
	{
		blackScreen.alpha = val;
	}
	
	private void EnableInput ()
	{
		waiting = 0.0f;
	}
	
	private void UpdateSoundVolume(float val){
		m_audio.volume = val;
	}
	
}
