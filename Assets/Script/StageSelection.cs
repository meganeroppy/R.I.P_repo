using UnityEngine;
using System.Collections;


public class StageSelection : UI {

	protected int numOfStage = 0;
	private const int ROW = 4;
	public GameObject selectionTemplate;
	private GameObject[] selection = new GameObject[50];
	private UILabel[] selection_label = new UILabel[50];
	private int stageIndex;
	private const float scale_label = 30.0f;
	private int current_selection = 0;
	
	protected override void Start ()
	{
		base.Start ();
		//activated = true;
		//numOfStage = 12;
		stageIndex = 0;
		float basePosX = -450;
		float intervalX = 200;
		float posY = -200;
		float intervalY = 100;
		do{
			for(int j = 0 ; j < ROW ; j++){
				selection[stageIndex] = Instantiate(selectionTemplate) as GameObject;
				selection[stageIndex].transform.parent = transform;
				selection[stageIndex].transform.localScale = new Vector3(scale_label, scale_label, 1);
				selection[stageIndex].transform.localPosition = new Vector3(basePosX + (j * intervalX), posY,0);
				
				selection_label[stageIndex] = selection[stageIndex].GetComponent<UILabel>();
				
				stageIndex++;
				
				if(stageIndex >= numOfStage){
					break;
				}
			}
			posY -= intervalY;
		}while(stageIndex < numOfStage);
	}
	
	protected override void Update ()
	{
		base.Update ();
		
		for(int i = 0 ; i < numOfStage ; i ++){
			if(i == current_selection){
				selection_label[i].alpha = 1.0f;
			}else{
				selection_label[i].alpha = 0.5f;
			}
		}
	}
	
	protected void Activate(int numOfStage){
		base.Activate();
		this.numOfStage = numOfStage;
	}
}
