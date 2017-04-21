using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : Singleton<GameHandler> {

	int currentPlayer;
	List<int> players;

	public GameObject[] avatars;

	public Transform spawnPoint;

	void Start() {
		if(CrossSceneData.Instance.GetActiveControllers() == null) {
			StartGame(new List<int>() { 1 }, true);
		}
		else{
			StartGame(CrossSceneData.Instance.GetActiveControllers());
		}
	}

	public void StartGame(List<int> playerNums, bool faked = false) {
		players = playerNums;
		currentPlayer = 0;
		InputController.instance.Setup(players, faked);
		SpawnPlayer();
	}

	public void PlayerGotKilled() {
		currentPlayer++;
		if(currentPlayer > players.Count - 1) {
			currentPlayer = 0;
		}
		SpawnPlayer();
	}

	void SpawnPlayer() {
		GameObject temp = Instantiate(avatars[players[currentPlayer]-1], spawnPoint.position, Quaternion.identity) as GameObject;
		InputController.instance.AssignNewPlayer(players[currentPlayer], temp.GetComponent<PlayerController>());
	}

}
