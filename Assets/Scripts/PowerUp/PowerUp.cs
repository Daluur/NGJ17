using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PowerUp {

    public string displayText = "No Display text defined?!";
    public PlayerController currentPlayer;
    public float powerUpDuration = FuckYouConstants.DURATIONINSECONDS;

    public void SetCurrentPlayer(PlayerController currPlayer) {
        currentPlayer = currPlayer;
    }

    public virtual void UsePowerUp(PlayerController player) {
        Debug.LogError("Implementation missing for this power up");
    }
}
