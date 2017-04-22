using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbySceneBackground : Singleton<LobbySceneBackground> {

	public GameObject[] avatars;
	public Transform[] spawnPoints;

	// Use this for initialization
	void Start () {
		SpawnNew();
	}
	
	public void GotKill() {
		SpawnNew();
	}

	public void SpawnNew() {
		Instantiate(avatars[Random.Range(0, avatars.Length)], spawnPoints[Random.Range(0,spawnPoints.Length)].position, Quaternion.identity);
	}
}
