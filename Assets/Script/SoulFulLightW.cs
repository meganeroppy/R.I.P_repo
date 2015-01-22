using UnityEngine;
using System.Collections;

public class SoulFulLightW : SoulfulLight {

	// Use this for initialization
	protected override void Start () {
		builtOnGround = false;
		base.Start();
		m_healRangeCenterOffset = Vector3.zero;
	}
	

}
