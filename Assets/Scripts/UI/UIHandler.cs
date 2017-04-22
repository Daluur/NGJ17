using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour {

	public PlayerPanel[] panels;
	public Text WonText;

	public void Setup(List<PlayerData> activePlayers) {
		foreach (var i in activePlayers) {
			panels[i.ID-1].Setup(i.color, i.ID);
			panels[i.ID-1].gameObject.SetActive(true);
		}
	}

	public void PlayerUsedFuckYou(int i) {
		panels[i - 1].UsedFuckYou();
	}

	public void NewPlayersTurn(int i) {
		for (int j = 0; j < panels.Length; j++) {
			if(j+1 == i) {
				panels[j].SetCurrentTurn();
			}
			else {
				panels[j].SetNotCurrentTurn();
			}
		}
	}

	public void Won(int id) {
		WonText.text = "PLAYER " + id + " WON!";
		WonText.gameObject.SetActive(true);
	}
}
