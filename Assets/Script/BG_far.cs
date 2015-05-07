using UnityEngine;
using System.Collections;

public class BG_far: MonoBehaviour {
	
	//System
	private float[] PreviousNumber = {0, 0};
	
	//Status
	[HideInInspector]
	public enum METHOD{BY_SCORE, BY_DATE, BY_MODE};
	public METHOD changeMethod = METHOD.BY_DATE;
	private enum BG_SCENE{MOUNTAIN, SUNSET, NIGHT};
	private BG_SCENE cur_bgScene;
	[HideInInspector]
	public enum BG_SET{BUILDINGS, FORESTS};
	public BG_SET cur_bgSet;
	
	//BG base
	public GameObject mountain;
	public GameObject sunset;
	public GameObject night;
	private GameObject cur_BG;
	private GameObject pre_BG;
	private bool fadingOut;
	private float bg_alpha;
	/*
	//BG_Forests
	public GameObject forest_far;
	public GameObject forest_mid_far;
	public GameObject forest_mid_near;
	public GameObject forest_near;
	*/
	
	//BG_Buildings
	public GameObject buildings_far;
	public GameObject buildings_mid;
	public GameObject buildings_near;
	
	/*
	//BG_Ocean
	public GameObject ocean_far;
	public GameObject ocean_mid_far;
	public GameObject ocean_mid_near;
	public GameObject ocean_near;
	*/
	
	//Clouds
	public GameObject[] cloudPrefab;
	public GameObject[] cloudPrefab_large;
	
	private GameObject m_camera;
	
	// Use this for initialization
	void Start () {
		m_camera = GameObject.Find ("Main Camera");
		Vector3 cameraPos = m_camera.transform.position;
		transform.position = new Vector3 (cameraPos.x, cameraPos.y, cameraPos.z + 50.0f);
		transform.parent = m_camera.transform;
		
		fadingOut = false;
		renderer.enabled = false;
		/*
		if(changeMethod == METHOD.BY_DATE){
			float cur_hour = System.DateTime.Now.Hour;
			if(cur_hour >= 6 && cur_hour < 15){
				cur_BG = Instantiate(mountain, this.transform.position, this.transform.rotation) as GameObject;
				cur_bgScene = BG_SCENE.MOUNTAIN;
			}else if(cur_hour >= 15 && cur_hour < 20){
				cur_BG = Instantiate(sunset, this.transform.position, this.transform.rotation) as GameObject;
				cur_bgScene = BG_SCENE.SUNSET;
			}else{
				cur_BG = Instantiate(night, this.transform.position, this.transform.rotation) as GameObject;
				cur_bgScene = BG_SCENE.NIGHT;
			}
			cur_BG.transform.parent = this.transform;
		}else if(changeMethod == METHOD.BY_SCORE){
			//			cur_hour = HOUR.MOUNTAIN;
		}
		*/
		
		cur_BG = Instantiate(night, this.transform.position, this.transform.rotation) as GameObject;
		cur_bgScene = BG_SCENE.NIGHT;
		cur_BG.transform.parent = this.transform;

		
		//Set BackGrounds as the first shot
		if(cur_bgSet == BG_SET.BUILDINGS){
		/*
			SetBuildings("near");
			SetBuildings("mid");
		*/
		}else if (cur_bgSet == BG_SET.FORESTS){
		/*
			SetForests("near");
			SetForests("mid_near");
			SetForests("mid_far");
		*/
		}
		PreSetBG("buildings");
	}
	
	void Update () {
		if (fadingOut) {
			pre_BG.GetComponent<SpriteRenderer>().color = new Color(1,1,1,bg_alpha);
			if(bg_alpha > 0){
				bg_alpha -= 0.25f * Time.deltaTime;
			}else{
				cur_BG.transform.position = transform.position;
				Destroy(pre_BG.gameObject);
				fadingOut = false;
				bg_alpha = 1.0f;
			}
		}

		//Clouds
		/*
		if(Mathf.Floor( Time.frameCount * Time.deltaTime * 1000) % 100 == 0 && !GameManager.Pause()){
			Judge("cloud");
		}
		*/
		
		//Other BackGround Sets
		if(!GameManager.Pause()){
			if(cur_bgSet == BG_SET.BUILDINGS){
				if(Mathf.Floor( Time.frameCount * Time.deltaTime * 1000) % 420 == 0 && !GameManager.Pause()){
					Judge("building_near");
				}
				if(Mathf.Floor( Time.frameCount * Time.deltaTime * 1000) % 420 == 0 && !GameManager.Pause()){
					Judge("building_mid");
				}
			}else if (cur_bgSet == BG_SET.FORESTS){
			/*
				if(Mathf.Floor( Time.frameCount * Time.deltaTime * 1000) % 540 == 0 && !GameManager.Pause()){
					Judge("forests_near");
				}
				if(Mathf.Floor( Time.frameCount * Time.deltaTime * 1000) % 680 == 0 && !GameManager.Pause()){
					Judge("forests_mid_near");
				}
				if(Mathf.Floor( Time.frameCount * Time.deltaTime * 1000) % 680 == 0 && !GameManager.Pause()){
					Judge("forests_mid_far");
				}
				*/
			}
		}
	}
	
	public void SwitchPic(){
		if (cur_bgScene == BG_SCENE.MOUNTAIN) {
			SwitchPic("Sunset");
			cur_bgScene = BG_SCENE.SUNSET;
		} else if (cur_bgScene == BG_SCENE.SUNSET) {
			SwitchPic("Night");	
			cur_bgScene = BG_SCENE.NIGHT;
		} else {
			SwitchPic("Mountain");
			cur_bgScene = BG_SCENE.MOUNTAIN;
		}
	}
	
	public void SwitchPic(string time){
		Vector3 pos = transform.position;
		FadeOut(cur_BG.gameObject);
		switch (time) {
		case "Mountain":
			cur_BG = Instantiate(mountain, new Vector3(pos.x, pos.y, pos.z + 1.0f), this.transform.rotation) as GameObject;
			cur_BG.transform.parent = this.transform;
			break;
		case "Sunset":
			cur_BG = Instantiate(sunset, new Vector3(pos.x, pos.y, pos.z + 1.0f), this.transform.rotation) as GameObject;
			cur_BG.transform.parent = this.transform;
			break;
		case "Night":
			cur_BG = Instantiate(night, new Vector3(pos.x, pos.y, pos.z + 1.0f), this.transform.rotation) as GameObject;
			cur_BG.transform.parent = this.transform;
			break;
		default:
			break;
		}
		
	}
	
	private void FadeOut(GameObject bg){
		pre_BG = bg;
		fadingOut = true;
		bg_alpha = 1.0f;
	}
	
	private void Judge(string BGSet){
		switch(BGSet){
		case "cloud":
			if((int)Mathf.Floor(Random.Range(0, 2)) == 0){
				MakeCloud();
			}
			break;
			/*
		case "building_near":
			SetBuildings("near");
			break;
		case "building_mid":
			SetBuildings("mid");
			break;
			*/
			
			/*
		case "forests_near":
			SetForests("near");
			break;
		case "forests_mid_near":
			SetForests("mid_near");
			break;
		case "forests_mid_far":
			SetForests("mid_far");
			break;
			*/
		default:
			break;
		}
	}

	/*
	private void SetBuildings(string distance){
		if(distance == "near" ){
			float scale = Random.Range(0.4f, 0.7f);
			float speed = Random.Range(1.1f, 1.3f);
			GameObject near = Instantiate(buildings_near, new Vector3(this.transform.position.x + 20.0f, this.transform.position.y - 5.5f, this.transform.position.z - 20.0f), this.transform.rotation)as GameObject;
			near.transform.parent = this.transform;
			near.transform.localScale = new Vector3(scale , scale , this.transform.localScale.z);
		}
		if(distance == "mid" ){
			float scale = Random.Range(0.3f, 0.5f);
			float speed = Random.Range(0.6f, 0.8f);
			GameObject mid = Instantiate(buildings_mid, new Vector3(this.transform.position.x + 20.0f, this.transform.position.y - 5.5f, this.transform.position.z - 10.0f), this.transform.rotation)as GameObject;
			mid.transform.parent = this.transform;
			mid.transform.localScale = new Vector3(scale, scale , this.transform.localScale.z);
		}
	}
	*/
	
	/*
	private void SetForests(string distance){
		if(distance == "near" ){
			float scale = Random.Range(0.4f, 0.6f);
			float speed = Random.Range(1.1f, 1.3f);
			GameObject near = Instantiate(forest_near, new Vector3(this.transform.position.x + 20.0f, this.transform.position.y - 5.5f, this.transform.position.z - 20.0f), this.transform.rotation)as GameObject;
			near.transform.parent = this.transform;
			near.transform.localScale = new Vector3(scale , scale , this.transform.localScale.z);
			near.SendMessage("SetSpeed", speed);
		}
		if(distance == "mid_near" ){
			float scale = Random.Range(0.3f, 0.5f);
			float speed = Random.Range(0.6f, 0.8f);
			GameObject mid = Instantiate(forest_mid_near, new Vector3(this.transform.position.x + 20.0f, this.transform.position.y - 5.5f, this.transform.position.z - 15.0f), this.transform.rotation)as GameObject;
			mid.transform.parent = this.transform;
			mid.transform.localScale = new Vector3(scale, scale , this.transform.localScale.z);
			mid.SendMessage("SetSpeed", speed);
		}
		if(distance == "mid_far" ){
			float scale = Random.Range(0.3f, 0.5f);
			float speed = Random.Range(0.6f, 0.8f);
			GameObject mid = Instantiate(forest_mid_far, new Vector3(this.transform.position.x + 20.0f, this.transform.position.y - 5.5f, this.transform.position.z - 10.0f), this.transform.rotation)as GameObject;
			mid.transform.parent = this.transform;
			mid.transform.localScale = new Vector3(scale, scale , this.transform.localScale.z);
			mid.SendMessage("SetSpeed", speed);
		}
	}
	*/
	
	private void MakeCloud(){
		int cloudType = Random.Range(0, cloudPrefab.Length);
		if(cloudType == PreviousNumber[0]){
			while(cloudType == PreviousNumber[0]){
				cloudType = Random.Range(0, cloudPrefab.Length);
			}
		}else{
			PreviousNumber[0] = cloudType;
		}
		float posY = Random.Range(2, 6);
		if(posY == PreviousNumber[1]){
			while(posY == PreviousNumber[1]){
				posY = Random.Range(2, 6);
			}
		}else{
			PreviousNumber[1] = posY;
		}
		float posZ = 5.0f;
		GameObject cloud = Instantiate(cloudPrefab[cloudType], new Vector3(this.transform.position.x + 15.0f, this.transform.position.y + posY, this.transform.position.z - posZ), this.transform.rotation) as GameObject;
		cloud.transform.parent = this.transform;
	}
	
	private void PreSetBG(string BGSet){
		
		//Preset Clouds
		float posX;
		float posY;
		float posZ;
		for(int i = 0 ; i < 15 ; i++){
			int cloudType = Random.Range(0, cloudPrefab.Length);
			if(cloudType == PreviousNumber[0]){
				while(cloudType == PreviousNumber[0]){
					cloudType = Random.Range(0, cloudPrefab.Length);
				}
			}else{
				PreviousNumber[0] = cloudType;
			}
			posX = Random.Range(-20.0f, 20.0f);
			if(posX == PreviousNumber[1]){
				while(posX == PreviousNumber[1]){
					posX = Random.Range(-20.0f, 20.0f);
				}
			}else{
				PreviousNumber[1] = posX;
			}
			posY = Random.Range(3, 6);
			if(posY == PreviousNumber[1]){
				while(posY == PreviousNumber[1]){
					posY = Random.Range(3, 6);
				}
			}else{
				PreviousNumber[1] = posY;
			}
			posZ = 5.0f;
			GameObject cloud = Instantiate( cloudPrefab[cloudType], new Vector3(this.transform.position.x + posX, this.transform.position.y + posY, this.transform.position.z - posZ), this.transform.rotation) as GameObject;
			cloud.transform.parent = this.transform;
		}
		
		//Preset Large Clouds

		for(int i = 0 ; i < 10 ; i++){
			int cloudType = Random.Range(0, cloudPrefab_large.Length);
			if(cloudType == PreviousNumber[0]){
				while(cloudType == PreviousNumber[0]){
					cloudType = Random.Range(0, cloudPrefab_large.Length);
				}
			}else{
				PreviousNumber[0] = cloudType;
			}
			posX = Random.Range(-20.0f, 20.0f);
			if(posX == PreviousNumber[1]){
				while(posX == PreviousNumber[1]){
					posX = Random.Range(-20.0f, 20.0f);
				}
			}else{
				PreviousNumber[1] = posX;
			}
			posY = 7;
			posZ = 5.0f;
			GameObject cloud = Instantiate( cloudPrefab_large[cloudType], new Vector3(this.transform.position.x + posX, this.transform.position.y + posY, this.transform.position.z - posZ), this.transform.rotation) as GameObject;
			cloud.transform.parent = this.transform;
		}
		
		/*
		float scale;
		float speed;
		for(int i = 0 ; i < 2 ; i ++){
			if(cur_bgSet == BG_SET.BUILDINGS){
			
				//Preset Buildings
				scale = Random.Range(0.4f, 0.7f);
				speed = Random.Range(1.1f, 1.3f);
				posX = Random.Range(-3.0f + (i * 15.0f), 8.0f + (i * 15.0f));
				GameObject buildings = Instantiate(buildings_near, new Vector3(this.transform.position.x + posX, this.transform.position.y - 5.5f, this.transform.position.z - 15.0f), this.transform.rotation)as GameObject;
				buildings.transform.parent = this.transform;
				buildings.transform.localScale = new Vector3(scale , scale , this.transform.localScale.z);
				
				scale = Random.Range(0.3f, 0.5f);
				speed = Random.Range(0.6f, 0.8f);
				posX = Random.Range(-3.0f + (i * 15.0f), 8.0f + (i * 15.0f));
				buildings = Instantiate(buildings_mid, new Vector3(this.transform.position.x + posX, this.transform.position.y - 5.5f, this.transform.position.z - 10.0f), this.transform.rotation)as GameObject;
				buildings.transform.parent = this.transform;
				buildings.transform.localScale = new Vector3(scale, scale , this.transform.localScale.z);
				
				
			}else if(cur_bgSet == BG_SET.FORESTS){
			
				//Preset Forests
				scale = Random.Range(0.4f, 0.6f);
				speed = Random.Range(1.1f, 1.3f);
				posX = Random.Range(-3.0f + (i * 15.0f), 8.0f + (i * 15.0f));
				GameObject forests = Instantiate(forest_near, new Vector3(this.transform.position.x + posX, this.transform.position.y - 5.5f, this.transform.position.z - 15.0f), this.transform.rotation)as GameObject;
				forests.transform.parent = this.transform;
				forests.transform.localScale = new Vector3(scale , scale , this.transform.localScale.z);
				forests.SendMessage("SetSpeed", speed);
				
				scale = Random.Range(0.3f, 0.5f);
				speed = Random.Range(0.6f, 0.8f);
				posX = Random.Range(-3.0f + (i * 15.0f), 8.0f + (i * 15.0f));
				forests = Instantiate(forest_mid_near, new Vector3(this.transform.position.x + posX, this.transform.position.y - 5.5f, this.transform.position.z - 13.0f), this.transform.rotation)as GameObject;
				forests.transform.parent = this.transform;
				forests.transform.localScale = new Vector3(scale, scale , this.transform.localScale.z);
				forests.SendMessage("SetSpeed", speed);
				
				scale = Random.Range(0.3f, 0.5f);
				speed = Random.Range(0.6f, 0.8f);
				posX = Random.Range(-3.0f + (i * 15.0f), 8.0f + (i * 15.0f));
				forests = Instantiate(forest_mid_far, new Vector3(this.transform.position.x + posX, this.transform.position.y - 5.5f, this.transform.position.z - 10.0f), this.transform.rotation)as GameObject;
				forests.transform.parent = this.transform;
				forests.transform.localScale = new Vector3(scale, scale , this.transform.localScale.z);
				forests.SendMessage("SetSpeed", speed);
			
			}
			
		}
		*/
	}
	
	public void SwtichBGSet(){
		cur_bgSet = cur_bgSet == BG_SET.BUILDINGS ? BG_SET.FORESTS : BG_SET.BUILDINGS;
	}
	/*
	private void SetBGPic(int key){
		GetComponent<SpriteRenderer>().sprite = bg_pic[key];
	}
	*/
}
