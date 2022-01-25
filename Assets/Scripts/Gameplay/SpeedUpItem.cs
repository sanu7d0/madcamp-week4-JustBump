using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpeedUpItem: Interactable
{
    [SerializeField]
    float speed = 1;
    [SerializeField]
    float duration = 10;

    public override void FinishInteract()
    {
        //throw new System.NotImplementedException();
    }

    public override void Interact()
    {
        //throw new System.NotImplementedException();
    }

    public override void Interact(PlayerMediator interactor)
    {
        //throw new System.NotImplementedException();
    }

    public override void StopInteract()
    {
        //throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    void Start()
    {
        // this.transform.localScale = Vector3.one * 0.3f;
    }

    // Update is called once per frame

    protected override void OnTriggerEnter2D(Collider2D other)
    {

        var interactor = other.GetComponent<PlayerMediator>();
        if (PhotonNetwork.IsMasterClient && interactor is PlayerMediator && !interactor.getSpeedIncrease()) {
		    interactor.IncreaseSpeed(speed);

            photonView.RPC("_actvieFalse", RpcTarget.All);
            TimerExtension.CreateEventTimer(() =>
            {
				interactor.InitSpeed();
            }, duration);

            TimerExtension.CreateEventTimer(() =>
            {
				photonView.RPC("_activeTrue", RpcTarget.All);
            }, (5 + Random.value * 10));

		}
    }

    [PunRPC]
    void _activeTrue() {
        gameObject.SetActive(true);
    }

    [PunRPC]
    void _actvieFalse() {
        gameObject.SetActive(false);
    }

}
