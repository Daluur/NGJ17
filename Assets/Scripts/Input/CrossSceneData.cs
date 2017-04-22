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
			playerInfo.Add(new PlayerData(i, colors[i-1],names[i-1]));
		}
	}	

	public List<PlayerData> GetPlayerData() {
		return playerInfo;
	}

	public Color[] colors = new Color[] { Color.red, Color.yellow, Color.green, Color.blue };
	public string[] names = new string[] { "Red", "Yellow", "Green", "Blue" };
}

public class PlayerData {
	public int ID;
	public bool[] usedFuckYouPower = new bool[3];
	public Color color;
    public PowerUp[] powerUp = new PowerUp[3];
	public string name;
    public AudioSource audioSource;

	public PlayerData(int i, Color col, string n) {
		ID = i;
		color = col;
		name = n;
        powerUp[0] = new IncreaseGravity();
        powerUp[1] = new ReverseMovement();
        powerUp[2] = new HidePaintFromPlayer();
    }
}