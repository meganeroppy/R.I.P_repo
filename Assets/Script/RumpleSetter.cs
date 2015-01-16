using UnityEngine;
using System.Collections;

public class RumpleSetter : ItemSetter {
	protected override void Start(){
		respawnInterval = 10.0f;
		base.Start();
		
	}
	protected override void UpdateChildrenAlpha ()
	{
		return;
	}
}
