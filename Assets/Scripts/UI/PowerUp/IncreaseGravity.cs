using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseGravity : PowerUp {
    public float gravityModifier = FuckYouConstants.GRAVITYMODIFIER;
    public override void UsePowerUp()
    {
        GameHandler.instance.StartCoroutine(GravityModifier());
    }

    IEnumerator GravityModifier() {
        var currPlayerRigid = currentPlayer.GetComponent<Rigidbody2D>();
        var prevGravity = currPlayerRigid.gravityScale;
        currPlayerRigid.gravityScale *= gravityModifier;
        yield return new WaitForSeconds(powerUpDuration);
        if(currPlayerRigid!=null)
            currPlayerRigid.gravityScale = prevGravity;
    }
}
