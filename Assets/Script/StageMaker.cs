using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class StageMaker : MonoBehaviour {

	private enum END{
		TOP,
		RIGHT,
		BOTTOM,
		LEFT,
	};
	
	private float[] endsOfStage = new float[4]{0.0f, 0.0f, 0.0f, 0.0f};

	//private const float DEFAULT_PIECE_SCALE = 5.12f;
	//private const float PIECE_SCALE = 3.20f;
	
	//system
	public TextAsset[] csv; 
	
	public GameObject[] stagePiece;
	public GameObject[] stageObject;
	public GameObject[] stageFunction;
	
//	private int len_pCode = 0;
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
	
	protected virtual bool Init(int stageIdx){
		//Set size of stage pieces;
		//	CSVReader.DebugOutputGrid( CSVReader.SplitCsvGrid(csv.text) ); 
		string[,] pieces = CSVReader.SplitCsvGrid(csv[stageIdx].text);
		
		Vector2 stage_size = GetStageSize(pieces);
		endsOfStage[(int)END.TOP] = stage_size.y;
		endsOfStage[(int)END.RIGHT] = stage_size.x * GameManager.PIECE_SCALE;
		endsOfStage[(int)END.BOTTOM] = stage_size.y - (stage_size.y * GameManager.PIECE_SCALE);
		endsOfStage[(int)END.LEFT] = 0;
		GameObject.FindWithTag("MainCamera").GetComponent<MainCamera>().SendMessage("SetEndsOfStage", endsOfStage);
		
		for(int hIdx = 0; hIdx < stage_size.y ; hIdx++){
			for(int wIdx = 0; wIdx < stage_size.x ;wIdx++){
				string p_code = pieces[wIdx, hIdx];	
				
				
				
				CreateStagePiece(p_code, new Vector3(wIdx * GameManager.PIECE_SCALE, stage_size.y - (hIdx * GameManager.PIECE_SCALE), 0.0f));
				//Analyze code
				
				//Instantiate(stg, new Vector3(wIdx * GameManager.PIECE_SCALE, stage_size.y - ( hIdx * GameManager.PIECE_SCALE), 0.0f), transform.rotation);
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
		
		
		int v_key = iP_code % 100;
		int v_rotKey = iP_code % 1000 / 100;
		iP_code /= 1000;
		iP_code = Mathf.RoundToInt(iP_code);
		
		GameObject stg;
		
		
		//Stage Visual///////////
		if(v_key != 0){
		
			if(v_key > stagePiece.Length){
				stg = Instantiate(new GameObject("Empty")) as GameObject;
				Debug.Log("Selected Stage Piece doesn't exist. idx:(" +  pos.x.ToString() + ", " + pos.y.ToString() + ") code:" + p_code.ToString());
				
			}else{
				stg = Instantiate(stagePiece[v_key-1]) as GameObject;
			}
		}else{
			stg = new GameObject("Empty");
		}
		
		stg.transform.position = pos;
		stg.transform.parent = this.transform;
		
		float scale = GameManager.PIECE_SCALE / GameManager.DEFAULT_PIECE_SCALE;
		stg.transform.localScale = new Vector3(stg.transform.localScale.x * scale, stg.transform.localScale.x * scale, scale);
		
		if(v_rotKey != 0){
			RotateItem(stg, v_rotKey);
		}
		
		//Stage Object/////////
		int o_key = iP_code % 100;
		int o_rotKey = iP_code % 1000 / 100;
		
		iP_code /= 1000;
		iP_code = Mathf.RoundToInt(iP_code);
		
		if(o_key != 0){
			pos.z -= 2.0f;
			
			GameObject obj;
			if(o_key > stageObject.Length){
				obj = Instantiate(new GameObject("Empty")) as GameObject;
				Debug.Log("Selected Stage Object doesn't exist. idx:(" +  pos.x.ToString() + ", " + pos.y.ToString() + ") code:" + p_code.ToString());
			} else{
				obj = Instantiate(stageObject[o_key-1]) as GameObject;
			}
			
			obj.transform.position = pos;
			
			if(o_rotKey != 0){
				RotateItem(obj, o_rotKey);
			}
			
			//obj.transform.Translate(0.0f, 0.0f, obj_ZOrder[o_key-1]);
			float zOrder = obj.tag == ("Monument") ? -2.0f : (obj.tag == ("Enemy") || obj.tag == "Player") ? -5.0f : 0.0f;
			obj.transform.Translate(0.0f, 0.0f, zOrder);
			//obj.transform.parent = stg.transform;
			obj.transform.parent = transform;
		}
		
		//Stage Function//////
		int f_key = iP_code;
		
		if(f_key != 0){
			AddFunction(f_key, stg);
		}	
	}
	
	private void AddFunction(int f_key, GameObject target){
		
		if(f_key != 0){
			GameObject obj;
			if(f_key > stageFunction.Length){
				obj = Instantiate(new GameObject("Empty"), target.transform.position, this.transform.rotation) as GameObject;
				Debug.Log("Selected Stage Function doesn't exist. target:" + target.name);
			}else{
				obj = Instantiate(stageFunction[f_key-1], target.transform.position, this.transform.rotation) as GameObject;
				
			}
		
			obj.transform.parent = target.transform;
			if(target.GetComponent<Collider>() == null){
				BoxCollider2D col = obj.AddComponent<BoxCollider2D>();
				col.isTrigger = true;
				col.size = new Vector2( GameManager.PIECE_SCALE, GameManager.PIECE_SCALE);
			}
		}
		
	/*
		//if(key == 3){
		target.gameObject.AddComponent(stageFunction[f_key-1]);
			if(target.GetComponent<Collider>() == null){
				BoxCollider2D col = target.AddComponent<BoxCollider2D>();
				col.isTrigger = true;
				col.size = new Vector2( GameManager.PIECE_SCALE, GameManager.PIECE_SCALE);
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
	
	private void RotateItem(GameObject obj, int rotKey){
		Vector3 scale = obj.transform.localScale;
		switch(rotKey){
		case 1:
			obj.transform.rotation = Quaternion.Euler(0.0f, 0.0f, -90.0f);
			break;
		case 2:
			obj.transform.rotation = Quaternion.Euler(0.0f, 0.0f, -180.0f);
			break;	
		case 3:
			obj.transform.rotation = Quaternion.Euler(0.0f, 0.0f, -270.0f);
			break;
		case 4:
			obj.transform.localScale = new Vector3(-scale.x, scale.y, scale.z);
			break;	
		case 5:
			obj.transform.localScale = new Vector3(scale.x, -scale.y, scale.z);	
			break;
		case 6:
			obj.transform.localScale = new Vector3(-scale.x, -scale.y, scale.z);
			break;	
		default:
			break;	
		}
	}
}
