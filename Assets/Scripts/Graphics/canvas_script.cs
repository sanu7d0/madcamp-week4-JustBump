using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class canvas_script : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Awake() {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Screen.SetResolution(720, 1280, true);
    }
}
