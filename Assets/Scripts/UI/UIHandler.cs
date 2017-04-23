using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour {

	public PlayerPanel[] panels;
	public Text WonText;
	public Text powerUpText;
    public TimerBar timerBar;

	public void Setup(List<PlayerData> activePlayers) {
		foreach (var i in activePlayers) {
			panels[i.ID-1].Setup(i.color, i.ID);
			panels[i.ID-1].gameObject.SetActive(true);
		}
	}

	public void PlayerUsedFuckYou(int i, string name, Color col, int powerUpID, PlayerData player) {
        powerUpText.gameObject.SetActive(true);
        timerBar.ShowProgressBar(col);
		powerUpText.text = name + player.powerUp[powerUpID].displayText;
		StartCoroutine(Fade(powerUpText, 2, col));
		panels[i - 1].UsedFuckYou(powerUpID);
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

	public void Won(string name, Color col) {
		WonText.text = name + " WON!";
		WonText.color = col;
		WonText.gameObject.SetActive(true);
	}

	IEnumerator Fade(Text text, float time, Color col) {
		Outline outline = text.gameObject.GetComponent<Outline>();
		var startTime = time;
		var initCol = text.color = col;
		var outlineCol = Color.white;
		while (time > 0) {
			time -= Time.deltaTime;
			initCol.a = Mathf.Lerp(0, 1, time / startTime);
			outlineCol.a = initCol.a;
			outline.effectColor = outlineCol;
			text.color = initCol;
			yield return new WaitForEndOfFrame();
		}
		powerUpText.gameObject.SetActive(false);
	}
}
