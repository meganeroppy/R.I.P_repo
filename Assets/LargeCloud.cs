using UnityEngine;
using System.Collections;

public class LargeCloud : Cloud {

	protected override void Start ()
	{
		SPEED_MIN = 0.2f;
		SPEED_MAX = 0.6f;
		SCALE_MIN = 1.5f;
		SCALE_MAX = 2.0f;
		base.Start ();
	}

}
