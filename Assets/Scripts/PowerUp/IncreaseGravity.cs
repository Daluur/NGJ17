﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseGravity : PowerUp {
    public float gravityModifier = FuckYouConstants.GRAVITYMODIFIER;

    public IncreaseGravity()
    {
        displayText = " Altered gravity!";
    }

    public IncreaseGravity(string displayText)
    {
        this.displayText = displayText;
    }

    public override void UsePowerUp(PlayerController player)
    {
		SetCurrentPlayer(player);
        GameHandler.instance.StartCoroutine(GravityModifier());
    }

    public IEnumerator GravityModifier() {
		if(currentPlayer == null) {
			yield break;
		}
        var currPlayerRigid = currentPlayer.GetComponent<Rigidbody2D>();
        var prevGravity = currPlayerRigid.gravityScale;
        currPlayerRigid.gravityScale *= gravityModifier;
        yield return new WaitForSeconds(powerUpDuration);
        if(currPlayerRigid!=null)
            currPlayerRigid.gravityScale = prevGravity;
    }
}
