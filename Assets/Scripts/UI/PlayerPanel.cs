﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPanel : MonoBehaviour {

	public Image icon;
	public Text numberText;
	public GameObject currentTurn;
	public GameObject hasFuckYou;

	public void Setup(Color col, int number) {
		numberText.text = number.ToString();
		icon.color = col;
	}

	public void SetCurrentTurn() {
		currentTurn.SetActive(true);
	}

	public void SetNotCurrentTurn() {
		currentTurn.SetActive(false);
	}

	public void UsedFuckYou() {
		hasFuckYou.SetActive(false);
	}
}
