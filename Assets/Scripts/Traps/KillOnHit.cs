using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillOnHit : MonoBehaviour {
    public AudioClip environmentalSound;
    public AudioClip deathAudio;
    private AudioSource source;
    public float maxDistanceDeath = 1000f;
    public float maxDistanceEnironment = 6f;

	private void OnCollisionEnter2D(Collision2D collision) {
		if(collision.gameObject.tag == "Player") {
			if (GameHandler.instance != null) {
				GameHandler.instance.MuteCurrentPlayerMusic();
			}
			collision.gameObject.GetComponent<PlayerController>().Kill();
			if (source == null) {
				return;
			}
            if (deathAudio != null) {
                source.maxDistance = maxDistanceDeath;
                source.clip = deathAudio;
                source.loop = false;
                source.Play();
            }
		}
	}

    private void Start()
    {
        source = GetComponent<AudioSource>();
    }

    private void Update()
    {
		if (source == null) {
			return;
		}
        if (!source.isPlaying && environmentalSound!=null) {
            source.maxDistance = maxDistanceEnironment;
            source.clip = environmentalSound;
            source.loop = true;
            source.Play();
        }
    }
}
