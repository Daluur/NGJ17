using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameHandler : Singleton<GameHandler> {

	int currentPlayer;
	List<PlayerData> players;

	public GameObject[] avatars;
    public GameObject audioFollowListener;

    public float standardBGVolume = 0.3f;

    private PlayerData previousPlayer;
    private AudioSource[] audioSources;

    private AudioSource otherModeAudioSourceBG;
    public AudioClip ffaBG;
    public AudioClip powerUpClip;

	public Transform spawnPoint;
	public Transform checkpoint;

	List<int> playerReachedCheckpoint = new List<int>();

	public GameObject UIObjectprefab;
	UIHandler ui;

	bool gameFinished = false;
	bool simultaneous;
	bool FFA;

	int alivePlayers = 0;

    protected override void Awake()
    {
        Instantiate(audioFollowListener);
        base.Awake();
    }

    void Start() {
        if (CrossSceneData.Instance.GetActiveControllers() == null) {
			StartGame(new List<PlayerData>() { new PlayerData(1, Color.red, "RED") }, true, false, false);
		}
		else{
			StartGame(CrossSceneData.Instance.GetPlayerData(),false , CrossSceneData.Instance.simultaneous, CrossSceneData.Instance.FFA);
		}
		ui = Instantiate(UIObjectprefab).GetComponent<UIHandler>();
		ui.Setup(players);
		if (!simultaneous && !FFA) { 
			ui.NewPlayersTurn(players[currentPlayer].ID);
		}
	}

    void InitAudio() {
        if (simultaneous || FFA)
        {
            otherModeAudioSourceBG.clip = ffaBG;
            otherModeAudioSourceBG.Play();
            otherModeAudioSourceBG.volume = standardBGVolume;
            return;
        }

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

    public void StartGame(List<PlayerData> playerNums, bool faked = false, bool sim = false, bool ffa = false) {
        
        simultaneous = sim;
		FFA = ffa;
		players = playerNums;
		currentPlayer = 0;
		List<int> i = new List<int>();
		foreach (var player in players) {
			i.Add(player.ID);
		}
		InputController.instance.Setup(i, faked, simultaneous || FFA);
        InitAudio();
		if (!simultaneous && !FFA) {
			SpawnPlayer();
		}
		else if(simultaneous) {
			SimultaneousSpawn();
		}
		else if (FFA) {
			FFASpawn();
		}
		else {
			Debug.LogError("No Mode selected!");
		}
	}

    public void MuteCurrentPlayerMusic() {
        if (!simultaneous && !FFA)
        {
            players[currentPlayer].audioSource.volume = 0;
        }
    }

	public void PlayerGotKilled(PlayerController cont) {
		alivePlayers--;
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
		if (!simultaneous && !FFA) {
			SpawnPlayer();
		}
		else if (simultaneous && alivePlayers == 0) {
			SimultaneousSpawn();
		}
		else if (FFA) {
			StartCoroutine(SpawnPlayerByID(InputController.instance.GetIDFromController(cont)));
		}
		if (!simultaneous && !FFA) {
			ui.NewPlayersTurn(players[currentPlayer].ID);
		}
	}

	void SpawnPlayer() {
		GameObject temp = Instantiate(avatars[players[currentPlayer].ID-1], playerReachedCheckpoint.Contains(players[currentPlayer].ID) ? checkpoint.position : spawnPoint.position, Quaternion.identity) as GameObject;
		if (simultaneous || FFA) {
			Destroy(temp.GetComponent<AudioListener>());
		}
		InputController.instance.AssignNewPlayer(players[currentPlayer].ID, temp.GetComponent<PlayerController>());
        if(!simultaneous && !FFA) { 
            if (previousPlayer != null)
                previousPlayer.audioSource.volume = 0;
            players[currentPlayer].audioSource.volume = standardBGVolume;
        }
        previousPlayer = players[currentPlayer];
		alivePlayers++;
    }

	IEnumerator SpawnPlayerByID(int id) {
		yield return new WaitForSeconds(values.respawnTimerFFAMode);
		GameObject temp = Instantiate(avatars[id - 1], playerReachedCheckpoint.Contains(players[currentPlayer].ID) ? checkpoint.position : spawnPoint.position, Quaternion.identity) as GameObject;
        if (!simultaneous && !FFA)
        {
            InputController.instance.AssignNewPlayer(id, temp.GetComponent<PlayerController>());
            if (previousPlayer != null)
                previousPlayer.audioSource.volume = 0;
            players[currentPlayer].audioSource.volume = standardBGVolume;
        }
		previousPlayer = players[currentPlayer];
		alivePlayers++;
	}

	void SimultaneousSpawn() {
		currentPlayer = 0;
		foreach (PlayerData i in players) {
			SpawnPlayer();
			currentPlayer++;
		}
	}

	void FFASpawn() {
		currentPlayer = 0;
		foreach (PlayerData i in players) {
			SpawnPlayer();
			currentPlayer++;
		}
	}

    public void CanUseFuckYouPower(int id, PlayerController currentPlayer, int powerUpID)
    {
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
                    var source = Camera.main.GetComponent<AudioSource>();
                    source.clip = powerUpClip;
                    source.Play();
					return;
				}
			}
		}
		return;
	}

	public void GameWon(int id) {
		gameFinished = true;
		InputController.instance.gameFinished = true;
		foreach (PlayerData data in players) {
			if(data.ID == id) {
				ui.Won(data.name);
				return;
			}
		}
	}

	bool returningToCharSelect = false;

	public void ReturnToCharSelect() {
		if (returningToCharSelect) {
			return;
		}
		returningToCharSelect = true;
		SceneManager.LoadScene(0);
	}

	public void PlayerHitCheckpoint(PlayerController cont) {
		int i = InputController.instance.GetIDFromController(cont);
		if (!playerReachedCheckpoint.Contains(i)) {
			playerReachedCheckpoint.Add(i);
		}
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
		if (GameHandler.instance == null) {
			LobbySceneBackground.instance.StartCoroutine(CoolDown());
		}
		else {
			GameHandler.instance.StartCoroutine(CoolDown());
		}
        return currentIncrement;
    }
    public static IEnumerator CoolDown() {
        yield return new WaitForSeconds(ParticleSystemConstants.TIMEBEFOREDESTRUCTION);
        locked = false;
    }
}