using UnityEngine;
using System.Collections;

public class ShadeSetter : ItemSetter {
	protected override void Start ()
	{
		respawnInterval = 1000.0f;
		base.Start ();
	}

}
