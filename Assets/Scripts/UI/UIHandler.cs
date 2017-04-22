using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHandler : MonoBehaviour {

	public PlayerPanel[] panels;

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
}
