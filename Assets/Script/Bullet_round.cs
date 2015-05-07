using UnityEngine;
using System.Collections;

public class Bullet_round : Bullet{

	protected override void Update ()
	{
		transform.Rotate(0, 0, 200.0f * Time.deltaTime);
		base.Update ();
	}

}
