using UnityEngine;
using System.Collections;

public class SpikyWire : Needle {

	// Use this for initialization
	protected override void Start () {
		base.Start();
		float scale = GameManager.PIECE_SCALE / GameManager.DEFAULT_PIECE_SCALE;
		transform.localScale = new Vector3(transform.localScale.x * scale, transform.localScale.x * scale, scale);
	}
}
