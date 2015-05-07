using UnityEngine;
using System.Collections;

public class StinkySetter : ItemSetter {
	protected override void Start ()
	{
		respawnInterval = 5.0f;
		
		base.Start ();
	}
}
