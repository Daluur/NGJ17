using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Win : MonoBehaviour {

	private void OnCollisionEnter2D(Collision2D collision) {
		if (collision.gameObject.tag == "Player") {
			GameHandler.instance.GameWon();
			collision.gameObject.GetComponent<PlayerController>().Kill();
		}
	}
}
