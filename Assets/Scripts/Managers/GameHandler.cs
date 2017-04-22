using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameHandler : Singleton<GameHandler> {

	int currentPlayer;
	List<PlayerData> players;

	public GameObject[] avatars;

    public float standardBGVolume = 0.5f;

    private PlayerData previousPlayer;
    private AudioSource[] audioSources;

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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O)) {
            PlayerGotKilled();
        }
    }

    void InitAudio() {
        audioSources = GetComponents<AudioSource>();
        int i = 0;
        foreach (var player in players)
        {
            player.audioSource = audioSources[i];
            player.audioSource.clip = avatars[i].GetComponent<SoundHolder>().playerBG;
            player.audioSource.Play();
            player.audioSource.volume = 0;
            i++;
        }
    }

    public void StartGame(List<PlayerData> playerNums, bool faked = false) {
		players = playerNums;
		currentPlayer = 0;
		List<int> i = new List<int>();
		foreach (var player in players) {
			i.Add(player.ID);
		}
		InputController.instance.Setup(i, faked);
        InitAudio();
        SpawnPlayer();
	}

    public void MuteCurrentPlayerMusic() {
        players[currentPlayer].audioSource.volume = 0;
    }

	public void PlayerGotKilled() {
		if (Camera.main.GetComponent<Shaker>() != null) {
			Camera.main.GetComponent<Shaker>().DoShake();
		}
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
        if (previousPlayer != null)
            previousPlayer.audioSource.volume = 0;
        players[currentPlayer].audioSource.volume = standardBGVolume;
       
        previousPlayer = players[currentPlayer];
    }

	public void CanUseFuckYouPower(int id, PlayerController currentPlayer, int powerUpID) {
		foreach (var p in players) {
			if(p.ID == id) {
				if (p.usedFuckYouPower[powerUpID]) {
					return;
				}
				else {
					p.usedFuckYouPower[powerUpID] = true;
					ui.PlayerUsedFuckYou(p.ID, p.name, p.color,powerUpID, p);
                    //TODO: Add FUCK YOU power up sound when used
                    p.powerUp[powerUpID].UsePowerUp(currentPlayer);
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

public static class IncrementerForParticles {
    private static int currentIncrement = 1;
    private static bool locked;
    public static int GetCurrentAndIncrement()
    {
        if (locked)
            return currentIncrement;
        locked = true;
        currentIncrement++;
        GameHandler.instance.StartCoroutine(CoolDown());
        return currentIncrement;
    }
    public static IEnumerator CoolDown() {
        yield return new WaitForSeconds(ParticleSystemConstants.TIMEBEFOREDESTRUCTION);
        locked = false;
    }
}