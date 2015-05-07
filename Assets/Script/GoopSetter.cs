using UnityEngine;
using System.Collections;

public class GoopSetter: ItemSetter {

	protected override void Start ()
	{
		respawnInterval = 15.0f;
		
		base.Start ();
	}
}
