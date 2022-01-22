using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionInteract : Interactable
{

    public GameObject prfGaugeBar;
    public GameObject canvas;

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
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
        // Debug.Log($"{other.name} entered {this.name}");
    }

    protected override void OnTriggerExit2D(Collider2D other)
    {
        base.OnTriggerExit2D(other);
        // Debug.Log($"{other.name} exited {this.name}");
    }

    public override void Interact()
    {
        base.Interact();
        gaugeBar = Instantiate(prfGaugeBar, canvas.transform).GetComponent<RectTransform>();
        Vector3 _gaugeBarPos = Camera.main.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y + height, 0));
        gaugeBar.position = _gaugeBarPos;
        Debug.Log($"??? interacted with {this.name}");

        // interactor.InvokeStartInteract

        // TODO: 몇 초 뒤에 끝난 거 알리기
    }
}
