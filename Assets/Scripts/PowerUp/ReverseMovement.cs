using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReverseMovement : PowerUp {
    bool isRunning;

    public ReverseMovement() {
        displayText = " Reversed somethings?";
    }

    public ReverseMovement(string displayText) {
        this.displayText = displayText;
    }


    public override void UsePowerUp(PlayerController player)
    {
        if (!isRunning)
            GameHandler.instance.StartCoroutine(TimedPowerUp());
    }
    public IEnumerator TimedPowerUp() {
        isRunning = true;
        InputController.instance.reversePowerUpVar = -1f;
        yield return new WaitForSeconds(FuckYouConstants.DURATIONINSECONDS);
        InputController.instance.reversePowerUpVar = 1f;
        isRunning = false;
    }
}
