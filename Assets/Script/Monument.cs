using UnityEngine;
using System.Collections;

public class Monument : StageObject {
	
	public GameObject effectPoint_destroy;
	protected Vector3 m_offset;
	protected bool builtOnGround = true;
	LayerMask layerMask;
	

	protected override void Start(){
	layerMask = 1 << 8;
		base.Start();

		if(builtOnGround){
			RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up, 100.0f, layerMask);
			
			if(hit.point.y - transform.position.y >= 100.0f){
				return;
			}else{
				Vector3 newPos = new Vector3(hit.point.x, hit.point.y, transform.position.z);
				transform.position = newPos;
			}
		}
		
		if(transform.parent != null){
			m_offset = transform.position - transform.parent.transform.position;
		}
	}

	protected override void ApplyHealthDamage(int value){
		base.ApplyHealthDamage(value);
		if (current_health <= 0) {
			Vector3 pos = transform.position;
			Vector2 colCenter = GetComponent<CircleCollider2D>().center;
			Vector3 CenterPos = new Vector3(pos.x + colCenter.x, pos.y + colCenter.y, pos.z - 1.0f);
			Instantiate(effectPoint_destroy, CenterPos, transform.rotation);
			StartCoroutine(WaitAndExecute(0.9f, true));
			m_collider.enabled = false;
		} else {
			StartCoroutine(WaitAndExecute(0.7f, false));
		}
	}

	protected override void Update(){

	base.Update();
		if(transform.parent != null){
			transform.position = transform.parent.transform.position + m_offset;
		}
	}

	protected virtual IEnumerator WaitAndExecute(float delay, bool dying){
		yield return new WaitForSeconds (delay);
		renderer.material.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
		invincible = false;
		if (dying) {
			Destroy (this.gameObject);
		}
	}
}
