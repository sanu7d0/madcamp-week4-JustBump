using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class Timer: MonoBehaviour
{

    public int LimitTime = 30;
    public float startTime = 0f;
    public float elapsedTime { 
        get { return Time.time - startTime; }
     }
    private TMP_Text timer;

    public static Timer Instance; 

    private void Awake()
    {
        this.timer = GetComponent<TMP_Text>();
    }

    private void Start()
    {
        Timer.Instance = this;
    }

    private void Update()
    {
        this.timer.text = Mathf.Floor(LimitTime - elapsedTime).ToString();
    }

    async Task StartTimer(Action onEndTime) {
        startTime = Time.time;
	    await Task.Delay(LimitTime);
        onEndTime();
    }

    void endTimer() {

        gameObject.SetActive(false);
    }

}
