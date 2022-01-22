using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventTimer : MonoBehaviour
{
    private Action callback;
    private float startTime;
    private float delayTime;

    public void InitTimer(Action callback, float delayTime) {
        this.callback = callback;
        this.delayTime = delayTime;
        startTime = Time.time;
    }

    void Update()
    {
        if (Time.time >= startTime + delayTime) {
            callback();
            Destroy(gameObject);
        }
    }
}

public static class TimerExtension {
    public static void CreateEventTimer(Action callback, float delayTime) {
        GameObject go = new GameObject();
        go.AddComponent<EventTimer>().InitTimer(callback, delayTime);
    }
}
