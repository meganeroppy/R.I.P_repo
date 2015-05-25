using UnityEngine;
using System.Collections;

public class TitleManager : MonoBehaviour {

	string[] labels = new string[]{"START", "STORY","STAGE1","STAGE2","BOSS","test","test","test"};

	private enum CONDITION{
		PRESSSTART,
		TITLE_SELECTION,
	}
	private CONDITION curCondition = CONDITION.TITLE_SELECTION;
	[SerializeField]
	private GameObject menuLabelPrefab;
	private ArrayList menuLabels;
	private Vector3 itemOffset = new Vector3(100, 200);
	private const int MAX_LINE = 4;
	private const int MAX_RAW = 4;
	
	int stageNum = 0; // read from somewhere
	
	private void Awake(){
		menuLabels = new ArrayList();
		stageNum = labels.Length;
	}
	
	private void Start(){
		
		// create labels
		for(int i = 0 ; i < stageNum ; i++){
			MenuLabel obj = (Instantiate(menuLabelPrefab) as GameObject).GetComponent<MenuLabel>();
			obj.SetLabel(labels[i]);
		}
	}
	
	
}
