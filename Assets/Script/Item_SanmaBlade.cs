using UnityEngine;
using System.Collections;

public class Item_SanmaBlade : Item {

	// Use this for initialization
	protected override void Start () {
		base.Start();
		item_type = ITEM_TYPE.REVIVAL;
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update();
	}
}
