using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PowerUp {

    public GameObject currentPlayer;
    public float powerUpDuration = 5f;

    public void SetCurrentPlayer(GameObject currPlayer) {
        currentPlayer = currPlayer;
    }

    public virtual void UsePowerUp() {
        Debug.LogError("Implementation missing for this power up");
    }
}
