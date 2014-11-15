using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	//Key Assign
	public enum BUTTON{
		UP = 0,
		DOWN,
		LEFT,
		RIGHT,
		ROUND,
		CROSS,
		SQUARE,
		TRIANGLE,
		T_LEFT,
		T_RIGHT,
		START,
		SELECT,
	}


	//Title Selection
	public enum SELECTION_TITLE{
		WAITFORKEY,
		MAIN,
		TUTORIAL,
		TESTSTAGE,
		OPTION,
		QUIT
	}
	[HideInInspector]
	public static SELECTION_TITLE current_selection_title;

	//Status
	static bool pausing;
	private static bool cleared;
	private static bool gameover;
	private static bool inMissingDirection;
	
	public static int player_life;
	private const int DEFAULT_LIFE = 1;
	private bool playerIsBorn = false;
	
	//Scripts
	private static SpiritBar spiritBar;
	private static SoundManager soundManager;
	private static InputManager inputManager;
	private static GUIManager guiManager;
	private static MainCamera mainCamera;
	
	private Player player;
	
	void Awake(){
		Debug.Log("Awake()");
		Application.targetFrameRate = 30;
	}
	
	// Use this for initialization
	void Start () {
		ReassignScripts();
		
		pausing = false;
		cleared = false;
		gameover = false;
		inMissingDirection = false;
		
		switch(Application.loadedLevelName.ToString()){
			case "Title":

				current_selection_title = SELECTION_TITLE.WAITFORKEY;
				spiritBar.enabled = false;
				break;//End of case Title
			case "Main":
			case "Tutorial":
			case "Test01":
			player_life = DEFAULT_LIFE;
			
			//spiritBar.enabled = false;
			//inputManager.enabled = false;
			//guiManager.enabled = false;
			

				break;//End of case "Main"
			default:
				break;
		}
	}
	
	public void Update(){
		if(Application.loadedLevelName.ToString() != "Title"){
			if(!playerIsBorn && player == null){
				player = GameObject.FindWithTag("Player").GetComponent<Player>();
				player.SendMessage("init", this.gameObject);
				playerIsBorn = true;
			}
		}
	}
	

	public static void  Pause(){
		if (!pausing) {
			Time.timeScale = 0.0f;
			pausing = true;
		} else {
			Time.timeScale = 1.0f;	
			pausing = false;
		}
		Debug.Log(player_life);
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
		obj.SendMessage("Restart");
		inMissingDirection = false;
	}

	public static void GameStart(string levelName){
		Application.LoadLevel(levelName);
	}
	
	public void EnableUI(){
		ReassignScripts();
		
		spiritBar.enabled = true;
		soundManager.enabled = true;
		inputManager.enabled = true;
		guiManager.enabled = true;
	
	}
	//For Title screen
	public static string GetTitleStatus(){
		switch(current_selection_title){
		case SELECTION_TITLE.WAITFORKEY:
			return "WAITFORKEY";
		case SELECTION_TITLE.MAIN:
			return "MAIN";
		case SELECTION_TITLE.TUTORIAL:
			return "TUTORIAL";
		case SELECTION_TITLE.TESTSTAGE:
			return "TESTSTAGE";
		case SELECTION_TITLE.OPTION:
			return "OPTION";
		default:
			return "ETC";
		}
	}
	
	public static void AcceptInput(BUTTON btn){
		if(Application.loadedLevelName == "Title"){
			if(btn == BUTTON.START){
				PressDecisionKey();
			}else if(btn == BUTTON.RIGHT){
				PressSelectKey(true);
			}else{
				PressSelectKey(false);
			}
		}
	}
	
	public static void PressDecisionKey(){
		switch(current_selection_title){
		case SELECTION_TITLE.WAITFORKEY:
			current_selection_title = SELECTION_TITLE.MAIN;
			return;
		case SELECTION_TITLE.MAIN:
			GameStart("Main");
			return;
		case SELECTION_TITLE.TUTORIAL:
			GameStart("Tutorial");
			return;
		case SELECTION_TITLE.TESTSTAGE:
			GameStart("Test01");
			return;
		case SELECTION_TITLE.OPTION:
			return;
		default:
			return;
		}
	}
	
	public static void PressSelectKey(bool dir){
		switch(current_selection_title){
		case SELECTION_TITLE.WAITFORKEY:
			current_selection_title = SELECTION_TITLE.MAIN;			
			return;
		case SELECTION_TITLE.MAIN:
			current_selection_title = dir ? SELECTION_TITLE.TUTORIAL : SELECTION_TITLE.OPTION;			
			return;
		case SELECTION_TITLE.TUTORIAL:
			current_selection_title = dir ? SELECTION_TITLE.TESTSTAGE : SELECTION_TITLE.MAIN;			
			return;
		case SELECTION_TITLE.TESTSTAGE:
			current_selection_title = dir ? SELECTION_TITLE.OPTION : SELECTION_TITLE.TUTORIAL;			
				return;
		case SELECTION_TITLE.OPTION:
			current_selection_title = dir ? SELECTION_TITLE.MAIN : SELECTION_TITLE.TESTSTAGE;			
			return;
		default:
			return;
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
			StartCoroutine (WaitAndExecute(2.0f, "Title"));
			gameover = true;
		}
	}
	
	public static bool GameClear(){
		return cleared;
	}
	
	public void GameClear(bool key){
		cleared = true;
		StartCoroutine (WaitAndExecute (2.0f, "Title"));
	}
	
	private void ReassignScripts(){
		if(spiritBar == null){
			spiritBar = GetComponent<SpiritBar>();
		}
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
}
