using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionInteract : Interactable
{
    public float totalTime;
    public GameObject prfGaugeBar;
    private GameObject canvas;
    private enum State {
        progress,
        idle
    };
    private State state;

    GameObject gaugeBarObject;
    RectTransform gaugeBar;


    public float height = 1.7f;

    // Update is called once per frame
    void Update()
    {
        if (gaugeBar != null)
            gaugeBar.position = Camera.main.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y + height, 0));

    }

    protected override void Awake()
    {
        base.Awake();
        canvas = GameObject.Find("Canvas");
        state = State.idle;
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
        // Debug.Log($"{other.name} entered {this.name}");
    }

    protected override void OnTriggerExit2D(Collider2D other)
    {
        base.OnTriggerExit2D(other);
        Debug.Log($"{other.name} exited {this.name}");
    }

    public override void Interact()
    {
        base.Interact();
        state = State.progress;
        gaugeBarObject = Instantiate(prfGaugeBar, canvas.transform);
        gaugeBar = gaugeBarObject.GetComponent<RectTransform>();
        Vector3 _gaugeBarPos = Camera.main.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y + height, 0));
        gaugeBar.position = _gaugeBarPos;
        gaugeBar.GetComponent<GaugeMove>().InitGauge(totalTime);
        
        Debug.Log($"??? interacted with {this.name}");

        // interactor.InvokeStartInteract

        
        Invoke("FinishInteract", totalTime);
        // TODO: 몇 초 뒤에 끝난 거 알리기
    }

    public override void StopInteract()
    {
        base.StopInteract();
        if (state == State.progress) {
            Destroy(gaugeBarObject);
            state = State.idle;
        }
        
    }

    public override void FinishInteract()
    {
        if (state == State.progress) {
            base.FinishInteract();
            Destroy(this.gameObject);
        }
    }
}
