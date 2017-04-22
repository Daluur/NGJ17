using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControllerAssigning : MonoBehaviour {

	List<int> activeControllers = new List<int>();
	public LobbyPlayer[] avatars;
	public GameObject readyToStart;
	int map = 1;

	private void Start() {
		ModeToggle(0);
	}

	// Update is called once per frame
	void Update () {
		GetSetupInput();
	}

	void GetSetupInput() {
		for (int i = 1; i < 5; i++) {
			if (Input.GetButtonDown("Joy" + i + "Jump")) {
				if (activeControllers.Contains(i)) {
					activeControllers.Remove(i);
					RemoveAvatar(i);
					Debug.Log("removed " + i);
				}
				else {
					activeControllers.Add(i);
					ShowAvatar(i);
					Debug.Log("added " + i);
				}
			}
		}
		if (Input.GetButtonDown("Start")) {
			if (activeControllers.Count == 0) {
				Debug.Log("needs atleast 1 to start!");
				return;
			}
			Debug.Log("Started game!");
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
}
