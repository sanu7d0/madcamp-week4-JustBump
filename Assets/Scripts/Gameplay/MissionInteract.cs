using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MissionInteract : Interactable
{
    public float minTotalTime;
    public float maxTotalTime;
    private float totalTime;
    public float minCoolTime;
    public float maxCoolTime;
    public GameObject prfGaugeBar;
    public GameObject canvas;
    private enum State {
        progress,
        idle
    };
    private State state;

    GameObject gaugeBarObject;
    RectTransform gaugeBar;
    PlayerMediator playerMediator;


    public float height = 1.7f;

    private void Awake()
    {
        canvas = GameObject.Find("MissionGaugeCanvas");
        state = State.idle;
    }

    private void Start()
    {
		if (gaugeBar != null)
		    gaugeBar.position = new Vector3(transform.position.x, transform.position.y + height, 0);
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
        // Debug.Log($"{other.name} entered {this.name}");
    }

    protected override void OnTriggerExit2D(Collider2D other)
    {
        base.OnTriggerExit2D(other);
        StopInteract();
        // Debug.Log($"{other.name} exited {this.name}");
    }

    public override void Interact(PlayerMediator playerMediator)
    {
        this.playerMediator = playerMediator;
        totalTime = Random.Range(minTotalTime, maxTotalTime);
        photonView.RPC("_Interact", RpcTarget.All, totalTime);
        Invoke("FinishInteract", totalTime);
    }
    
    [PunRPC]
    public void _Interact(float totalTime) { 
        StopInteract();
        state = State.progress;
	    gaugeBarObject = Instantiate(prfGaugeBar, canvas.transform);
        gaugeBar = gaugeBarObject.GetComponent<RectTransform>();
        Vector3 _gaugeBarPos = new Vector3(transform.position.x, transform.position.y + height, 0);
        gaugeBar.position = _gaugeBarPos;
        gaugeBar.GetComponent<GaugeMove>().InitGauge(totalTime);
    }

    public override void StopInteract()
    {
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
            gameObject.SetActive(false);
            float coolTime = Random.Range(minCoolTime, maxCoolTime);
            Invoke("recreate_mission", coolTime);
        }
    }

    public void recreate_mission() {
        this.gameObject.SetActive(true);
    }

    public override void Interact()
    {
        throw new System.NotImplementedException();
    }
}
