using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightOnTimeController : MonoBehaviour
{
    [SerializeField]
    private int turnOnTime = 20;
    [SerializeField]
    private int turnOffTime = 8;

    private DateTime timeOfDay;
    private Light2D light2D;

    private void Awake()
    {
        timeOfDay = System.DateTime.Now;
        light2D = GetComponent<Light2D>();
    }

    private void FixedUpdate()
    {
        timeOfDay = timeOfDay.AddSeconds(10);
        if (timeOfDay.Hour >= turnOnTime)
        {
            light2D.enabled = true;
            return;
        }

        if (timeOfDay.Hour >= turnOffTime)
        {
            light2D.enabled = false;
            return;
        }
    }
}
