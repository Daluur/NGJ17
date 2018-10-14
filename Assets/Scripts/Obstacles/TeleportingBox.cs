using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportingBox : MonoBehaviour {

	public int length;
	public int speed;
	float initialX;
	float endX;
	public float delayMS;
	bool started = false;

	// Use this for initialization
	void Start()
	{
		initialX = transform.position.x;
		endX = initialX + length;
	}

	// Update is called once per frame
	void Update()
	{
		if (!started)
		{
			delayMS -= Time.deltaTime;
			if (delayMS < 0)
			{
				started = true;
			}
		}
		if (started) {
			transform.position = new Vector3(transform.position.x + (speed * Time.deltaTime), transform.position.y, transform.position.z);
			if (transform.position.x > endX)
			{
				transform.position = new Vector3(initialX, transform.position.y, transform.position.z);
			}
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Player")
		{
			if (collision.contacts[0].normal == Vector2.down)
			{
				collision.transform.parent = transform;
			}
		}
	}

	private void OnCollisionExit2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Player")
		{
			collision.transform.parent = null;
		}
	}
}
