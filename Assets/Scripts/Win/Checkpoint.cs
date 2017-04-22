using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {

	public GameObject red;
	public GameObject yellow;
	public GameObject green;
	public GameObject blue;

	private void OnTriggerEnter2D(Collider2D collision) {
		if(collision.gameObject.tag == "Player") {
			var i = GameHandler.instance.PlayerHitCheckpoint(collision.gameObject.GetComponent<PlayerController>());
			switch (i) {
			case 1:
				red.SetActive (true);
				break;
			case 2:
				yellow.SetActive (true);
				break;
			case 3:
				green.SetActive (true);
				break;
			case 4:
				blue.SetActive (true);
				break;
			default:
				break;
			}
		}
	}
}
