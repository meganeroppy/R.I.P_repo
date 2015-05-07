using UnityEngine;
using System.Collections;

public class SimpleAnimator : MonoBehaviour {


	public Sprite[] pics;
	protected int current_frame = 0;
	protected float m_timer = 0.0f;
	protected float animInterval = 0.075f;
	
	protected SpriteRenderer spriteRenderer;
	
	// Use this for initialization
	void Start () {
	
		spriteRenderer = GetComponent<SpriteRenderer>();
		

	}
	
	// Update is called once per frame
	void Update () {
		if(m_timer > animInterval){
			m_timer = 0.0f;
			current_frame = current_frame == pics.Length ? 0 : current_frame+1;
			spriteRenderer.sprite = pics[current_frame];
		}else{
			m_timer += Time.deltaTime;
		}
	}
}
