using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControllerAssigning : MonoBehaviour {

	List<int> activeControllers = new List<int>();
	public GameObject[] avatars;

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
			SceneManager.LoadScene(1);
		}
	}

	void ShowAvatar(int id) {
		avatars[id-1].SetActive(true);
	}

	void RemoveAvatar(int id) {
		avatars[id-1].SetActive(false);
	}
}
