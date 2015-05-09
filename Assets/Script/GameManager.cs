using UnityEngine;
using System.Collections;
using System.Collections.Generic;

 public class GameManager : MonoBehaviour {

	private string[] scenes = new string[]{
		"Tutorial",	
		"Stage01",
		"Stage02",
		"StageBoss",
		//"Event02",
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

	public static bool playerIsBorn = false;
	private bool StageMakingHasBeenExecuted = false;
	private int m_sceneIdx = 0;
	private int numOfTreasure = 0;
	private bool[] treasureIsObtained = new bool[5];
	private Player player;
	
	private static Vector3	m_respawnPos;

	//System
	//Scripts
	private static SoundManager sound;
	private static InputManager input;
	private static GUIManager gui;
	private static MainCamera mainCamera;
	//private static SceneManager scene;
	private float saveTimer = 0;
	private const float SAVE_INTERVAL = 5f;
	
	void Awake(){
		Application.targetFrameRate = 30;
		//scene = GameObject.Find("SceneManager").GetComponent<SceneManager>();
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
		
		switch(SceneManager.currentLevelName.ToString()){
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
		if( !SceneManager.currentLevelName.Contains("Event") ){
			if( (!SceneManager.currentLevelName.Contains("old") && !SceneManager.currentLevelName.Contains("Title")) && !StageMakingHasBeenExecuted ){
				int stageIdx = 0;
				while(SceneManager.currentLevelName != scenes[stageIdx]){
					stageIdx++;
				}
				
				m_sceneIdx = stageIdx;
				
				GameObject.FindWithTag("StageMaker").GetComponent<StageMaker>().Init(stageIdx);
				GameObject.FindWithTag("Opening").GetComponent<OpeningSet>().Activate();
				
				CountTresure();

				StageMakingHasBeenExecuted = true;
				SetLoadingSkin(false);
			}
		
			if(!playerIsBorn && player == null && StageMakingHasBeenExecuted){
				if(GameObject.FindWithTag("Player")){
					player = GameObject.FindWithTag("Player").GetComponent<Player>();
					player.init(this.gameObject);
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
				child.GetComponent<LoadingSet>().Activate();
			}
			
		}else{
			foreach(GameObject child in obj){
				child.GetComponent<LoadingSet>().Deactivate();
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
			SceneManager.LoadLevelAdditive("Title");
//			Application.LoadLevel("Title");
		}else if(cmd == "GoToNext"){
			m_sceneIdx++;
			SceneManager.LoadLevelAdditive(scenes[m_sceneIdx]);
			//Application.LoadLevel(scenes[m_sceneIdx]);//////////////////
		}
	}
	
	private void Restart(){
	
		GameObject[] setters = GameObject.FindGameObjectsWithTag("Setter");
		for(int i = 0 ; i < setters.Length ; i ++){
			setters[i].SendMessage("ResetItem");
		}
	
	
		Player obj = GameObject.FindWithTag("Player").GetComponent<Player>();
		obj.enabled = true;
		obj.Restart(m_respawnPos);
		inMissingDirection = false;
		Pause(false);
	}
	
	public static void GameStart(string levelName){
		SetLoadingSkin(true);
		SceneManager.LoadLevelAdditive(levelName);
//		Application.LoadLevel(levelName);//////////////
	}
	
	public void EnableUI(){
		ReassignScripts();
		sound.enabled = true;
		input.enabled = true;

		GameObject[] obj = GameObject.FindGameObjectsWithTag("UI");
		foreach(GameObject child in obj){
			if(child.name.Contains("stage")){
				child.SendMessage("Activate", 12);
			}else{
				child.SendMessage("Activate");
			}
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
				GameStart(SceneManager.currentLevelName);///////////////
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
		mainCamera.ReleaseTarget();
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
	
		input.Invalidate();
		if(!cleared){
			//Turn off BGM
			sound.FadeoutBGM();
			StartCoroutine (WaitAndShowLogo (delay));

		}
	}

	private void SaveGame(){
		if(SceneManager.currentLevelName.Contains("Stage")){
			string stageIndexName;

			for(int i = 0 ; i < numOfTreasure ; i++){
				stageIndexName = "Treasure_stg" + m_sceneIdx.ToString() + "_idx" + i.ToString();
				PlayerPrefs.SetInt(stageIndexName, treasureIsObtained[i] ? 1 : 0);
			}
		}
	}

	private void LoadTreauseStatus(){
		if(SceneManager.currentLevelName.Contains("Stage")){
			string stageIndexName;

			for(int i = 0 ; i < numOfTreasure ; i++){
				stageIndexName = "Treasure_stg" + m_sceneIdx.ToString() + "_idx" + i.ToString();
				treasureIsObtained[i] = PlayerPrefs.GetInt(stageIndexName, 0) == 1 ? true : false;
			}
		}
	}

	private void ReassignScripts(){

		if(sound == null){
			sound = GetComponent<SoundManager>();
		}
		if(input == null){
			input = GetComponent<InputManager>();
		}
		if(gui == null){
			gui = GetComponent<GUIManager>();
		}
		if(gui == null){
			gui = GetComponent<GUIManager>();
		}
		if( mainCamera == null){
			mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<MainCamera>();
		}
	}
	
	public void SetRespawnPoint(Vector3 newPos){
		m_respawnPos = newPos;
//		Debug.Log("ApplyNewRespawnPos : " + m_respawnPos.ToString());
	}
	
	public static Vector3 GetCurrentRespawnPosition(){
		return m_respawnPos;
	}
	
	public static bool GetPlayerIsGhost(){
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
		Treasure tre = objs[i].GetComponent<Treasure>();
		tre.SetIndex(i);
			if(treasureIsObtained[i] == true){
				tre.SetAlpha(0.25f);
			}
		}

	}
	
	public void SetTreasureInfo(int idx){
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
