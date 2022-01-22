using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mission : MonoBehaviour
{
    public GameObject prfGaugeBar;
    public GameObject canvas;

    RectTransform gaugeBar;

    public float height = 1.7f;
    // Start is called before the first frame update
    void Start()
    {
        // gaugeBar = Instantiate(prfGaugeBar, canvas.transform).GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        // Vector3 _gaugeBarPos = Camera.main.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y + height, 0));
        // gaugeBar.position = _gaugeBarPos;
    }
}
