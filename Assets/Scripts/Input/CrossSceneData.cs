using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossSceneData {

	private static CrossSceneData instance;

	private CrossSceneData() { }

	public static CrossSceneData Instance {
		get {
			if (instance == null) {
				instance = new CrossSceneData();
			}
			return instance;
		}
	}

	List<PlayerData> playerInfo = new List<PlayerData>();

	List<int> activeControllers;

	public List<int> GetActiveControllers() {
		return activeControllers;
	}

	public void SetActiveControllers(List<int> conts) {
		activeControllers = conts;
		playerInfo.Clear();
		foreach (int i in activeControllers) {
			playerInfo.Add(new PlayerData(i, colors[i-1]));
		}
	}	

	public List<PlayerData> GetPlayerData() {
		return playerInfo;
	}

	public Color[] colors = new Color[] { Color.red, Color.yellow, Color.green, Color.blue };
}

public class PlayerData {
	public int ID;
	public bool usedFuckYouPower = false;
	public Color color;
    public PowerUp powerUp;

	public PlayerData(int i, Color col) {
		ID = i;
		color = col;
        powerUp = new IncreaseGravity();
	}
}