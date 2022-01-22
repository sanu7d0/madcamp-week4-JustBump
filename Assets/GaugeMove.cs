using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GaugeMove : MonoBehaviour
{
    [SerializeField] public static float totalTime;
    private float startTime;
    [SerializeField] private Image gaugeBar;
    
    private void Awake() {
        // gaugeBar = GetComponent<Image>();
    }

    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        gaugeDecrease();
    }

    private void FixedUpdate() {
        
    }

    void gaugeDecrease() {
        float elapsedTime = Time.time - startTime;
        if (elapsedTime >= totalTime) {
            // Destroy(GameObject.Find("Box"));
            Destroy(this.gameObject);
            // Destroy object
            // player 점수 추가
            return;
        }

        
        gaugeBar.fillAmount = 1 - elapsedTime / totalTime;
    }
}
