using UnityEngine;
using System.Collections;

public class GarbageSetter : ItemSetter {

	protected override void Start ()
	{
		respawnDelay = 15.0f;
		base.Start ();
	}
}
