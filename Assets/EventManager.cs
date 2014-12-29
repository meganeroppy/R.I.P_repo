using UnityEngine;
using System.Collections;

public class EventManager : MonoBehaviour
{

				
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
		private AudioSource audio;
	
		private void Start (){
		
		audio = GetComponent<AudioSource>();
		
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
		blackScreen = GameObject.FindWithTag ("Loading").GetComponent<UISprite> ();
		
		StartEvent();
	}
	
	private void Update ()
	{
		if (Input.GetKeyDown (KeyCode.S)) {
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
			if(Time.frameCount % 20.0f == 0.0f ){
				label_pressButton.color = label_pressButton.color == Color.white ? Color.yellow : Color.white;
			}
		}
		
		if (Input.GetKeyDown (KeyCode.Space)) {
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
		EndEvent ();
	}
	
	private void StartEvent ()
	{
		iTween.ValueTo (gameObject, iTween.Hash ("from", 1, "to", 0, "time", 3.5f, "onupdate", "UpdateBlackScreenAlpha", "oncomplete", "EnableInput"));
	}
		
	private void EndEvent ()
	{
		ended = true;
		
		iTween.ValueTo (gameObject, iTween.Hash ("from", blackScreen.alpha, "to", 1, "time", 4.5f, "onupdate", "UpdateBlackScreenAlpha", "oncomplete", "LoadNextLevel"));	
	}
	
	private void LoadNextLevel ()
	{
		Application.LoadLevel ("Test02");
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
		audio.volume = val;
	}
	
}
