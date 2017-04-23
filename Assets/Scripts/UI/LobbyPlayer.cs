using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyPlayer : MonoBehaviour {

	public Color col;
	public Text joinText;

	public void SetInputTypeAsController() {
		joinText.text = "Press 'A' to join!";
	}

	public void SetInputTypeAsKeyboard(int i) {
		switch (i) {
			case 0:
				joinText.text = "Press 'W' to join!";
				break;
			case 1:
				joinText.text = "Press 'I' to join!";
				break;
			case 2:
				char arrow = '\u25B2';
				joinText.text = "Press "+arrow+" to join!";
				break;
			case 3:
				joinText.text = "Press '8' to join!";
				break;
			default:
				break;
		}
	}

	public void Assign() {
		GetComponent<Image>().color = col;
		joinText.gameObject.SetActive(false);
	}

	public void UnAssign() {
		GetComponent<Image>().color = Color.white;
		joinText.gameObject.SetActive(true);
	}
}
