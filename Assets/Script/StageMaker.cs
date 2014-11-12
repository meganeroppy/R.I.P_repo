using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class StageMaker : MonoBehaviour {

	private const float DEFAULT_PIECE_SCALE = 5.12f;
	private const float PIECE_SCALE = 3.20f;
	
	//system
	public TextAsset csv; 
	
	public GameObject[] stagePiece;
	public GameObject[] stageObject;
	
	private float[] obj_ZOrder = {-2,-2,-5,-5, 0};
	
	// Use this for initialization
	void Start () {		
		//Set size of stage pieces;
	
//		CSVReader.DebugOutputGrid( CSVReader.SplitCsvGrid(csv.text) ); 
		 string[,] pieces = CSVReader.SplitCsvGrid(csv.text);
		 
		 Vector2 stage_size = GetStageSize(pieces);
		 
		
		for(int hIdx = 0; hIdx < stage_size.y ; hIdx++){
			for(int wIdx = 0; wIdx < stage_size.x ;wIdx++){
				string p_code = pieces[wIdx, hIdx];			
				
				CreateStagePiece(p_code, new Vector3(wIdx * PIECE_SCALE, stage_size.y - (hIdx * PIECE_SCALE), 0.0f));
				//Analyze code
			
				//Instantiate(stg, new Vector3(wIdx * PIECE_SCALE, stage_size.y - ( hIdx * PIECE_SCALE), 0.0f), transform.rotation);
				//stg.transform.parent = this.transform;
			}
		}
	}	
	
	protected virtual bool init(){
		return true;
	}

	// Update is called once per frame
	void Update () {
	}
	
	
	
	private void AddFunction(int key, GameObject target){
		if(key == 3){
			//Goal
			target.gameObject.AddComponent<Goal>();
			if(target.GetComponent<Collider>() == null){
				BoxCollider2D col = target.AddComponent<BoxCollider2D>();
				col.isTrigger = true;
				col.size = new Vector2( PIECE_SCALE, PIECE_SCALE);
			} 
		}
	
	}
	
	private void CreateStagePiece(string p_code, Vector3 pos){
	
		if(p_code == null){
			return;
		}
	
		if( p_code.Equals("") || p_code.Equals("0") || p_code.EndsWith(" ")){
			return;
		}
				
		//Convert into int value
		int iP_code = int.Parse(p_code);
		
		GameObject stg;
		
		int v_key = iP_code % 10;
		iP_code = Mathf.RoundToInt(iP_code * 0.1f);
		
		//Stage Visual
		if(v_key != 0){
			stg = Instantiate(stagePiece[v_key-1], pos, this.transform.rotation) as GameObject;
			
		}else{
			stg = new GameObject("Empty");
			stg.transform.position = pos;
		}
		
		stg.transform.parent = this.transform;
		
		float scale = PIECE_SCALE / DEFAULT_PIECE_SCALE;
		stg.transform.localScale = new Vector3(scale, scale, scale);
		
		//Stage Object
		int o_key = iP_code % 10;
		iP_code = Mathf.RoundToInt(iP_code * 0.1f);
		
		if(o_key != 0){
			pos.z -= 2.0f;
			GameObject obj = Instantiate(stageObject[o_key-1], pos, this.transform.rotation) as GameObject;
			obj.transform.Translate(0.0f, 0.0f, obj_ZOrder[o_key-1]);
			obj.transform.parent = stg.transform;
			//obj.transform.localScale = new Vector3(scale, scale, scale);
		}
		
		//Stage Function
		int f_key = iP_code;
		
		if(f_key != 0){
			AddFunction(f_key, stg);
		}
		
	}
	
	//Check and return the stage width & height loaded
	private Vector2 GetStageSize(string[,] piece){
	
		int width = 0;
		while(true){
			if(piece[width, 0] != null){
				width++;
			}else{
				break;
			}
		}
		
		int height = 0;
		while(true){
			if(piece[0,height] != null){
				height++;
			}else{
				break;
			}
		}
		
		return new Vector2(width, height);
	}
}
