using UnityEngine;
using System.Collections;

public class DebugView : UI {
	
	
	
	protected UILabel uiLabel;
	protected Player player;
	protected bool grounded;
	STATUS status;
	
	protected override void Start ()
	{
		uiLabel = GetComponent<UILabel>();
		activated = false;
		uiLabel.enabled = true;
	}
	
	// Update is called once per frame
	protected override void Update () {
	
		if(!activated){
			return;
		}
		
		grounded = player.grounded;
		status = player.GetStatus();
		
		string fps = Application.targetFrameRate.ToString();
		uiLabel.text = "R Stick Vec: " + new Vector2( Input.GetAxis("Right Stick Horizontal"), Input.GetAxis("Right Stick Vertical") ).ToString() + "\nground : " + grounded.ToString() + "\nstatus : " + status.ToString() + "\nfps : " + fps.ToString() ;
	}
	
	public override void Activate (){
	
	
	if(player == null){
		player = GameObject.FindWithTag("Player").GetComponent<Player> ();
	}
		
		activated = true;
	}
}
