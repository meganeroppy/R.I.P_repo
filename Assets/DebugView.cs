using UnityEngine;
using System.Collections;

public class DebugView : UI {
	
	
	
	protected UILabel uiLabel;
	protected Player player;
	protected bool grounded;
	STATUS status;
	//public Transform panel;
	//public Transform uiLabelPrefab;
	//private const int NUMOFSELECTION = 4;
	//private GameObject[] label = new GameObject[NUMOFSELECTION];
	//private UILabel[] uiLabel_selection = new UILabel[NUMOFSELECTION];
	
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
		uiLabel.text = "ground : " + grounded.ToString() + "\nstatus : " + status.ToString() + "\nfps : " + fps.ToString() ;
	}
	
	protected override void Activate (){
	
	
	if(player == null){
		player = GameObject.FindWithTag("Player").GetComponent<Player> ();
	}
		
		activated = true;
	}
}
