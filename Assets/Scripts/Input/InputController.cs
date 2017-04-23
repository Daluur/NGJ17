using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : Singleton<InputController> {

	List<int> activeControllers = new List<int>();

	bool simultaneous;

	int currentPlayerID;
	Dictionary<int, PlayerController> currentPlayer = new Dictionary<int, PlayerController>();
    [HideInInspector]
    public float reversePowerUpVar = 1f;

	public bool keyboardInput = false;
	bool multiplayerKeyboardInput = false;
	bool startedInGameScene = false;
	[HideInInspector]
	public bool gameFinished = false;

	public void Setup(List<int> activeConts, bool startInScene = false, bool sim = false) {
		simultaneous = sim;
		activeControllers = activeConts;
		if (startInScene) {
			startedInGameScene = true;
			activeControllers = new List<int>() { 1, 2, 3, 4 };
		}
		else {
			multiplayerKeyboardInput = !CrossSceneData.Instance.controller;
			startedInGameScene = false;
			keyboardInput = false;
		}
	}

	public void AssignNewPlayer(int id, PlayerController controller) {
		currentPlayerID = id;
		currentPlayer[id] = controller;
	}
	
	// Update is called once per frame
	void Update () {
		if(currentPlayer.Count == 0) {
			return;
		}
		if (gameFinished) {
			ListenForStartButton();
			return;
		}
		if (keyboardInput) {
			GetKeyboardInput();
		}
		else if (startedInGameScene) {
			GetALLJoystickInput();
		}
		else if (simultaneous) {
			if (multiplayerKeyboardInput) {
				GetMultipleKeyboardInput();
			}
			else {
				GetMultipleJoystickInput();
			}
		}
		else if (multiplayerKeyboardInput) {
			GetMultipleKeyboardInput();
			if (!simultaneous) {
				GetNonActiveMultiplayerKeyboardInput();
			}
		}
		else {
			GetJoystickInput();
			GetNonActivePlayerInput();
		}
	}

	void GetJoystickInput() {
		if (Input.GetButtonDown("Joy" + currentPlayerID + "Jump")) {
			currentPlayer[currentPlayerID].Jumping = true;
		}
		if (Input.GetButtonUp("Joy" + currentPlayerID + "Jump")) {
			currentPlayer[currentPlayerID].Jumping = false;
		}
		currentPlayer[currentPlayerID].MoveDirection = Input.GetAxis("Joy" + currentPlayerID + "X") * reversePowerUpVar;
	}

	void GetNonActivePlayerInput() {
		foreach (int i in activeControllers) {
			if (currentPlayerID == i) {
				continue;
			}
            if (Input.GetButtonDown("Joy" + i + "LeftButton")) {
                GameHandler.instance.CanUseFuckYouPower(i, currentPlayer[currentPlayerID], 0);
            }
            if (Input.GetButtonDown("Joy" + i + "UpperButton"))
            {
                GameHandler.instance.CanUseFuckYouPower(i, currentPlayer[currentPlayerID], 1);
            }
            if (Input.GetButtonDown("Joy" + i + "RightButton"))
            {
                GameHandler.instance.CanUseFuckYouPower(i, currentPlayer[currentPlayerID], 2);
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
				currentPlayer[currentPlayerID].Jumping = true;
			}
			if (Input.GetButtonUp("Joy" + i + "Jump")) {
				currentPlayer[currentPlayerID].Jumping = false;
			}
            moveDir += Input.GetAxis("Joy" + i + "X");
		}
		currentPlayer[currentPlayerID].MoveDirection = moveDir;
	}

	void GetMultipleJoystickInput() {
		foreach (int i in activeControllers) {
			if (!currentPlayer.ContainsKey(i)) {
				continue;
			}
			if (Input.GetButtonDown("Joy" + i + "Jump")) {
				currentPlayer[i].Jumping = true;
			}
			if (Input.GetButtonUp("Joy" + i + "Jump")) {
				currentPlayer[i].Jumping = false;
			}
			currentPlayer[i].MoveDirection = Input.GetAxis("Joy" + i + "X");
		}
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
       /*if (Input.GetKeyDown(KeyCode.U)) {
            PowerUp pu = new ReverseMovement();
            pu.UsePowerUp(currentPlayer);
        }*/
		currentPlayer[currentPlayerID].MoveDirection = direction * reversePowerUpVar;
        currentPlayer[currentPlayerID].Jumping = Input.GetKey(KeyCode.W);
	}

	void GetMultipleKeyboardInput() {
		if (activeControllers.Contains(1) && currentPlayer.ContainsKey(1)) {
			var direction = 0f;
			if (Input.GetKey(KeyCode.A)) {
				direction -= 1f;
			}
			if (Input.GetKey(KeyCode.D)) {
				direction += 1f;
			}
			currentPlayer[1].MoveDirection = direction * reversePowerUpVar;
			currentPlayer[1].Jumping = Input.GetKey(KeyCode.W);
		}
		if (activeControllers.Contains(2) && currentPlayer.ContainsKey(2)) {
			var direction = 0f;
			if (Input.GetKey(KeyCode.J)) {
				direction -= 1f;
			}
			if (Input.GetKey(KeyCode.L)) {
				direction += 1f;
			}
			currentPlayer[2].MoveDirection = direction * reversePowerUpVar;
			currentPlayer[2].Jumping = Input.GetKey(KeyCode.I);
		}
		if (activeControllers.Contains(3) && currentPlayer.ContainsKey(3)) {
			var direction = 0f;
			if (Input.GetKey(KeyCode.LeftArrow)) {
				direction -= 1f;
			}
			if (Input.GetKey(KeyCode.RightArrow)) {
				direction += 1f;
			}
			currentPlayer[3].MoveDirection = direction * reversePowerUpVar;
			currentPlayer[3].Jumping = Input.GetKey(KeyCode.UpArrow);
		}
		if (activeControllers.Contains(4) && currentPlayer.ContainsKey(4)) {
			var direction = 0f;
			if (Input.GetKey(KeyCode.Keypad4)) {
				direction -= 1f;
			}
			if (Input.GetKey(KeyCode.Keypad6)) {
				direction += 1f;
			}
			currentPlayer[4].MoveDirection = direction * reversePowerUpVar;
			currentPlayer[4].Jumping = Input.GetKey(KeyCode.Keypad8);
		}
	}

	void GetNonActiveMultiplayerKeyboardInput() {
		if (activeControllers.Contains(1) && currentPlayerID != 1) {
			if (Input.GetKeyDown(KeyCode.Q)) {
				GameHandler.instance.CanUseFuckYouPower(1, currentPlayer[currentPlayerID], 0);
			}
			if (Input.GetKeyDown(KeyCode.S)) {
				GameHandler.instance.CanUseFuckYouPower(1, currentPlayer[currentPlayerID], 1);
			}
			if (Input.GetKeyDown(KeyCode.E)) {
				GameHandler.instance.CanUseFuckYouPower(1, currentPlayer[currentPlayerID], 2);
			}
		}
		if (activeControllers.Contains(2) && currentPlayerID != 2) {
			if (Input.GetKeyDown(KeyCode.U)) {
				GameHandler.instance.CanUseFuckYouPower(2, currentPlayer[currentPlayerID], 0);
			}
			if (Input.GetKeyDown(KeyCode.K)) {
				GameHandler.instance.CanUseFuckYouPower(2, currentPlayer[currentPlayerID], 1);
			}
			if (Input.GetKeyDown(KeyCode.O)) {
				GameHandler.instance.CanUseFuckYouPower(2, currentPlayer[currentPlayerID], 2);
			}
		}
		if (activeControllers.Contains(3) && currentPlayerID != 3) {
			if (Input.GetKeyDown(KeyCode.RightControl)) {
				GameHandler.instance.CanUseFuckYouPower(3, currentPlayer[currentPlayerID], 0);
			}
			if (Input.GetKeyDown(KeyCode.Minus)) {
				GameHandler.instance.CanUseFuckYouPower(3, currentPlayer[currentPlayerID], 1);
			}
			if (Input.GetKeyDown(KeyCode.RightShift)) {
				GameHandler.instance.CanUseFuckYouPower(3, currentPlayer[currentPlayerID], 2);
			}
		}
		if (activeControllers.Contains(4) && currentPlayerID != 4) {
			if (Input.GetKeyDown(KeyCode.Keypad7)) {
				GameHandler.instance.CanUseFuckYouPower(4, currentPlayer[currentPlayerID], 0);
			}
			if (Input.GetKeyDown(KeyCode.Keypad5)) {
				GameHandler.instance.CanUseFuckYouPower(4, currentPlayer[currentPlayerID], 1);
			}
			if (Input.GetKeyDown(KeyCode.Keypad9)) {
				GameHandler.instance.CanUseFuckYouPower(4, currentPlayer[currentPlayerID], 2);
			}
		}
	}

	public int GetIDFromController(PlayerController cont) {
		foreach (KeyValuePair<int,PlayerController> item in currentPlayer) {
			if(item.Value == cont) {
				return item.Key;
			}
		}
		return 1;
	}
}
