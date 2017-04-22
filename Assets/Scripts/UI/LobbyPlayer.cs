using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyPlayer : MonoBehaviour {

	public Color col;
	public GameObject joinText;

	public void Assign() {
		GetComponent<Image>().color = col;
		joinText.SetActive(false);
	}

	public void UnAssign() {
		GetComponent<Image>().color = Color.black;
		joinText.SetActive(true);
	}
}
