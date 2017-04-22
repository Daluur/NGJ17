using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : Singleton<InputController> {

	List<int> activeControllers = new List<int>();

	int currentPlayerID;
	PlayerController currentPlayer;

	public bool keyboardInput = false;
	bool startedInGameScene = false;
	[HideInInspector]
	public bool gameFinished = false;

	public void Setup(List<int> activeConts, bool startInScene = false) {
		activeControllers = activeConts;
		if (startInScene) {
			startedInGameScene = true;
			activeControllers = new List<int>() { 1, 2, 3, 4 };
		}
	}

	public void AssignNewPlayer(int id, PlayerController controller) {
		currentPlayerID = id;
		currentPlayer = controller;
	}
	
	// Update is called once per frame
	void Update () {
		if (gameFinished) {
			ListenForStartButton();
			return;
		}
		if (keyboardInput) {
			GetKeyboardInput();
		}
		else if(startedInGameScene){
			GetALLJoystickInput();
		}
		else {
			GetJoystickInput();
			GetNonActivePlayerInput();
		}
	}

	void GetJoystickInput() {
		if (Input.GetButtonDown("Joy" + currentPlayerID + "Jump")) {
			Debug.Log("Jump down: " + currentPlayerID);
		}
		if (Input.GetButtonUp("Joy" + currentPlayerID + "Jump")) {
			Debug.Log("Jump up: " + currentPlayerID);
		}
		currentPlayer.MoveDirection = Input.GetAxis("Joy" + currentPlayerID + "X");
	}

	void GetNonActivePlayerInput() {
		foreach (int i in activeControllers) {
			if (currentPlayerID == i) {
				continue;
			}
			if (Input.GetButtonDown("Joy" + i + "Jump")) {
				Debug.Log("Not active player: "+i+" Pressed fuck you button");
				if (GameHandler.instance.CanUseFuckYouPower(i)) {
					currentPlayer.Kill();
				}
			}
		}
	}

	void ListenForStartButton() {
		if (Input.GetButtonDown("Start")) {
			if (keyboardInput) {
				UnityEngine.SceneManagement.SceneManager.LoadScene(1);
			}
			else {
				GameHandler.instance.ReturnToCharSelect();
			}
		}
	}

	/// <summary>
	/// Only used when the game is started directly from the scene. Only for debug.
	/// </summary>
	void GetALLJoystickInput() {
		float moveDir = 0;
		foreach (int i in activeControllers) {
			if (currentPlayerID != i) {
				if (!startedInGameScene) {
					continue;
				}
			}
			if (Input.GetButtonDown("Joy" + i + "Jump")) {
				Debug.Log("Jump down: " + i);
			}
			if (Input.GetButtonUp("Joy" + i + "Jump")) {
				Debug.Log("Jump up: " + i);
			}
			moveDir += Input.GetAxis("Joy" + i + "X");
		}
		currentPlayer.MoveDirection = moveDir;
	}

	/// <summary>
	/// Using keyboard input, only for debug.
	/// </summary>
	void GetKeyboardInput() {
		if(currentPlayer == null) {
			return;
		}
		var direction = 0f;
		if (Input.GetKey(KeyCode.A)) {
			direction -= 1f;
		}
		if (Input.GetKey(KeyCode.D)) {
			direction += 1f;
		}
		currentPlayer.MoveDirection = direction;
	}

	/*foreach (int i in activeControllers) {
		if(currentPlayerID != i) {
			if (!startedInGameScene) {
				continue;
			}
		}
		if (Input.GetButtonDown("Joy" + i + "Jump")) {
			Debug.Log("Jump down: " + i);
		}
		if (Input.GetButtonUp("Joy" + i + "Jump")) {
			Debug.Log("Jump up: " + i);
		}
		currentPlayer.MoveDirection = Input.GetAxis("Joy" + i + "X");
	}*/
}
