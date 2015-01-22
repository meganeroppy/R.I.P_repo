using UnityEngine;
using System.Collections;

public class DollSetter : ItemSetter {

	protected override void Start ()
	{
		respawnInterval = 4.0f;
		base.Start ();
	}

	protected override void UpdateChildrenAlpha ()
	{
		return;
	}
}
