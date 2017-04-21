using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour {

	List<int> activeControllers = new List<int>();
	bool allowSetAcive = true;
	bool gameRunning = false;
	
	// Update is called once per frame
	void Update () {
		if (allowSetAcive) {
			GetSetupInput();
		}
		if (gameRunning) {
			GetGameInput();
		}
	}

	void GetGameInput() {
		foreach (int i in activeControllers) {
			if (Input.GetButtonDown("Joy" + i + "Jump")) {
				Debug.Log("Jump down: " + i);
			}
			if (Input.GetButtonUp("Joy" + i + "Jump")) {
				Debug.Log("Jump up: " + i);
			}
			if (Mathf.Abs(Input.GetAxis("Joy" + i + "X")) > 0f) {
				Debug.Log("X-axis: " + i + " value: " + Input.GetAxis("Joy" + i + "X"));
			}
		}
	}

	void GetSetupInput() {
		for (int i = 1; i < 5; i++) {
			if (Input.GetButtonDown("Joy" + i + "Jump")) {
				if (activeControllers.Contains(i)) {
					activeControllers.Remove(i);
					Debug.Log("removed " + i);
				}
				else {
					activeControllers.Add(i);
					Debug.Log("added " + i);
				}
			}
		}
		if (Input.GetButtonDown("Start")) {
			if(activeControllers.Count == 0) {
				Debug.Log("needs atleast 1 to start!");
				return;
			}
			allowSetAcive = false;
			gameRunning = true;
			Debug.Log("Started game!");
		}
	}
}
