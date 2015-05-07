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


public class InputManager : MonoBehaviour {
	
	private Player m_player;
	public static float charging = 0.0f;
	private bool invalid = false;

	// Use this for initialization
	void Start () {
		invalid = false;
		switch(Application.loadedLevelName.ToString()){
		case "Title":
			break;//End of case Title
		case "Main":
		case "Tutorial":
		case "Stage01":			
			break;//End of case "Main"
		default:
			break;
		}
	}

	// Update is called once per frame
	void Update () {
	
		if(invalid){
			return;
		}

		switch(Application.loadedLevelName.ToString()){
		case "Title":
			switch(GameManager.current_selection_title){
				case  GameManager.SELECTION_TITLE.WAITFORKEY:
					if(Input.anyKeyDown){
						GameManager.AcceptInput("Title", GameManager.BUTTON.DECIDE);
					}
					break;
			case  GameManager.SELECTION_TITLE.EVENT1:
			case  GameManager.SELECTION_TITLE.STAGE1:
			case  GameManager.SELECTION_TITLE.STAGE2:
			case  GameManager.SELECTION_TITLE.BOSS:
				if(Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.JoystickButton9)){
					GameManager.AcceptInput("Title", GameManager.BUTTON.RIGHT);
				}
				if(Input.GetKeyDown(KeyCode.LeftArrow) ||Input.GetKeyDown(KeyCode.JoystickButton11)){
					GameManager.AcceptInput("Title", GameManager.BUTTON.LEFT);
				}
				
				if(Input.GetKeyDown(KeyCode.KeypadEnter) 
				|| Input.GetKeyDown(KeyCode.Return) 
				|| Input.GetKeyDown(KeyCode.JoystickButton1)
				|| Input.GetKeyDown(KeyCode.JoystickButton2)
				|| Input.GetKeyDown(KeyCode.JoystickButton3)
				||Input.GetKeyDown(KeyCode.JoystickButton7)){
					GameManager.AcceptInput("Title", GameManager.BUTTON.DECIDE);
				}	
			break;
				default:
					break;
			}

			break;//End of case Title
		case "Event01":
			
			break;
		default:
		
		///////////////////////////////////////
		//Main Game
		///////////////////////////////////////
			if(GameManager.GameClear() || !GameManager.CompleteOpneingDirection()){
				return;
			}
			
			
			if(!GameManager.Pause()){
				if (Input.GetKeyDown (KeyCode.P) || Input.GetKeyDown(KeyCode.JoystickButton7)) {
					GameManager.Pause(true);
					return;
				}
			}else{
				///In The Pause Menu FROM THIS LINE////////
				
				if (Input.GetKeyDown (KeyCode.P) || Input.GetKeyDown(KeyCode.JoystickButton7)) {
					GameManager.Pause(false);
					return;
				}
				
				if(Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.JoystickButton10)){
					GameManager.AcceptInput("Pause", GameManager.BUTTON.DOWN);
				}
				if(Input.GetKeyDown(KeyCode.UpArrow) ||Input.GetKeyDown(KeyCode.JoystickButton8)){
					GameManager.AcceptInput("Pause", GameManager.BUTTON.UP);
				}
				
				if(Input.GetKeyDown(KeyCode.KeypadEnter) //DECIDE
				   || Input.GetKeyDown(KeyCode.Return) 
				   || Input.GetKeyDown(KeyCode.JoystickButton1)
				   || Input.GetKeyDown(KeyCode.JoystickButton2)
				   || Input.GetKeyDown(KeyCode.JoystickButton3)
				   ||Input.GetKeyDown(KeyCode.JoystickButton7)){
					GameManager.AcceptInput("Pause", GameManager.BUTTON.DECIDE);
				}	
				if(Input.GetKeyDown(KeyCode.JoystickButton0)){
					GameManager.AcceptInput("Pause", GameManager.BUTTON.CANCEL);
				}
				return;
				///In The Pause Menu TO THIS LINE////////
			}
			
			if(m_player == null && !GameManager.Miss()){
				GameObject obj = GameObject.FindWithTag ("Player");
				if(obj == null){
					return;
				}
				
				m_player = obj.GetComponent<Player> ();	
			}
			
			if (Input.GetKeyDown (KeyCode.Z)
			    || Input.GetKeyDown (KeyCode.Joystick1Button2) ) {
				m_player.Attack(false);		
			}
			
			//About Charging Input
			if(Input.GetKey (KeyCode.Z)
			   || Input.GetKey (KeyCode.Joystick1Button2) ) {
				charging += Time.deltaTime;
				//	print(charging);
			}
			if(Input.GetKeyUp (KeyCode.Z)
			   || Input.GetKeyUp (KeyCode.Joystick1Button2) ) {
				if(charging >= 0.8f){
					m_player.Attack(true);		
				}
				charging = 0.0f;
			}
			
			if (Input.GetKeyDown (KeyCode.J) 
			    || Input.GetKeyDown (KeyCode.X)
			    || Input.GetKeyDown (KeyCode.Space)
			    || Input.GetKeyDown(KeyCode.Joystick1Button0)
			    || Input.GetKeyDown(KeyCode.Joystick1Button2)
			    || Input.GetKeyDown(KeyCode.Joystick1Button3)) {
				m_player.Jump();		
			}
			
			if (Input.GetKeyDown (KeyCode.Z)
			    || Input.GetKeyDown (KeyCode.Joystick1Button1)
			    || Input.GetKeyDown (KeyCode.Joystick1Button2) 
			    || Input.GetKeyDown (KeyCode.Joystick1Button3)
			    ||Input.GetKeyDown (KeyCode.J) 
			    || Input.GetKeyDown (KeyCode.X)
			    || Input.GetKeyDown (KeyCode.Space)
			    || Input.GetKeyDown(KeyCode.Joystick1Button0)) {
				m_player.CancelMotion();		
			}
			
			float analogInputX = Input.GetAxis ("Left Stick Horizontal") >= Mathf.Abs(0.05f) ? Input.GetAxis ("Left Stick Horizontal") : Input.GetAxis ("Horizontal");
			float analogInputY = Input.GetAxis ("Left Stick Vertical") >= Mathf.Abs(0.05f) ? - Input.GetAxis ("Left Stick Vertical") : Input.GetAxis ("Vertical");
			
			float speedX = Input.GetKey( KeyCode.JoystickButton9 ) ? 1.0f : Input.GetKey( KeyCode.JoystickButton11 ) ? -1.0f : analogInputX;
			float speedY = Input.GetKey( KeyCode.JoystickButton8 ) ? 1.0f : Input.GetKey( KeyCode.JoystickButton10 ) ? -1.0f : analogInputY;
			
			Vector2 speed = new Vector2(speedX, speedY);
			
			m_player.SetMoveSpeed(speed);
		
			break;
		}
	}
	
	public void Invalidate (){
		m_player.SetMoveSpeed(Vector2.zero);
		invalid = true;
	}
}

