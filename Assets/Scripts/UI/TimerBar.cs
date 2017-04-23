using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TimerBar : MonoBehaviour
{
    float fillAmount = 0;
    public Image img;
    bool isRunning;

    public void ShowProgressBar(Color col) {
        gameObject.SetActive(true);
        fillAmount = 0;
        img.color = col;
        if (!isRunning) {
            StartCoroutine(ProgressBarProgressing());
        }
    }

    private IEnumerator ProgressBarProgressing() {
        isRunning = true;
        var target = 1;
        var current = 0f;
        while (fillAmount < target) {
            
            yield return new WaitForFixedUpdate();
            current += Time.deltaTime;
            var fraction = current / (FuckYouConstants.DURATIONINSECONDS*2);
            var diff = target;
            fillAmount = (diff * fraction);
            fillAmount += fraction;
            img.fillAmount = fillAmount;
        }
        isRunning = false;
        HideProgressBar();
    }
    private void HideProgressBar() {
        gameObject.SetActive(false);
    }
}