using UnityEngine;
using System.Collections;

public class MenuSelection : UI {

	private enum SELECTION_SET{
		PRESS_START,
		MENU_SELECT,
		STAGE_SELECT,
	}
	private SELECTION_SET curSelectionSet = SELECTION_SET.PRESS_START;

	protected int numOfSelection = 0; // change dynamicly ralating to curSelectionSet
	private const int MAX_ROW = 4;
	[SerializeField]
	private GameObject menuSelection;
	private UILabel titleLabel = null;
	private ArrayList selections;
	private const float scale_label = 30.0f;
	private int current_selection = 0;
	private GameManager gameManager;
	//private bool waitForPressStart = true;
	
	
	protected void Awake(){
		selections = new ArrayList();
		gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
	}
	
	
	protected override void Start ()
	{
		base.Start ();
		curSelectionSet = SELECTION_SET.PRESS_START;
		DisplaySelections(curSelectionSet);
	}
	
	private void DisplaySelections(SELECTION_SET newSelectionSet){
	
		Cleanup(); // delete old selections
		
		current_selection = 0; // defaultSelection
		UILabel selection = null;
		// from top
		float basePosX = -450;
		float intervalX = 200;
		float basePosYFromTop = -100;
		float intervalY = 100;
		
		// from bottom
		Vector3 basePosFromBottom = new Vector3(0, -750, 0);
		
		switch(newSelectionSet){
		case SELECTION_SET.PRESS_START:
			
			selection = (Instantiate(menuSelection) as GameObject).GetComponent<UILabel>();
			
			selection.transform.parent = transform;
			selection.transform.localScale = new Vector3(50.0f, 50.0f, 1);
			selection.transform.localPosition = basePosFromBottom;
			selections.Add(selection);
			
			selection.text = "Press Any Key To Get Started";
			
			break;
		case SELECTION_SET.MENU_SELECT:
			
			string[] labels = new string[4]{"New Game", "Stage Select", "Option", "???"};
			numOfSelection = labels.Length;
			
			for(int j = 0 ; j < numOfSelection ; j++){
				
				intervalX = 330;
				
				selection = (Instantiate(menuSelection) as GameObject).GetComponent<UILabel>();
				
				selection.transform.parent = transform;
				selection.transform.localScale = new Vector3(scale_label * 1.15f, scale_label * 1.15f, 1);
				selection.transform.localPosition = basePosFromBottom + new Vector3(basePosX + ( (j % MAX_ROW) * intervalX), 0,0);
				selections.Add(selection);
				
				selection.text = labels[j];
			}
			break;
		case SELECTION_SET.STAGE_SELECT:
			
			float posY = basePosYFromTop;
			
			// title
			titleLabel = (Instantiate(menuSelection) as GameObject).GetComponent<UILabel>();
			titleLabel.transform.parent = transform;
			titleLabel.transform.localScale = new Vector3(scale_label * 1.4f, scale_label * 1.4f, 1);
			titleLabel.transform.localPosition = new Vector3(basePosX, posY, 0);
			titleLabel.text = "Stage Select";
			titleLabel.alpha = 1.0f;
			
			
			posY -= intervalY;	
			
			// stage list
			numOfSelection = 14; // stage + back
			
			
			for(int j = 0 ; j < numOfSelection ; j++){
				
				selection = (Instantiate(menuSelection) as GameObject).GetComponent<UILabel>();
				
				selection.transform.parent = transform;
				selection.transform.localScale = new Vector3(scale_label, scale_label, 1);
				
				
				// about label
				string labelText = "";
				if(j == numOfSelection -1){ // backButton
					labelText = "Back";
					posY -= intervalY;	//
					
					selection.transform.localPosition = new Vector3(basePosX, posY, 0);
					titleLabel.transform.localScale = new Vector3(scale_label * 1.1f, scale_label * 1.1f, 1);
				}else if(j == numOfSelection -2){ // boss stage
					labelText = "Boss";
					posY -= intervalY;	//
					selection.transform.localPosition = new Vector3(basePosX, posY, 0);
				}else{
					labelText = "Stage" +  string.Format("{0:D2}",(j+1));
					// about position
					if(j % MAX_ROW == 0 && j != 0){
						posY -= intervalY;	//
					}
					selection.transform.localPosition = new Vector3(basePosX + ( (j % MAX_ROW) * intervalX), posY, 0);
				}
				
				selection.text = labelText;
				
				selections.Add(selection);
				
			}
			
			break;
		}
	
		curSelectionSet = newSelectionSet;
		
	}
	
	
	public void AcceptInput(GameManager.BUTTON btn){
		
		UILabel label = selections[current_selection] as UILabel;
		
		switch(btn){
		case GameManager.BUTTON.DECIDE:
			if(curSelectionSet == SELECTION_SET.PRESS_START){
				
				DisplaySelections(SELECTION_SET.MENU_SELECT);
			}else if(curSelectionSet == SELECTION_SET.MENU_SELECT){
				if(label.text == "Stage Select"){
					DisplaySelections(SELECTION_SET.STAGE_SELECT);
				}else if(label.text == "New Game"){
					gameManager.InformAcception(label.text);
				}
			}else if(curSelectionSet == SELECTION_SET.STAGE_SELECT){
				if(label.text == "Back"){
					DisplaySelections(SELECTION_SET.MENU_SELECT);
				}else{
					gameManager.InformAcception(label.text);
				}
			}
			
			break;
		case GameManager.BUTTON.RIGHT:
		current_selection += current_selection < selections.Count-1 ? 1 : 0;
			break;	
		case GameManager.BUTTON.LEFT:
		current_selection -= current_selection > 0 ? 1 : 0;
			break;	
		case GameManager.BUTTON.UP:
			if(label.text == "Back"){
				current_selection--;
			}else if(current_selection >= 0 && current_selection <= 3){
				current_selection = numOfSelection-1;
			}else{
				current_selection -=  (current_selection - MAX_ROW >= 0)  ? MAX_ROW : 0;
			}
			break;	
		case GameManager.BUTTON.DOWN:
			if(label.text == "Boss"){
				current_selection++;
			}else if(label.text == "Back"){
				current_selection = 0;
			}else{
				current_selection +=  (current_selection + MAX_ROW <= selections.Count-1) ? MAX_ROW : 0;
			}			
			break;
			
		}
	}
	
	
	public string Accept(){
		UILabel label = selections[current_selection] as UILabel;
		return label.text;
	}
	
	
	protected override void Update ()
	{
		base.Update ();
		
		if(curSelectionSet == SELECTION_SET.PRESS_START){
			UILabel label = selections[0] as UILabel;
			label.alpha = 1.0f;		
		}else{	
			for(int i = 0 ; i < numOfSelection ; i ++){
				UILabel label = selections[i] as UILabel;
				label.alpha = i == current_selection ? 1.0f : 0.5f;
			}
		}
	}
	
	private void Cleanup(){
		for(int i = 0 ; i < selections.Count ; i++){
			UILabel label = selections[i] as UILabel;
			Destroy( label.gameObject );
		}
		
		if(titleLabel != null){
			Destroy(titleLabel);
		}
		selections.Clear();
	}
	
}
