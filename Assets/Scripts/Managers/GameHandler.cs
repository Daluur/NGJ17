using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameHandler : Singleton<GameHandler> {

	int currentPlayer;
	List<PlayerData> players;

	public GameObject[] avatars;

	public Transform spawnPoint;

	public GameObject UIObjectprefab;
	UIHandler ui;

	bool gameFinished = false;

	void Start() {
		if(CrossSceneData.Instance.GetActiveControllers() == null) {
			StartGame(new List<PlayerData>() { new PlayerData(1, Color.red, "RED") }, true);
		}
		else{
			StartGame(CrossSceneData.Instance.GetPlayerData());
		}
		ui = Instantiate(UIObjectprefab).GetComponent<UIHandler>();
		ui.Setup(players);
		ui.NewPlayersTurn(players[currentPlayer].ID);
	}

	public void StartGame(List<PlayerData> playerNums, bool faked = false) {
		players = playerNums;
		currentPlayer = 0;
		List<int> i = new List<int>();
		foreach (var player in players) {
			i.Add(player.ID);
		}
		InputController.instance.Setup(i, faked);
		SpawnPlayer();
	}

	public void PlayerGotKilled() {
		if (gameFinished) {
			return;
		}
		currentPlayer++;
		if(currentPlayer > players.Count - 1) {
			currentPlayer = 0;
		}
		SpawnPlayer();
		ui.NewPlayersTurn(players[currentPlayer].ID);
	}

	void SpawnPlayer() {
		GameObject temp = Instantiate(avatars[players[currentPlayer].ID-1], spawnPoint.position, Quaternion.identity) as GameObject;
		InputController.instance.AssignNewPlayer(players[currentPlayer].ID, temp.GetComponent<PlayerController>());
	}

	public void CanUseFuckYouPower(int id, PlayerController currentPlayer) {
		foreach (var p in players) {
			if(p.ID == id) {
				if (p.usedFuckYouPower) {
					return;
				}
				else {
					p.usedFuckYouPower = true;
					ui.PlayerUsedFuckYou(p.ID, p.name, p.color);
                    p.powerUp.UsePowerUp(currentPlayer);
					return;
				}
			}
		}
		return;
	}

	public void GameWon() {
		gameFinished = true;
		InputController.instance.gameFinished = true;
		ui.Won(players[currentPlayer].name);
	}

	bool returningToCharSelect = false;

	public void ReturnToCharSelect() {
		if (returningToCharSelect) {
			return;
		}
		returningToCharSelect = true;
		SceneManager.LoadScene(0);
	}

}