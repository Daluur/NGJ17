using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Swing : MonoBehaviour
{
    public float swingDuration = 3f;
    public float swingAngle = 80f;

	private bool leftSwing = true;
    private float swingStart;

    private void Start()
    {
        swingStart = Time.timeSinceLevelLoad;
    }

    private void Update()
    {
        var time = Time.timeSinceLevelLoad;
        var duration = time - swingStart;
        var t = duration / swingDuration;
        
        float angle = Mathf.Cos(Mathf.PI * t) * swingAngle;
        if (!leftSwing)
        {
            angle *= -1;
        }
        transform.rotation = Quaternion.Euler(0, 0, angle);

        if (duration >= swingDuration)
        {
            swingStart = time;
            leftSwing = !leftSwing;
        }
	}
}
