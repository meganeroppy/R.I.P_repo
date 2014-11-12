using UnityEngine;
using System.Collections;

public class Effect_slush : Effect {

	// Use this for initialization
	protected override void Start () {
		Vector2 offset = GetComponent<SpriteRenderer>().sprite.bounds.size;
		offset += new Vector2( Random.Range(-0.3f, 0.3f), Random.Range(-0.3f, 0.3f));
		base.Start();
		transform.position = transform.position + new Vector3(offset.x, offset.y, transform.position.z);
	}
}
