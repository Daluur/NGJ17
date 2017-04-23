using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideTheAvatar : PowerUp {
    bool isRunning;

    public HideTheAvatar()
    {
        displayText = " Hid your PLAYER!";
    }

    public HideTheAvatar(string displayText)
    {
        this.displayText = displayText;
    }

    public override void UsePowerUp(PlayerController player)
    {
        if (!isRunning)
            GameHandler.instance.StartCoroutine(TimedPowerUp(player));
    }

    public IEnumerator TimedPowerUp(PlayerController player) {
        isRunning = true;
		if(player == null) {
			yield break;
		}
        player.GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(FuckYouConstants.DURATIONINSECONDS);
        if(player!=null)
            player.GetComponent<SpriteRenderer>().enabled = true;
        isRunning = false;
    }

}
