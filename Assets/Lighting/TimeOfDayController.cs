using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TimeOfDayController : MonoBehaviour
{
    DateTime timeOfDay;
    Light2D light2D;

    private void Awake()
    {
        timeOfDay = System.DateTime.Now;
        light2D = GetComponent<Light2D>();
    }

    private void FixedUpdate()
    {
        timeOfDay = timeOfDay.AddSeconds(10);
    }

    private void Update()
    {
        TimeSpan timespan = new TimeSpan(timeOfDay.Hour, timeOfDay.Minute, timeOfDay.Second);
        float val = Math.Abs(43200 - (float)timespan.TotalSeconds);
        val = val / 43200;
        val = 1 - val;
        if (val < 0.2f)
        {
            val = 0.2f;
        }
        if (val > 0.95f)
        {
            val = 0.95f;
        }
        light2D.intensity = (float)val;
    }
}
