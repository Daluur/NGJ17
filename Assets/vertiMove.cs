using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vertiMove : MonoBehaviour {

	public int length;
	public int speed;
	float initialY;

	// Use this for initialization
	void Start () {
		initialY = transform.position.y;
	}

	// Update is called once per frame
	void Update () {
		transform.position = new Vector3 (transform.position.x, initialY + Mathf.PingPong (speed*Time.time, length), transform.position.z);
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
