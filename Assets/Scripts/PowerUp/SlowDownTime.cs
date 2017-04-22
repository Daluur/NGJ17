using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowDownTime : PowerUp {
    // Update is called once per frame
    float factor = FuckYouConstants.SLOWDOWNFACTOR;
    float prevAirSpeed, prevGroundSpeed, prevGroundAcceleration; //prevWallJumpSpeed;
    private bool isRunning;
    public override void UsePowerUp(PlayerController player)
    {
        if(!isRunning)
            GameHandler.instance.StartCoroutine(TimedPowerUp(player));
    }
    public IEnumerator TimedPowerUp(PlayerController player) {
        isRunning = true;
        prevAirSpeed = player.airSpeed;
        prevGroundSpeed = player.groundSpeed;
        prevGroundAcceleration = player.groundAcceleration;
        //prevWallJumpSpeed = player.wallJumpSpeed;
        //player.airAcceleration *= factor;
        player.airSpeed *= factor;
        //player.groundJumpSpeed *= factor;
        player.groundSpeed *= factor;
        player.groundAcceleration *= factor;
        //player.wallJumpSpeed *= factor;
        yield return new WaitForSeconds(FuckYouConstants.DURATIONINSECONDS);
        if (player != null) {
            player.airSpeed = prevAirSpeed;
            player.groundSpeed = prevGroundSpeed;
            player.groundAcceleration = prevGroundAcceleration;
        }
        isRunning = false;
    }
}
