using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MissionInteract : Interactable
{
    public float totalTime;
    public float coolTime;
    public GameObject prfGaugeBar;
    private GameObject canvas;
    private enum State {
        progress,
        idle
    };
    private State state;

    GameObject gaugeBarObject;
    RectTransform gaugeBar;
    PlayerMediator playerMediator;


    public float height = 1.7f;

    protected override void Awake()
    {
        base.Awake();
        canvas = GameObject.Find("Canvas");
        state = State.idle;
    }

    private void Start()
    {
		if (gaugeBar != null)
		    gaugeBar.position = Camera.main.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y + height, 0));
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

    public override void Interact(PlayerMediator playerMediator)
    {
        base.Interact(playerMediator);
        this.playerMediator = playerMediator;
        photonView.RPC("_Interact", RpcTarget.All);
        Invoke("FinishInteract", totalTime);
    }
    
    [PunRPC]
    public void _Interact() { 
        StopInteract();
        state = State.progress;
	    gaugeBarObject = Instantiate(prfGaugeBar, canvas.transform);
        gaugeBar = gaugeBarObject.GetComponent<RectTransform>();
        Vector3 _gaugeBarPos = Camera.main.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y + height, 0));
        gaugeBar.position = _gaugeBarPos;
        gaugeBar.GetComponent<GaugeMove>().InitGauge(totalTime);
    }

    public override void StopInteract()
    {
        base.StopInteract();
        if (state == State.progress) {
            Destroy(gaugeBarObject);
            CancelInvoke("FinishInteract");
            state = State.idle;
        }
    }

    public override void FinishInteract()
    {
        playerMediator.AddScore(10);
        photonView.RPC("_FinishInteract", RpcTarget.All);
    }
    
    [PunRPC] 
    public void _FinishInteract() { 
        if (state == State.progress) {
            base.FinishInteract();
            // Destroy(this.gameObject);
            gameObject.SetActive(false);
            Invoke("recreate_mission", coolTime);
        }
    }

    public void recreate_mission() {
        this.gameObject.SetActive(true);
    }
}
