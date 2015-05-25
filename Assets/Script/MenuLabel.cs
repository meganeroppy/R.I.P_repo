using UnityEngine;
using System.Collections;

public class MenuLabel : MonoBehaviour {

	private UILabel myLabel;
	
	public void Awake(){
		myLabel = GetComponent<UILabel>();
	}
	
	public void SetLabel(string newLabel){
		myLabel.text = newLabel;
	}
	
}
