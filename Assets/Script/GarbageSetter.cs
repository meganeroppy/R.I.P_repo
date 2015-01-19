using UnityEngine;
using System.Collections;

public class GarbageSetter : ItemSetter {

	protected override void Start ()
	{
		respawnInterval = 15.0f;
		base.Start ();
	}
}
