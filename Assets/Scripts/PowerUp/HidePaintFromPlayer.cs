using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class HidePaintFromPlayer : PowerUp {
    bool isRunning;
    public override void UsePowerUp(PlayerController player)
    {
        if(!isRunning)
            GameHandler.instance.StartCoroutine(TimedPowerUp());
    }

    public IEnumerator TimedPowerUp() {
        isRunning = true;
        var cams = GameObject.FindObjectsOfType<Camera>().ToList();
        foreach (var cam in cams) {
            if (!cam.Equals(Camera.main)) {
                cam.gameObject.SetActive(false);
            }
        }
        yield return new WaitForSeconds(FuckYouConstants.DURATIONINSECONDS);
        foreach (var cam in cams)
        {
            if (!cam.Equals(Camera.main))
            {
                cam.gameObject.SetActive(true);
            }
        }
        isRunning = false;
    }
}
