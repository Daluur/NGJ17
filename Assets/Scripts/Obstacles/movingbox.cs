using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movingbox : MonoBehaviour {

	float initialX;

	// Use this for initialization
	void Start () {
		initialX = transform.position.x;
	}

	// Update is called once per frame
	void Update () {

		transform.position = new Vector3 (initialX + Mathf.PingPong (2*Time.time, 5), transform.position.y, transform.position.z);
	}
}
