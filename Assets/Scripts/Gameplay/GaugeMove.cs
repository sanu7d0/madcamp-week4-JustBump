using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GaugeMove : MonoBehaviour
{
    private float totalTime;
    private float startTime;
    [SerializeField] private Image gaugeBar;

    private bool isStarted;
    
    private void Awake() {
        isStarted = false;
        // gaugeBar = GetComponent<Image>();
    }

    void Update()
    {
        if (isStarted)
        {
            gaugeDecrease();
        }
    }

    public void InitGauge(float totalTime)
    {
        startTime = Time.time;
        this.totalTime = totalTime;
        isStarted = true;
    }

    void gaugeDecrease() {
        float elapsedTime = Time.time - startTime;
        if (elapsedTime >= totalTime) {
            // Destroy(GameObject.Find("Box"));
            Debug.Log("Guage gone");
            Destroy(this.gameObject);
            // Destroy object
            // player 점수 추가
            return;
        }
        
        gaugeBar.fillAmount = 1 - elapsedTime / totalTime;
    }
}
