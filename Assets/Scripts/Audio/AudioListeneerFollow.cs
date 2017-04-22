using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioListeneerFollow : Singleton<AudioListeneerFollow> {

    private GameObject playerToFollow;

	// Update is called once per frame
	void Update () {
		if(playerToFollow == null) {
			return;
		}
        transform.position = playerToFollow.transform.position;
	}

    public void SetPlayerToFollow(GameObject playerToFollow) {
        this.playerToFollow = playerToFollow;
    }
}
