using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ControllerAssigning : MonoBehaviour {

	List<int> activeControllers = new List<int>();
	public LobbyPlayer[] avatars;
	public GameObject readyToStart;
	int map = 1;

	bool controller = true;

	private void Start() {
		ModeToggle(0);
	}

	// Update is called once per frame
	void Update () {
		if (controller) {
			GetSetupInput();
		}
		else {
			GetKeyboardInput();
		}
	}

	void GetSetupInput() {
		for (int i = 1; i < 5; i++) {
			if (Input.GetButtonDown("Joy" + i + "Jump")) {
				if (activeControllers.Contains(i)) {
					activeControllers.Remove(i);
					RemoveAvatar(i);
				}
				else {
					activeControllers.Add(i);
					ShowAvatar(i);
				}
			}
		}
		if (Input.GetButtonDown("Start")) {
			if (activeControllers.Count == 0) {
				Debug.Log("needs atleast 1 to start!");
				return;
			}
			activeControllers.Sort();
			CrossSceneData.Instance.SetActiveControllers(activeControllers);
			SceneManager.LoadScene(map);
		}
	}

	void GetKeyboardInput() {
		if (Input.GetKeyDown(KeyCode.W)) {
			if (activeControllers.Contains(1)) {
				activeControllers.Remove(1);
				RemoveAvatar(1);
			}
			else {
				activeControllers.Add(1);
				ShowAvatar(1);
			}
		}
		if (Input.GetKeyDown(KeyCode.I)) {
			if (activeControllers.Contains(2)) {
				activeControllers.Remove(2);
				RemoveAvatar(2);
			}
			else {
				activeControllers.Add(2);
				ShowAvatar(2);
			}
		}
		if (Input.GetKeyDown(KeyCode.UpArrow)) {
			if (activeControllers.Contains(3)) {
				activeControllers.Remove(3);
				RemoveAvatar(3);
			}
			else {
				activeControllers.Add(3);
				ShowAvatar(3);
			}
		}
		if (Input.GetKeyDown(KeyCode.Keypad8)) {
			if (activeControllers.Contains(4)) {
				activeControllers.Remove(4);
				RemoveAvatar(4);
			}
			else {
				activeControllers.Add(4);
				ShowAvatar(4);
			}
		}
		if (Input.GetButtonDown("Start")) {
			if (activeControllers.Count == 0) {
				Debug.Log("needs atleast 1 to start!");
				return;
			}
			activeControllers.Sort();
			CrossSceneData.Instance.SetActiveControllers(activeControllers);
			SceneManager.LoadScene(map);
		}
	}

	void ShowAvatar(int id) {
		avatars[id-1].Assign();
		if(activeControllers.Count > 0) {
			readyToStart.SetActive(true);
		}
	}

	void RemoveAvatar(int id) {
		avatars[id-1].UnAssign();
		if (activeControllers.Count == 0) {
			readyToStart.SetActive(false);
		}
	}

	public void ModeToggle(int i) {
		if (i == 0) {
			CrossSceneData.Instance.simultaneous = false;
			CrossSceneData.Instance.FFA = false;
		}
		else if(i == 1) {
			CrossSceneData.Instance.simultaneous = true;
			CrossSceneData.Instance.FFA = false;
		}
		else if (i == 2) {
			CrossSceneData.Instance.simultaneous = false;
			CrossSceneData.Instance.FFA = true;
		}
	}

	public void MapToggle(int i) {
		map = i+1;
	}

	public void ChangeInputDevice(int i) {
		if(i == 0) {
			controller = true;
			CrossSceneData.Instance.controller = true;
		}
		else if(i == 1) {
			controller = false;
			CrossSceneData.Instance.controller = false;
		}
		ChangeTexts();
	}

	void ChangeTexts() {
		if (controller) {
			for (int i = 0; i < avatars.Length; i++) {
				avatars[i].SetInputTypeAsController();
			}
			readyToStart.GetComponent<Text>().text = "Press 'Start' to start!";
		}
		else {
			for (int i = 0; i < avatars.Length; i++) {
				avatars[i].SetInputTypeAsKeyboard(i);
			}
			readyToStart.GetComponent<Text>().text = "Press 'Enter' to start!";
		}
	}

	public void Quit() {
		Application.Quit();
	}
}
