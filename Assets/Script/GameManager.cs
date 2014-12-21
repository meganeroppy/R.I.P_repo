using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	//Key Assign
	public enum BUTTON{
		UP = 0,
		DOWN,
		LEFT,
		RIGHT,
		DECIDE,
		CANCEL,
		START,
		SELECT,
	}

	//Title Selection
	public enum SELECTION_TITLE{
		WAITFORKEY,
		MAIN,
		TESTSTAGE1,
		TESTSTAGE2,
		OPTION,
		QUIT
	}
	[HideInInspector]
	public static SELECTION_TITLE current_selection_title;

	//Pause Selection
	public enum SELECTION_PAUSE{
		RESUME,
		RESTART,
		QUIT
	}
	[HideInInspector]
	public static SELECTION_PAUSE current_selection_pause;

	//Status
	private static bool pausing;
	private static bool cleared;
	private static bool gameover;
	private static bool inMissingDirection;
	static bool IsGhost = false;
	
	public static int player_life;
	private const int DEFAULT_LIFE = 3;
	public static int player_health;
	private bool playerIsBorn = false;
	private bool StageMakingHasBeenExecuted = false;
	
	//Scripts
	private static SoundManager soundManager;
	private static InputManager inputManager;
	private static GUIManager guiManager;
	private static MainCamera mainCamera;
	
	private Player player;
	
	private static Vector3	m_respawnPos;
	
	void Awake(){
		Application.targetFrameRate = 30;
//		if (SystemInfo.operatingSystem.Contains ("Vita")){}; 
	}
	
	// Use this for initialization
	void Start () {
		ReassignScripts();
		
		Pause(false);
		cleared = false;
		gameover = false;
		inMissingDirection = false;
		
		switch(Application.loadedLevelName.ToString()){
		case "Title":
			EnableUI();
			current_selection_title = SELECTION_TITLE.WAITFORKEY;
			break;//End of case Title
		case "Main":
			StageMakingHasBeenExecuted = true;
			goto case "Test02";
		case "Tutorial":
		case "Test01":
		case "Test02":
			player_life = DEFAULT_LIFE;
			current_selection_pause = SELECTION_PAUSE.RESUME;
			break;//End of case "Main"
		default:
			break;
		}
	}
	
	public void Update(){
		if(Application.loadedLevelName.ToString() != "Title"){
			if(Application.loadedLevelName.Contains("Test") && !StageMakingHasBeenExecuted){
				int stageIdx = Application.loadedLevelName == "Test01" ? 0 : 1 ;
				GameObject.FindWithTag("StageMaker").GetComponent<StageMaker>().SendMessage("Init", stageIdx);
				StageMakingHasBeenExecuted = true;
			}
		
			if(!playerIsBorn && player == null){
				player = GameObject.FindWithTag("Player").GetComponent<Player>();
				player.SendMessage("init", this.gameObject);
				playerIsBorn = true;
			}
		}
	}
	

	public static void Pause(bool key){
		if (key) {
			Time.timeScale = 0.0f;
			pausing = true;
		} else {
			Time.timeScale = 1.0f;	
			pausing = false;
		}
	}

	public static bool  Pause(){
		return pausing;
	}

	
	private IEnumerator  WaitAndExecute(float delay, string cmd){
		yield return new WaitForSeconds (delay);
		//Restart the same stage
		if(cmd == "Restart"){	
			Restart();
		}else if(cmd == "Title"){
			Application.LoadLevel("Title");
		}
	}
	
	private void Restart(){
		GameObject obj = GameObject.FindWithTag("Player");
		obj.GetComponent<Player>().enabled = true;
		obj.SendMessage("Restart", m_respawnPos);
		inMissingDirection = false;
		Pause(false);
	}
	
	public static void GameStart(string levelName){
		GameObject[] obj = GameObject.FindGameObjectsWithTag("Loading");
		foreach(GameObject child in obj){
			child.SendMessage("Activate");
		}
		Application.LoadLevel(levelName);
	}
	
	public void EnableUI(){
		ReassignScripts();
		soundManager.enabled = true;
		inputManager.enabled = true;
		//guiManager.enabled = true;

		GameObject[] obj = GameObject.FindGameObjectsWithTag("UI");
		foreach(GameObject child in obj){
			child.SendMessage("Activate");
		}
	
	}
	//For Title screen
	public static string GetTitleStatus(){
		switch(current_selection_title){
		case SELECTION_TITLE.WAITFORKEY:
			return "WAITFORKEY";
		case SELECTION_TITLE.MAIN:
			return "MAIN";
		case SELECTION_TITLE.TESTSTAGE1:
			return "TESTSTAGE1";
		case SELECTION_TITLE.TESTSTAGE2:
			return "TESTSTAGE2";
		case SELECTION_TITLE.OPTION:
			return "OPTION";
		default:
			return "ETC";
		}
	}
	
	public static int GetPauseStatus(){
		return (int)current_selection_pause;
	/*
		switch(current_selection_pause){
		case SELECTION_PAUSE.RESUME:
			return "RESUME";
		case SELECTION_PAUSE.RESTART:
			return "RESTART";
		case SELECTION_PAUSE.QUIT:
			return "QUIT";
		default:
			return "ETC";
		}
		*/
	}
	
	public static void AcceptInput(string situation, BUTTON btn){
		if(situation == "Title"){
			if(btn == BUTTON.DECIDE){
				PressDecisionKey(situation);
			}else if(btn == BUTTON.RIGHT){
				PressSelectKey(situation, true);
			}else if(btn == BUTTON.LEFT){
				PressSelectKey(situation, false);
			}
		}else if(situation == "Pause"){
			if(btn == BUTTON.DECIDE){
				PressDecisionKey(situation);
			}else if(btn == BUTTON.DOWN){
				PressSelectKey(situation, true);
			}else if(btn == BUTTON.UP){
				PressSelectKey(situation, false);
			}
		}
	}
	
	public static void PressDecisionKey(string situation){
	
		if(situation == "Title"){
		 switch(current_selection_title){
			case SELECTION_TITLE.WAITFORKEY:
				current_selection_title = SELECTION_TITLE.MAIN;
				return;
			case SELECTION_TITLE.MAIN:
				GameStart("Main");
				return;
			case SELECTION_TITLE.TESTSTAGE1:
				GameStart("Test01");
				return;
			case SELECTION_TITLE.TESTSTAGE2:
				GameStart("Test02");
				return;
			case SELECTION_TITLE.OPTION:
				return;
			default:
				return;
			}
		}else if(situation == "Pause"){
			switch(current_selection_pause){
			case SELECTION_PAUSE.RESUME:
				Pause(false);
				return;
			case SELECTION_PAUSE.RESTART :
				Pause(false);
				GameStart(Application.loadedLevelName);
				return;
			case SELECTION_PAUSE.QUIT:
				GameStart("Title");
				return;
			default:
				return;
			}
		}
	}
	
	public static void PressSelectKey(string situation, bool dir){
		if(situation == "Title"){
			switch(current_selection_title){
			case SELECTION_TITLE.WAITFORKEY:
				current_selection_title = SELECTION_TITLE.MAIN;			
				return;
			case SELECTION_TITLE.MAIN:
				current_selection_title = dir ? SELECTION_TITLE.TESTSTAGE1 : SELECTION_TITLE.OPTION;			
				return;
			case SELECTION_TITLE.TESTSTAGE1:
				current_selection_title = dir ? SELECTION_TITLE.TESTSTAGE2 : SELECTION_TITLE.MAIN;			
				return;
			case SELECTION_TITLE.TESTSTAGE2:
				current_selection_title = dir ? SELECTION_TITLE.OPTION : SELECTION_TITLE.TESTSTAGE1;			
					return;
			case SELECTION_TITLE.OPTION:
				current_selection_title = dir ? SELECTION_TITLE.MAIN : SELECTION_TITLE.TESTSTAGE2;			
				return;
			default:
				return;
			}
		}else if ( situation == "Pause"){
			switch(current_selection_pause){
			case SELECTION_PAUSE.RESUME:
				current_selection_pause = dir ? SELECTION_PAUSE.RESTART : SELECTION_PAUSE.QUIT;			
				return;
			case SELECTION_PAUSE.RESTART:
				current_selection_pause = dir ? SELECTION_PAUSE.QUIT : SELECTION_PAUSE.RESUME;			
				return;
			case SELECTION_PAUSE.QUIT:
				current_selection_pause = dir ? SELECTION_PAUSE.RESUME : SELECTION_PAUSE.RESTART;			
				return;

			default:
				return;
			}
		}
	}
	
	
	//Getter & Setter///////////////////////////
	
	public static bool GameOver(){
		return gameover;
	}
	
	public static bool Miss(){
		return inMissingDirection;
	}
	
	private void GameOver(bool key){
		gameover = true;
	}


	public void Miss(bool key){
		ReassignScripts();
		mainCamera.SendMessage("ReleaseTarget");
		inMissingDirection = true;
		
		GetComponent<InputManager> ().enabled = false;
		
		if(player_life > 0){
			player_life--;
			StartCoroutine (WaitAndExecute(2.0f, "Restart"));
		}else{
			StartCoroutine (WaitAndExecute(4.0f, "Title"));
			gameover = true;
		}
	}
	
	public static bool GameClear(){
		return cleared;
	}
	
	public void GameClear(bool key){
		cleared = true;
		StartCoroutine (WaitAndExecute (4.0f, "Title"));
	}
	
	private void ReassignScripts(){

		if(soundManager == null){
			soundManager = GetComponent<SoundManager>();
		}
		if(inputManager == null){
			inputManager = GetComponent<InputManager>();
		}
		if(guiManager == null){
			guiManager = GetComponent<GUIManager>();
		}
		if(guiManager == null){
			guiManager = GetComponent<GUIManager>();
		}
		if( mainCamera == null){
			mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<MainCamera>();
		}
	}
	
	private void ApplyRespawnPoint(Vector3 newPos){
		m_respawnPos = newPos;
//		Debug.Log("ApplyNewRespawnPos : " + m_respawnPos.ToString());
	}
	
	public static Vector3 GetCurrentRespawnPosition(){
		return m_respawnPos;
	}
	
	public static bool CheckCurrentPlayerIsGhost(){
		return IsGhost;
	}
	public static void InformBecomeGhost(bool ghost){
		IsGhost = ghost;
	}
}
