using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swing : MonoBehaviour {

	bool leftswing;
	bool rightswing;
	float rotationAlign;

	// Use this for initialization
	void Start () {
		leftswing = true;
		rightswing = false;
	}
	
	// Update is called once per frame
	void Update () {
		rotationAlign = transform.eulerAngles.z;

		if (rightswing) {
			transform.Rotate (Vector3.forward * Time.deltaTime * 50);
		}
		if (leftswing) {
			transform.Rotate (Vector3.forward * Time.deltaTime * -50);
		}

		if (rightswing && rotationAlign > 80 && rotationAlign < 200) {
			rightswing = false;
			leftswing = true;
		}
		if (leftswing && rotationAlign < 280 && rotationAlign > 260) {
			leftswing = false;
			rightswing = true;
		}
	}
}
