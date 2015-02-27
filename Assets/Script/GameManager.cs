using UnityEngine;
using System.Collections;
using System.Collections.Generic;

 public class GameManager : MonoBehaviour {

	private string[] scenes = new string[]{
		"Tutorial",	
		"Stage01",
		"Stage02",
		"StageBoss",
		"Event02",
		"Title",
		"Event01",	
	};

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
		EVENT1,
		STAGE1,
		STAGE2,
		BOSS,
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
	private static bool openingDirectionIsCompleted; 
	public static float DEFAULT_PIECE_SCALE = 5.12f;
	public static float PIECE_SCALE = 3.20f;
		
	public static bool IsGhost = false;
	public static bool IsHidden = false;
	
	public static int player_life;
	private const int DEFAULT_LIFE = 9;
	//public static int player_health;
	public static bool playerIsBorn = false;
	private bool StageMakingHasBeenExecuted = false;
	private int m_sceneIdx = 0;
	private int numOfTreasure = 0;
	private bool[] treasureIsObtained = new bool[5];
	private Player player;
	
	private static Vector3	m_respawnPos;

	//System
	//Scripts
	private static SoundManager soundManager;
	private static InputManager inputManager;
	private static GUIManager guiManager;
	private static MainCamera mainCamera;
	private float saveTimer = 0;
	private const float SAVE_INTERVAL = 5f;

	
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
		playerIsBorn = false;
		openingDirectionIsCompleted = false;
		
		switch(Application.loadedLevelName.ToString()){
		case "Title":
			EnableUI();
			current_selection_title = SELECTION_TITLE.WAITFORKEY;
			break;//End of case Title
		case "Main":
			StageMakingHasBeenExecuted = true;
			goto case "Stage02";
		case "Stage01":
		case "Stage02":
		case "StageBoss":
			
			player_life = DEFAULT_LIFE;
			goto default;
		case "Tutorial":
			player_life = 9999;
			goto default;			
		default:
			SetLoadingSkin(true);
			current_selection_pause = SELECTION_PAUSE.RESUME;
			
			break;
		}
	}
	
	public void Update(){
		if( !Application.loadedLevelName.ToString().Contains("Event") ){
			if( (!Application.loadedLevelName.Contains("old") && !Application.loadedLevelName.Contains("Title")) && !StageMakingHasBeenExecuted ){
				int stageIdx = 0;
				while(Application.loadedLevelName != scenes[stageIdx]){
					stageIdx++;
				}
				
				m_sceneIdx = stageIdx;
				
				GameObject.FindWithTag("StageMaker").GetComponent<StageMaker>().SendMessage("Init", stageIdx);
				GameObject.FindWithTag("Opening").GetComponent<OpeningSet>().SendMessage("Activate");
				
				CountTresure();

				StageMakingHasBeenExecuted = true;
				SetLoadingSkin(false);
			}
		
			if(!playerIsBorn && player == null && StageMakingHasBeenExecuted){
				if(GameObject.FindWithTag("Player")){
					player = GameObject.FindWithTag("Player").GetComponent<Player>();
					player.SendMessage("init", this.gameObject);
					playerIsBorn = true;
					GameObject.FindWithTag("MainCamera").GetComponent<MainCamera>().enabled = true;
					
				}
			}
		}
		if(saveTimer > SAVE_INTERVAL){
			saveTimer = 0;
			PlayerPrefs.Save();
		}else{
			saveTimer += Time.deltaTime;
		}

	}
	
	private static void SetLoadingSkin(bool set){
		GameObject[] obj = GameObject.FindGameObjectsWithTag("Loading");
		
		if(set){
			foreach(GameObject child in obj){
				child.SendMessage("Activate");
			}
			
		}else{
			foreach(GameObject child in obj){
				child.SendMessage("Deactivate");
			}
		}
	}
	
	public static void CompleteOpneingDirection( bool key){
		openingDirectionIsCompleted = key;
	}
	
	public static bool CompleteOpneingDirection(){
		return openingDirectionIsCompleted;
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
		}else if(cmd == "GoToTitle"){
			Application.LoadLevel("Title");
		}else if(cmd == "GoToNext"){
			m_sceneIdx++;
			Application.LoadLevel(scenes[m_sceneIdx]);
		}
	}
	
	private void Restart(){
	
		GameObject[] setters = GameObject.FindGameObjectsWithTag("Setter");
		for(int i = 0 ; i < setters.Length ; i ++){
			setters[i].SendMessage("ResetItem");
		}
	
	
		GameObject obj = GameObject.FindWithTag("Player");
		obj.GetComponent<Player>().enabled = true;
		obj.SendMessage("Restart", m_respawnPos);
		inMissingDirection = false;
		Pause(false);
	}
	
	public static void GameStart(string levelName){
		SetLoadingSkin(true);
		Application.LoadLevel(levelName);
	}
	
	public void EnableUI(){
		ReassignScripts();
		soundManager.enabled = true;
		inputManager.enabled = true;

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
		case SELECTION_TITLE.EVENT1:
			return "EVENT1";
		case SELECTION_TITLE.STAGE1:
			return "STAGE1";
		case SELECTION_TITLE.STAGE2:
			return "STAGE2";
		case SELECTION_TITLE.BOSS:
			return "BOSS";
		default:
			return "ETC";
		}
	}
	
	public static int GetPauseStatus(){
		return (int)current_selection_pause;

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
				current_selection_title = SELECTION_TITLE.EVENT1;
				return;
			case SELECTION_TITLE.EVENT1:
				GameStart("Event01");
				return;
			case SELECTION_TITLE.STAGE1:
				GameStart("Stage01");
				return;
			case SELECTION_TITLE.STAGE2:
				GameStart("Stage02");
				return;
			case SELECTION_TITLE.BOSS:
				GameStart("StageBoss");
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
				current_selection_title = SELECTION_TITLE.EVENT1;			
				return;
			case SELECTION_TITLE.EVENT1:
				current_selection_title = dir ? SELECTION_TITLE.STAGE1 : SELECTION_TITLE.BOSS;			
				return;
			case SELECTION_TITLE.STAGE1:
				current_selection_title = dir ? SELECTION_TITLE.STAGE2 : SELECTION_TITLE.EVENT1;			
				return;
			case SELECTION_TITLE.STAGE2:
				current_selection_title = dir ? SELECTION_TITLE.BOSS : SELECTION_TITLE.STAGE1;			
					return;
			case SELECTION_TITLE.BOSS:
				current_selection_title = dir ? SELECTION_TITLE.EVENT1 : SELECTION_TITLE.STAGE2;			
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
		
		//player.SendMessage("GetExorcised");
		
		GetComponent<InputManager> ().enabled = false;
		
		if(player_life > 0){
			player_life--;
			StartCoroutine (WaitAndExecute(2.0f, "Restart"));
		}else{
			StartCoroutine (WaitAndExecute(4.0f, "GoToTitle"));
			gameover = true;
		}
	}
	
	public static bool GameClear(){
		return cleared;
	}
	
	IEnumerator WaitAndShowLogo(float delay){
		yield return new WaitForSeconds (delay);
		if(!cleared){
			cleared = true;
			StartCoroutine( WaitAndExecute(4.0f, "GoToNext") );
			SaveGame();
		}
	}
	
	public void GameClear(float delay){
	
		inputManager.SendMessage("Invalidate");
		if(!cleared){
			//Turn off BGM
			soundManager.SendMessage("FadeoutBGM");
			StartCoroutine (WaitAndShowLogo (delay));

		}
	}

	private void SaveGame(){
		if(Application.loadedLevelName.Contains("Stage")){
			string stageIndexName;

			for(int i = 0 ; i < numOfTreasure ; i++){
				stageIndexName = "Treasure_stg" + m_sceneIdx.ToString() + "_idx" + i.ToString();
				PlayerPrefs.SetInt(stageIndexName, treasureIsObtained[i] ? 1 : 0);
			}
		}
	}

	private void LoadTreauseStatus(){
		if(Application.loadedLevelName.Contains("Stage")){
			string stageIndexName;

			for(int i = 0 ; i < numOfTreasure ; i++){
				stageIndexName = "Treasure_stg" + m_sceneIdx.ToString() + "_idx" + i.ToString();
				treasureIsObtained[i] = PlayerPrefs.GetInt(stageIndexName, 0) == 1 ? true : false;
			}
		}
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
	
	private void CountTresure(){
		GameObject[] objs = GameObject.FindGameObjectsWithTag("Treasure");
		numOfTreasure = objs.Length;
		
		for(int i = 0 ; i < numOfTreasure ; i ++){
			for(int j = i ; j < numOfTreasure ; j++){
				if(i == j){
					continue;
				}
				if(objs[i].transform.position.x > objs[j].transform.position.x){
				GameObject obj = objs[i];
				objs[i] = objs[j];
				objs[j] = obj;
				
				}
			}
		}

		LoadTreauseStatus();
		
		for (int i = 0 ; i < numOfTreasure ; i++){
			objs[i].SendMessage("SetIndex", i);
			if(treasureIsObtained[i] == true){
				objs[i].SendMessage("SetAlpha", 0.25f);
			}
		}

	}
	
	private void UpdateTreasureInfo(int idx){
		if(idx == -1){
			Debug.Log("This treasure has Not been applied .");
			return;
		}
		
		treasureIsObtained[idx] = true;
	}
	
	public bool[] GetTreasureInfo(){
		bool[] treInfo = new bool[numOfTreasure];
		for(int i = 0 ; i < treInfo.Length ; i++){
			treInfo[i] = treasureIsObtained[i];
		}
		return treInfo;	
	}
	
	public static bool CheckCurrentPlayerIsHidden(){
		return IsHidden;
	}
	public static void InformBecomeHidden(bool hidden){
		IsHidden = hidden;
	}
}
