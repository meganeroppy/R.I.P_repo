using UnityEngine;
using System.Collections;

public class GUIManager : MonoBehaviour {

	//System
	protected bool grounded;
	STATUS status;

	//Property
	protected float m_w;
	protected float m_h;

	//About Life Point
	private Vector2 m_basePos_lifePoint;
	private float m_scale_lifePoint;
	private float m_interval_life;
	public Texture2D[] icon_heart = new Texture2D[2];
	
/*
	//About spiritBar
	public Texture2D spiritBar_frame;
	public Texture2D spiritBar;
	private Vector2 m_basePos_spiritBar;
	private Vector2 m_scale_spiritBarFrame;
*/		

	//Player 
	private Player player;

	public bool DEBUG_MODE = true;

	// Use this for initialization
	void Start () {

		m_w = Screen.width;
		m_h = Screen.height;

		switch(Application.loadedLevelName.ToString()){

		case "Title":
			break;//End of case Title

		case "Main":
		case "Tutorial":
		case "Test01":
			
			
			player = GameObject.FindWithTag("Player").GetComponent<Player> ();
			grounded = player.grounded;
			status = player.GetStatus();

			//About LifePoint
			m_basePos_lifePoint = new Vector2 (m_w * 0.01f, m_h * 0.01f);
			m_scale_lifePoint = m_w * 0.03f;
			m_interval_life = m_w * 0.03f;

		/*
		//About spiritBar
		m_basePos_spiritBar = new Vector2 (m_w * 0.01f, m_h * 0.08f);
		m_scale_spiritBarFrame.y = m_h * 0.3f;
		m_scale_spiritBarFrame.x = m_w * 0.3f;
		*/		
			break;
			//End of case "Main"/////////////////////////
			////////////////////////////////////////////
		default:
			break;
		}
	}

	
	// Update is called once per frame
	void Update () {

		switch(Application.loadedLevelName.ToString()){
			
		case "Title":			
			break;//End of case "Title"

		case "Main":
		case "Tutorial":
		case "Test01":
			
			grounded = player.grounded;
			status = player.GetStatus();
			break;//End of case "Main"
		default:
			break;
		}

	}

	void OnGUI(){

		GUIStyle style =  new GUIStyle();

		switch(Application.loadedLevelName.ToString()){
			//Begin of case "Title"/////////////////////////
			////////////////////////////////////////////
		case "Title":

			style.fontSize = 18;
			style.normal.textColor = Color.gray;
			style.alignment = TextAnchor.MiddleCenter;
			
			Vector2 text_size = new Vector2(m_w * 0.2f, m_h * 0.05f);
			
			
			//Background Image
			//GUI.Box(new Rect(0.0f, 0.0f, m_w, m_h), "");
			
			switch(GameManager.GetTitleStatus()){
				case "WAITFORKEY":
				style.fontSize = 22;
				style.normal.textColor = Color.yellow;
				
				
				text_size = new Vector2(m_w * 0.4f, m_h * 0.05f);
				GUI.Box(new Rect((m_w * 0.5f) - (text_size.x * 0.5f), m_h * 0.9f, text_size.x, text_size.y), "Press Any Key To Start", style);
				break;
				
			case "MAIN":
				style.normal.textColor = Color.yellow;
				GUI.Box(new Rect((m_w * 0.2f) - (text_size.x * 0.5f), m_h * 0.9f, text_size.x, text_size.y), "Main", style);
				style.normal.textColor = Color.gray;
				GUI.Box(new Rect((m_w * 0.4f) - (text_size.x * 0.5f), m_h * 0.9f, text_size.x, text_size.y), "Tutorial", style);
				GUI.Box(new Rect((m_w * 0.6f) - (text_size.x * 0.5f), m_h * 0.9f, text_size.x, text_size.y), "TestStage", style);
				GUI.Box(new Rect((m_w * 0.8f) - (text_size.x * 0.5f), m_h * 0.9f, text_size.x, text_size.y), "Option", style);
				
				break;
			case "TUTORIAL":
				GUI.Box(new Rect((m_w * 0.2f) - (text_size.x * 0.5f), m_h * 0.9f, text_size.x, text_size.y), "Main", style);
				style.normal.textColor = Color.yellow;
				GUI.Box(new Rect((m_w * 0.4f) - (text_size.x * 0.5f), m_h * 0.9f, text_size.x, text_size.y), "Tutorial", style);
				style.normal.textColor = Color.gray;
				GUI.Box(new Rect((m_w * 0.6f) - (text_size.x * 0.5f), m_h * 0.9f, text_size.x, text_size.y), "TestStage", style);
				GUI.Box(new Rect((m_w * 0.8f) - (text_size.x * 0.5f), m_h * 0.9f, text_size.x, text_size.y), "Option", style);
				break;
			case "TESTSTAGE":
				GUI.Box(new Rect((m_w * 0.2f) - (text_size.x * 0.5f), m_h * 0.9f, text_size.x, text_size.y), "Main", style);
				GUI.Box(new Rect((m_w * 0.4f) - (text_size.x * 0.5f), m_h * 0.9f, text_size.x, text_size.y), "Tutorial", style);
				style.normal.textColor = Color.yellow;
				GUI.Box(new Rect((m_w * 0.6f) - (text_size.x * 0.5f), m_h * 0.9f, text_size.x, text_size.y), "TestStage", style);
				style.normal.textColor = Color.gray;
				GUI.Box(new Rect((m_w * 0.8f) - (text_size.x * 0.5f), m_h * 0.9f, text_size.x, text_size.y), "Option", style);
				break;
			case "OPTION":
				GUI.Box(new Rect((m_w * 0.2f) - (text_size.x * 0.5f), m_h * 0.9f, text_size.x, text_size.y), "Main", style);
				GUI.Box(new Rect((m_w * 0.4f) - (text_size.x * 0.5f), m_h * 0.9f, text_size.x, text_size.y), "Tutorial", style);
				GUI.Box(new Rect((m_w * 0.6f) - (text_size.x * 0.5f), m_h * 0.9f, text_size.x, text_size.y), "TestStage", style);
				style.normal.textColor = Color.yellow;
				GUI.Box(new Rect((m_w * 0.8f) - (text_size.x * 0.5f), m_h * 0.9f, text_size.x, text_size.y), "Option", style);
				break;
			}
			
			break;
			//End of case "Title"/////////////////////////
			////////////////////////////////////////////
		case "Main":
		case "Tutorial":
		case "Test01":
			
			style.normal.textColor = Color.yellow;
			
			//For Debug
			if(DEBUG_MODE){
				Vector2 base_pos = new Vector2(20, m_h - 50);
				
				GUI.Box (new Rect (base_pos.x, base_pos.y, 20, 20), "GROUND :" + grounded.ToString(), style);
				GUI.Box (new Rect (base_pos.x, base_pos.y + 20, 20, 20), "status :" + status.ToString(), style);
			}
			//End of For Debug
			style.fontSize = 25;
			
			if (GameManager.GameClear()) {
				style.normal.textColor = UnityEngine.Color.white;
				GUI.Box (new Rect (m_w * 0.05f, m_h * 0.8f, 40.0f, 20.0f), "CLEARED!", style);
			} else if (GameManager.Miss()) {
				style.normal.textColor = UnityEngine.Color.red;
				if( GameManager.GameOver()){
					GUI.Box (new Rect (m_w * 0.05f, m_h * 0.8f, 40.0f, 20.0f), "GAMEOVER!", style);
				}else{
					GUI.Box (new Rect (m_w * 0.05f, m_h * 0.8f, 40.0f, 20.0f), "MISSED!!", style);
				}
			}
			
			//Display FPS
			string fps = Application.targetFrameRate.ToString();
			GUI.Box (new Rect (m_w * 0.9f, m_h * 0.9f, 40.0f, 20.0f), fps, style);
			
			
			
			//About HitPoint
			int[] life = player.GetLifeInfo ();
			for (int i = 0; i < life[0]; i++) {
				if(i < life[1]){
					GUI.Box (new Rect (m_basePos_lifePoint.x + (m_interval_life * i), m_basePos_lifePoint.y, m_scale_lifePoint, m_scale_lifePoint), icon_heart [1], GUIStyle.none);
				}else{
					GUI.Box (new Rect (m_basePos_lifePoint.x + (m_interval_life * i), m_basePos_lifePoint.y, m_scale_lifePoint, m_scale_lifePoint), icon_heart [0], GUIStyle.none);
				}
			}
			//End of About HitPoint
			
			/*
					//About spiritBar
					float[] spirit = player.GetSpiritInfo ();
					float value_percent = spirit[1] / spirit[0];
					float scaleX = m_scale_spiritBarFrame.x * value_percent;
			
					GUI.Box (new Rect (m_basePos_spiritBar.x, m_basePos_spiritBar.y, m_scale_spiritBarFrame.x, m_scale_spiritBarFrame.y), spiritBar, GUIStyle.none);
			
					//spiritBar frame
					GUI.Box (new Rect (m_basePos_spiritBar.x, m_basePos_spiritBar.y, m_scale_spiritBarFrame.x, m_scale_spiritBarFrame.y), spiritBar_frame, GUIStyle.none);
			
					//End of About spiritBar
			*/
			break;
//End of case "Main"/////////////////////////
////////////////////////////////////////////
			
		default:
			break;
		}
	}

}
