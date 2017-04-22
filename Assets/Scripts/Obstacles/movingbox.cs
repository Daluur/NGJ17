using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movingbox : MonoBehaviour {

	public int length;
	public int speed;
	float initialX;

	// Use this for initialization
	void Start () {
		initialX = transform.position.x;
	}

	// Update is called once per frame
	void Update () {
		transform.position = new Vector3 (initialX + Mathf.PingPong (speed*Time.time, length), transform.position.y, transform.position.z);
	}

	private void OnCollisionEnter2D(Collision2D collision) {
		if (collision.gameObject.tag == "Player") {
			collision.transform.parent = transform;
		}
	}

	private void OnCollisionExit2D(Collision2D collision) {
		if (collision.gameObject.tag == "Player") {
			collision.transform.parent = null;
		}
	}
}
