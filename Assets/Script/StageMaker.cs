using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class StageMaker : MonoBehaviour {


	private const float DEFAULT_PIECE_SCALE = 5.12f;
	private const float PIECE_SCALE = 3.20f;
	
	//system
	public TextAsset[] csv; 
	
	public GameObject[] stagePiece;
	public GameObject[] stageObject;
	public GameObject[] stageFunction;
	
	private int len_pCode = 0;
//	public string[] stageFunction;
	//	 [] stageFunction;
//	 MonoBehaviour[] stageFunction;
	
	public GameObject bgSet;
	//private float[] obj_ZOrder = {-2,-2,-5,-5, 0,-2};
	
	// Use this for initialization
	void Start () {		
	}
	
	/*
	protected virtual bool Init(){
		return Init (0);
	}
	*/
	
	protected virtual bool Init(int mapIdx){
		//Set size of stage pieces;
		//	CSVReader.DebugOutputGrid( CSVReader.SplitCsvGrid(csv.text) ); 
		string[,] pieces = CSVReader.SplitCsvGrid(csv[mapIdx].text);
		
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
		
		//After all stage parts are made, BGParts will be set.
		Instantiate(bgSet, Vector3.zero, transform.rotation);
		
		return true;
	}
	
	private void CreateStagePiece(string p_code, Vector3 pos){
	
		if(p_code == null){
			return;
		}
	
		if( p_code.Equals("") || p_code.Equals("0") || p_code.EndsWith(" ")){
			return;
		}
				
		//Convert into int value
		//int iP_code = int.Parse(p_code, System.Globalization.NumberStyles.HexNumber);
		int iP_code = int.Parse(p_code);
		
		GameObject stg;
		
		int v_key = iP_code % 100;
		iP_code /= 100;
		iP_code = Mathf.RoundToInt(iP_code);
		
		//Stage Visual///////////
		if(v_key != 0){
			stg = Instantiate(stagePiece[v_key-1]) as GameObject;
			stg.transform.position = pos;
		}else{
			stg = new GameObject("Empty");
			stg.transform.position = pos;
		}
		
		stg.transform.parent = this.transform;
		
		float scale = PIECE_SCALE / DEFAULT_PIECE_SCALE;
		stg.transform.localScale = new Vector3(stg.transform.localScale.x * scale, scale, scale);
		
		//Stage Object/////////
		int o_key = iP_code % 10;
		iP_code /= 10;
		iP_code = Mathf.RoundToInt(iP_code);
		
		if(o_key != 0){
			pos.z -= 2.0f;
			GameObject obj = Instantiate(stageObject[o_key-1], pos, this.transform.rotation) as GameObject;
			//obj.transform.Translate(0.0f, 0.0f, obj_ZOrder[o_key-1]);
			float zOrder = obj.tag == ("Monument") ? -2.0f : (obj.tag == ("Enemy") || obj.tag == "Player") ? -5.0f : 0.0f;
			obj.transform.Translate(0.0f, 0.0f, zOrder);
			obj.transform.parent = stg.transform;
		}
		
		//Stage Function//////
		int f_key = iP_code;
		
		if(f_key != 0){
			AddFunction(f_key, stg);
		}	
	}
	
	private void AddFunction(int f_key, GameObject target){
		
		if(f_key != 0){
			GameObject obj = Instantiate(stageFunction[f_key-1], target.transform.position, this.transform.rotation) as GameObject;
			obj.transform.parent = target.transform;
			if(target.GetComponent<Collider>() == null){
				BoxCollider2D col = obj.AddComponent<BoxCollider2D>();
				col.isTrigger = true;
				col.size = new Vector2( PIECE_SCALE, PIECE_SCALE);
			}
		}
		
	/*
		//if(key == 3){
		target.gameObject.AddComponent(stageFunction[f_key-1]);
			if(target.GetComponent<Collider>() == null){
				BoxCollider2D col = target.AddComponent<BoxCollider2D>();
				col.isTrigger = true;
				col.size = new Vector2( PIECE_SCALE, PIECE_SCALE);
			} 
		///}
	*/	
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
