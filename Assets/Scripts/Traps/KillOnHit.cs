using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillOnHit : MonoBehaviour {
    public AudioClip environmentalSound;
    public AudioClip deathAudio;
	private void OnCollisionEnter2D(Collision2D collision) {
		if(collision.gameObject.tag == "Player") {
            GameHandler.instance.MuteCurrentPlayerMusic();
            collision.gameObject.GetComponent<PlayerController>().Kill();
            if (deathAudio != null) {
                GetComponent<AudioSource>().clip = deathAudio;
                GetComponent<AudioSource>().loop = false;
                GetComponent<AudioSource>().Play();
            }
		}
	}
    private void Update()
    {
        if (!GetComponent<AudioSource>().isPlaying && environmentalSound!=null) {
            GetComponent<AudioSource>().clip = environmentalSound;
            GetComponent<AudioSource>().loop = true;
            GetComponent<AudioSource>().Play();
        }
    }
}
