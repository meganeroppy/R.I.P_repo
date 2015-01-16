using UnityEngine;
using System.Collections;

public class WraithSetter : ItemSetter {

	// Use this for initialization
	protected override void Start () {
		respawnInterval = 5.0f;
		base.Start();
		
	}
	/*
	protected override void UpdateChildrenAlpha ()
	{
		return;
	}
	*/
}
