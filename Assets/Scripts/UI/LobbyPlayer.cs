using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyPlayer : MonoBehaviour {

	bool Assigned = false;
	public Color col;
	public GameObject joinText;

	public void Assign() {
		Assigned = true;
		GetComponent<Image>().color = col;
		joinText.SetActive(false);
	}

	public void UnAssign() {
		Assigned = false;
		GetComponent<Image>().color = Color.black;
		joinText.SetActive(true);
	}
}
