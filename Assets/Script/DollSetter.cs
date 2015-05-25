using UnityEngine;
using System.Collections;

public class DollSetter : ItemSetter {



	protected override void Start ()
	{
		respawnInterval = 4.0f;
		
		base.Start ();
	}
	
	protected override Object SetEffect(){
		return Resources.Load("Prefab/Flash");
	}

	protected override void UpdateChildrenAlpha ()
	{
		return;
	}
}
