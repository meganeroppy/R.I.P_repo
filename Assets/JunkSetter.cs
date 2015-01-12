using UnityEngine;
using System.Collections;

public class JunkSetter : ItemSetter {
	protected override void Start ()
	{
		respawnInterval = 12.0f;
		base.Start ();
	}
}
