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

	// Use this for initialization
	void Start () {
		switch(Application.loadedLevelName.ToString()){
		case "Title":
			break;//End of case Title
		case "Main":
		case "Tutorial":
		case "Test01":			
			break;//End of case "Main"
		default:
			break;
		}
	}

	// Update is called once per frame
	void Update () {

		switch(Application.loadedLevelName.ToString()){
		case "Title":
			switch(GameManager.current_selection_title){
				case  GameManager.SELECTION_TITLE.WAITFORKEY:
					if(Input.anyKeyDown){
						GameManager.AcceptInput(GameManager.BUTTON.START);
					}
					break;
			case  GameManager.SELECTION_TITLE.MAIN:
			case  GameManager.SELECTION_TITLE.TUTORIAL:
			case  GameManager.SELECTION_TITLE.TESTSTAGE:
			case  GameManager.SELECTION_TITLE.OPTION:
				if(Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.JoystickButton9)){
					GameManager.AcceptInput(GameManager.BUTTON.RIGHT);
				}
				if(Input.GetKeyDown(KeyCode.LeftArrow) ||Input.GetKeyDown(KeyCode.JoystickButton11)){
					GameManager.AcceptInput(GameManager.BUTTON.LEFT);
				}
				
				if(Input.GetKeyDown(KeyCode.KeypadEnter) 
				|| Input.GetKeyDown(KeyCode.Return) 
			    || Input.GetKeyDown(KeyCode.JoystickButton1)
		  	  || Input.GetKeyDown(KeyCode.JoystickButton2)
			  || Input.GetKeyDown(KeyCode.JoystickButton3)
			|| Input.GetKeyDown(KeyCode.JoystickButton7)){
					GameManager.AcceptInput(GameManager.BUTTON.START);
				}	
			break;
				default:
					break;
			}

			break;//End of case Title
		case "Main":
		case "Tutorial":
		case "Test01":
		
			if(m_player == null && !GameManager.Miss()){
				GameObject obj = GameObject.FindWithTag ("Player");
				if(obj == null){
					return;
				}
				
				m_player = obj.GetComponent<Player> ();	
			}
			
			if (Input.GetKeyDown (KeyCode.Z)
			|| Input.GetKeyDown (KeyCode.Joystick1Button1)
			|| Input.GetKeyDown (KeyCode.Joystick1Button2) 
			|| Input.GetKeyDown (KeyCode.Joystick1Button3) ) {
				m_player.SendMessage("Attack");		
			}
			
			if (Input.GetKeyDown (KeyCode.J) 
			|| Input.GetKeyDown (KeyCode.X)
			 || Input.GetKeyDown (KeyCode.Space)
			 || Input.GetKeyDown(KeyCode.Joystick1Button0)) {
				m_player.SendMessage("Jump");		
			}
			
			if( m_player.GetStatus() == STATUS.GHOST){
				float speedX = Input.GetAxis ("Horizontal");
				float speedY = Input.GetAxis ("Vertical");
				Vector2 speed = new Vector2(speedX, speedY);
				
				m_player.SendMessage ("UpdateMoveSpeed", speed);
			}else{
				float speed = Input.GetAxis ("Horizontal");
				//Debug.Log("move! : speed" + speed.ToString());
				
				m_player.SendMessage ("UpdateWalkSpeed", speed);
			}
			
			if (Input.GetKeyDown (KeyCode.P) || Input.GetKeyDown(KeyCode.Joystick4Button6)) {
				GameManager.Pause();
				
			}
			
			break;//End of case "Main"
		default:
			break;
		}



	}
}

